using Godot;

public partial class SpawnDungeon : Node2D
{
	// One camera position per room, the index is the room number used by portals and doors
	[Export] public Godot.Collections.Array<Marker2D> RoomCameraPositions {get; set;} = new();
	[Export] public PlayerSoul PlayerSoul {get; set;} = null;
	// Distance (in pixels) the camera slides from when entering a room
	[Export] public float SlideDistance {get; set;} = 12f;
	[Export] public float SlideDuration {get; set;} = 0.35f;

	private int _currentRoom = 0;
	private bool _isTransitioning = false;
	private Camera2D _camera = null;
	private ScreenFader _screenFader = null;

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_screenFader = GetNode<ScreenFader>("ScreenFader");
		// integer zoom only, to keep the pixel art crisp
		_camera.Zoom = Vector2.One;
		_camera.GlobalPosition = RoomCameraPositions[_currentRoom].GlobalPosition;
	}

	////////////////// LOGIC ////////////////////

	// Fade out -> teleport the body and place the camera -> fade in while sliding the camera into the room
	private async void PlayTransition(Node2D body, Vector2 destination, int targetRoom)
	{
		// to ignore requests while a transition is already running (no double teleport)
		if (_isTransitioning) return;
		_isTransitioning = true;
		SetInputLocked(body, true);

		await _screenFader.FadeOut();

		Vector2 slideDirection = GetAxisDirection(destination - body.GlobalPosition);
		body.GlobalPosition = destination;
		if (body is CharacterBody2D characterBody)
			characterBody.Velocity = Vector2.Zero;

		_currentRoom = targetRoom;
		Vector2 roomCameraPosition = RoomCameraPositions[_currentRoom].GlobalPosition;
		_camera.GlobalPosition = roomCameraPosition - slideDirection * SlideDistance;

		var fadeIn = _screenFader.FadeIn();
		var slideTween = CreateTween()
			.SetTrans(Tween.TransitionType.Cubic)
			.SetEase(Tween.EaseType.Out);
		slideTween.TweenProperty(_camera, "global_position", roomCameraPosition, SlideDuration);
		await ToSignal(slideTween, Tween.SignalName.Finished);
		await fadeIn;

		SetInputLocked(body, false);
		_isTransitioning = false;
	}

	// the soul handles every input (even in a host) and the body is frozen so it can not move
	private void SetInputLocked(Node2D body, bool locked)
	{
		PlayerSoul.SetPhysicsProcess(!locked);
		body.SetPhysicsProcess(!locked);
	}

	// to slide strictly up/down/left/right, along the main axis of the travel
	private static Vector2 GetAxisDirection(Vector2 travel)
	{
		if (travel == Vector2.Zero) return Vector2.Zero;

		return Mathf.Abs(travel.X) > Mathf.Abs(travel.Y)
			? new Vector2(Mathf.Sign(travel.X), 0)
			: new Vector2(0, Mathf.Sign(travel.Y));
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private void OnTeleportRequested(Node2D body, Vector2 destination, int targetRoom)
	{
		PlayTransition(body, destination, targetRoom);
	}
}
