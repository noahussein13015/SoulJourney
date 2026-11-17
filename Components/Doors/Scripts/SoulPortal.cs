using Godot;

public partial class SoulPortal : StaticBody2D
{
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			playerSoul.TargetPortal = this;
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
