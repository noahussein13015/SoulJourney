using Godot;

public partial class SpawnDungeon : Node2D
{
	[Export] public Marker2D SpawnRoomCameraPosition {get; set;} = null;
	[Export] public Marker2D HallCameraPosition {get; set;} = null;

	public enum Room
	{
		SpawnRoom,
		Hall
	}
	private Room _currentRoom = Room.SpawnRoom;
	private Camera2D _camera = null;
    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("Camera2D");
		_camera.GlobalPosition = SpawnRoomCameraPosition.GlobalPosition;
    }

	private void OnSoulPortalRoomChanged(Room room)
	{
		_currentRoom = room;

		switch(_currentRoom)
		{
			case Room.SpawnRoom:
				_camera.GlobalPosition = SpawnRoomCameraPosition.GlobalPosition;
				break;
			case Room.Hall:
				_camera.GlobalPosition = HallCameraPosition.GlobalPosition;
				break;
		}
	}
}
