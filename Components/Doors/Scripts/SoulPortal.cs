using Godot;

public partial class SoulPortal : Area2D
{
	[Export] public Marker2D TeleportationPoint {get; set;} = null;
	[Export] public int TargetRoom {get; set;}
	// the area runs the transition (fade, camera, teleport), the portal only asks for it
	[Signal] public delegate void TeleportRequestedEventHandler(Node2D body, Vector2 destination, int targetRoom);

	////////////////// LOGIC ////////////////////

	public void RequestTeleport(PlayerSoul playerSoul)
	{
		if (TeleportationPoint == null) return;

		EmitSignal(SignalName.TeleportRequested, playerSoul, TeleportationPoint.GlobalPosition, TargetRoom);
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	// only tracks the overlap, the host/portal priority is decided by PlayerSoul when the action is pressed
	private void OnBodyEntered(Node2D body)
	{
		if (body is not PlayerSoul playerSoul) return;

		playerSoul.TargetPortal = this;
	}

	private void OnBodyExited(Node2D body)
	{
		// to not clear a portal entered right after this one
		if (body is not PlayerSoul playerSoul || playerSoul.TargetPortal != this) return;

		playerSoul.TargetPortal = null;
	}
}
