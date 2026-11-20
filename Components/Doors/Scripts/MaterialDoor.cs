using Godot;

public partial class MaterialDoor : StaticBody2D
{
	[Signal] public delegate void StateChangedEventHandler();
	// the area runs the transition (fade, camera, teleport), the door only asks for it
	[Signal] public delegate void TeleportRequestedEventHandler(Node2D body, Vector2 destination, int targetRoom);
	[Export] public int TargetRoom {get; set;}
	[Export] public Marker2D TeleportationPoint {get; set;} = null;

	private const float CloseDelay = 0.5f;

	private enum STATE
	{
		OPEN,
		CLOSE
	}
	private STATE _state = STATE.CLOSE;
	// The host currently standing in the DetectionZone
	private Host _hostInZone = null;
	private AnimatedSprite2D _animatedSprite = null;

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
	}

	////////////////// LOGIC ////////////////////

	public void RequestTeleport(Host playerHost)
	{
		if (TeleportationPoint == null) return;

		EmitSignal(SignalName.TeleportRequested, playerHost, TeleportationPoint.GlobalPosition, TargetRoom);
	}

	// the door is open only while a possessed host stands in front of it
	private void UpdateState()
	{
		bool isOpen = _hostInZone != null && _hostInZone.State == Host.STATE.POSSESSED;
		SetState(isOpen ? STATE.OPEN : STATE.CLOSE);
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private async void OnStateChanged()
	{
		switch (_state)
		{
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
		playerHost.TargetDoor = this;
		// to open/close the door when the host is possessed or released while in the zone
		playerHost.StateChanged += UpdateState;
		UpdateState();
	}

	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is not Host playerHost || playerHost != _hostInZone) return;

		playerHost.StateChanged -= UpdateState;
		if (playerHost.TargetDoor == this)
			playerHost.TargetDoor = null;
		_hostInZone = null;
		UpdateState();
	}
}
