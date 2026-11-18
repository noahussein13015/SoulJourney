using Godot;

public partial class SoulPortal : StaticBody2D
{
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
	[Export] public SpawnDungeon.Room TargetRoom {get; set;}
	[Signal] public delegate void RoomChangedEventHandler(SpawnDungeon.Room TargetRoom);
	
	////////////////// LOGIC ////////////////////
	
	public void TeleportPlayer(PlayerSoul playerSoul)
	{
		if (TeleportationPoint != null)
		{
			playerSoul.GlobalPosition = TeleportationPoint.GlobalPosition;
			playerSoul.TargetPortal = null;
			EmitSignal(SignalName.RoomChanged, (int)TargetRoom);
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
