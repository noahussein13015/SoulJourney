using Godot;

public partial class SpawnDungeon : Node2D
{
	[Export] public Marker2D SpawnRoomCameraPosition {get; set;} = null;
	[Export] public Marker2D HallCameraPosition {get; set;} = null;
	[Export] public float CameraTransitionSpeed {get; set; } = 5f;

	private int _currentRoom = 0;
	private Camera2D _camera = null;
	private Vector2 _targetCameraPosition;
	private Vector2 _targetCameraZoom;

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_targetCameraPosition = SpawnRoomCameraPosition.GlobalPosition;
		_camera.GlobalPosition = _targetCameraPosition;
		_targetCameraZoom = new Vector2(1.1f, 1.1f);
		_camera.Zoom = _targetCameraZoom;
	}

	public override void _PhysicsProcess(double delta)
	{
		float t = CameraTransitionSpeed * (float)delta;
		
		_camera.GlobalPosition = _camera.GlobalPosition.Lerp(_targetCameraPosition, t);
		_camera.Zoom = _camera.Zoom.Lerp(_targetCameraZoom, t);
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private void OnRoomChanged(int room)
	{
		_currentRoom = room;

		switch(_currentRoom)
		{
			case 0:
				_targetCameraPosition = SpawnRoomCameraPosition.GlobalPosition;
				_targetCameraZoom = new Vector2(1.1f, 1.1f);
				break;
			case 1:
				_targetCameraPosition = HallCameraPosition.GlobalPosition;
				_targetCameraZoom = new Vector2(0.9f, 0.9f);
				break;
		}
	}
}
