using Godot;

public partial class SpawnDungeon : Node2D
{
	[Export] public Marker2D SpawnRoomCameraPosition {get; set;} = null;
	[Export] public Marker2D HallCameraPosition {get; set;} = null;
	[Export] public float CameraTransitionSpeed {get; set; } = 5f; // Vitesse de transition

	public enum Room
	{
		SpawnRoom,
		Hall
	}

	private Room _currentRoom = Room.SpawnRoom;
	private Camera2D _camera = null;
	private Vector2 _targetCameraPosition;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_targetCameraPosition = SpawnRoomCameraPosition.GlobalPosition;
		_camera.GlobalPosition = _targetCameraPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		_camera.GlobalPosition = _camera.GlobalPosition.Lerp(
			_targetCameraPosition, 
			CameraTransitionSpeed * (float)delta
		);
	}

	private void OnSoulPortalRoomChanged(Room room)
	{
		_currentRoom = room;

		switch(_currentRoom)
		{
			case Room.SpawnRoom:
				_targetCameraPosition = SpawnRoomCameraPosition.GlobalPosition;
				break;
			case Room.Hall:
				_targetCameraPosition = HallCameraPosition.GlobalPosition;
				break;
		}
	}
}
