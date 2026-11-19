using System.Threading.Tasks;
using Godot;

public partial class SoulPortal : StaticBody2D
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

	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			// To make the portal unaccessible if an host is near
			playerSoul.TargetPortal = playerSoul.TargetHost == null ? this : null;
		}
	}
	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			playerSoul.TargetPortal = null;
		}
	}

}
