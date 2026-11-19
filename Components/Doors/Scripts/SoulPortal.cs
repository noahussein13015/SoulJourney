using System.Threading.Tasks;
using Godot;

public partial class SoulPortal : Area2D
{
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
	[Export] public int TargetRoom {get; set;}
	[Signal] public delegate void RoomChangedEventHandler(int TargetRoom);
	
	////////////////// LOGIC ////////////////////
	
	public async void TeleportPlayer(PlayerSoul playerSoul)
	{
		if (TeleportationPoint != null)
		{
			EmitSignal(SignalName.RoomChanged, TargetRoom);
			await Task.Delay(500);
			playerSoul.GlobalPosition = TeleportationPoint.GlobalPosition;
			playerSoul.TargetPortal = null;
		}
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private void OnBodyEntered(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			// To make the portal unaccessible if an host is near
			playerSoul.TargetPortal = playerSoul.TargetHost == null ? this : null;
		}
	}
	private void OnBodyExited(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			playerSoul.TargetPortal = null;
		}
	}

}
