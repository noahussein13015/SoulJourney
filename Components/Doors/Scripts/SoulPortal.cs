using System.Runtime;
using Godot;

public partial class SoulPortal : StaticBody2D
{
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
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
