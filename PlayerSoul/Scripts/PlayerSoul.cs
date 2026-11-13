using Godot;

public partial class PlayerSoul : CharacterBody2D
{
	[Export] public float Speed {get; set;} = 200f;
	[Export] public float FloatingEffectAttenuation {get; set;} = 5f;
	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
			"move_down"
		);
		var targetVelocity = direction * Speed;
		Velocity = Velocity.Lerp(targetVelocity, FloatingEffectAttenuation * (float)delta);
		GD.Print("Position actuelle : ", Position);
		MoveAndSlide();
	}
}
