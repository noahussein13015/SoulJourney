using System.Threading.Tasks;
using Godot;

public partial class MaterialDoor : StaticBody2D
{
	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is Host playerHost)
		{
			playerHost.TargetDoor = playerHost.State == Host.STATE.POSSESSED ? this : null;
			if (playerHost.TargetDoor != null)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("opened");	
			}
		}
	}
	private async void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is Host playerHost)
		{
			playerHost.TargetDoor = null;
			await Task.Delay(500);
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("closed");
		}
	}
}
