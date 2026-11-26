using Godot;

public partial class MaterialDoor : StaticBody2D, IInteractable
{
	[Signal] public delegate void StateChangedEventHandler();
	// the area runs the transition (fade, camera, teleport), the door only asks for it
	[Signal] public delegate void TeleportRequestedEventHandler(Node2D body, Vector2 destination, int targetRoom);
	[Export] public int TargetRoom {get; set;}
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
	// for a door controlled by a lever, which starts off
	[Export] public bool StartsLocked {get; set;} = false;

	private const float CloseDelay = 0.5f;

	private enum STATE
	{
		OPEN,
		CLOSE,
		LOCKED
	}
	private STATE _state = STATE.CLOSE;
	// The host currently standing in the DetectionZone
	private Host _hostInZone = null;
	private AnimatedSprite2D _animatedSprite = null;
	private Sprite2D _bars = null;

	////////////////// ACCESSORS ////////////////////

	private void SetState(STATE value)
	{
		if (_state == value) return;
		_state = value;
		EmitSignal(SignalName.StateChanged);
	}

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_bars = GetNode<Sprite2D>("Bars");
		if (StartsLocked)
			SetState(STATE.LOCKED);
	}

	////////////////// LOGIC ////////////////////

	// only a human can open a material door, and not while it is locked
	public bool CanBeUsedBy(Host host)
	{
		return host is Human && _state != STATE.LOCKED;
	}

	public void Interact(Host host)
	{
		RequestTeleport(host);
	}

	private void RequestTeleport(Host playerHost)
	{
		if (TeleportationPoint == null) return;

		EmitSignal(SignalName.TeleportRequested, playerHost, TeleportationPoint.GlobalPosition, TargetRoom);
	}

	// the door is open only while a possessed host that can use it stands in front of it
	private void UpdateState()
	{
		// only the lever can unlock the door
		if (_state == STATE.LOCKED) return;

		bool isOpen = _hostInZone != null && _hostInZone.State == Host.STATE.POSSESSED && CanBeUsedBy(_hostInZone);
		SetState(isOpen ? STATE.OPEN : STATE.CLOSE);
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	// the lever on removes the bars, then the door opens or closes as usual
	private void OnLeverToggled(bool isOn)
	{
		if (isOn)
		{
			SetState(STATE.CLOSE);
			UpdateState();
		}
		else
		{
			SetState(STATE.LOCKED);
		}
	}

	private async void OnStateChanged()
	{
		_bars.Visible = _state == STATE.LOCKED;

		switch (_state)
		{
			case STATE.LOCKED:
				_animatedSprite.Play("closed");
				break;
			case STATE.OPEN:
				_animatedSprite.Play("opened");
				break;
			case STATE.CLOSE:
				await ToSignal(GetTree().CreateTimer(CloseDelay), SceneTreeTimer.SignalName.Timeout);
				// to check if the door is still closed and not reopened during the delay
				if (_state == STATE.CLOSE)
					_animatedSprite.Play("closed");
				break;
		}
	}

	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is not Host playerHost) return;

		_hostInZone = playerHost;
		playerHost.TargetInteractable = this;
		// to open/close the door when the host is possessed or released while in the zone
		playerHost.StateChanged += UpdateState;
		UpdateState();
	}

	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is not Host playerHost || playerHost != _hostInZone) return;

		playerHost.StateChanged -= UpdateState;
		if (playerHost.TargetInteractable == this)
			playerHost.TargetInteractable = null;
		_hostInZone = null;
		UpdateState();
	}
}
