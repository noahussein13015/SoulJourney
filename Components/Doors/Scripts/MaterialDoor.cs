using System.Threading.Tasks;
using Godot;

public partial class MaterialDoor : StaticBody2D
{
	[Signal] public delegate void StateChangedEventHandler();
	[Signal] public delegate void RoomChangedEventHandler(int TargetRoom);
	[Export] public int TargetRoom {get; set;}
	[Export] public Marker2D TeleportationPoint {get; set;} = null;

	private const int DelayMs = 500;

	private enum STATE
	{
		OPEN,
		CLOSE
	}
	private STATE _state = STATE.CLOSE;
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

	public async void TeleportPlayer(Host playerHost)
	{
		if (TeleportationPoint == null) return;

		EmitSignal(SignalName.RoomChanged, TargetRoom);
		await Task.Delay(DelayMs);
		playerHost.GlobalPosition = TeleportationPoint.GlobalPosition;
		playerHost.TargetDoor = null;
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
				await Task.Delay(DelayMs);
				// to check if the door is still closed and not reopened during the delay
				if (_state == STATE.CLOSE)
					_animatedSprite.Play("closed");
				break;
		}
	}

	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is not Host playerHost) return;

		bool isPossessed = playerHost.State == Host.STATE.POSSESSED;
		playerHost.TargetDoor = isPossessed ? this : null;
		if (isPossessed)
			SetState(STATE.OPEN);
	}

	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is not Host playerHost) return;

		SetState(STATE.CLOSE);
		playerHost.TargetDoor = null;
	}
}
