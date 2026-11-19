using System.Threading.Tasks;
using Godot;

public partial class MaterialDoor : StaticBody2D
{
	[Signal] public delegate void StateChangedEventHandler();
	[Signal] public delegate void RoomChangedEventHandler(int TargetRoom);
	[Export] public int TargetRoom {get; set;}
	[Export] public Marker2D TeleportationPoint {get; set;} = null;

	private enum STATE
	{
		OPEN,
		CLOSE
	}
	private STATE State = STATE.CLOSE;

	////////////////// ASSESSORS ////////////////////

	private void SetState(STATE value)
	{
		if (State == value)
			return;
		State = value;
		EmitSignal(SignalName.StateChanged);
	}

	////////////////// LOGIC ////////////////////
	public async void TeleportPlayer(Host playerHost)
	{
		if (TeleportationPoint != null)
		{
			EmitSignal(SignalName.RoomChanged, TargetRoom);
			await Task.Delay(500);
			playerHost.GlobalPosition = TeleportationPoint.GlobalPosition;
			playerHost.TargetDoor = null;
		}
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private async void OnStateChanged()
	{
		switch(State)
		{
			case STATE.OPEN:
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("opened");
				break;
			case STATE.CLOSE:
				await Task.Delay(500);
				// to check if the door is still closed and not reopened during the 500ms timer
				if (State == STATE.CLOSE)
				{	
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("closed");
				}
				break;
			default:
				break;
		}
	}
	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is Host playerHost)
		{
			playerHost.TargetDoor = playerHost.State == Host.STATE.POSSESSED ? this : null;
			if (playerHost.TargetDoor != null)
			{
				SetState(STATE.OPEN);
			}
		}
	}
	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is Host playerHost)
		{
			SetState(STATE.CLOSE);
			playerHost.TargetDoor = null;
		}
	}
}
