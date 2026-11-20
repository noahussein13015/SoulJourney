using Godot;

public partial class PlayerSoul : CharacterBody2D
{
	[Export] public float Speed {get; set;} = 200f;
	[Export] public float FloatingEffectAttenuation {get; set;} = 5f;
	[Signal] public delegate void StateChangedEventHandler();
	private enum STATE
	{
		SOUL,
		IN_HOST
	}
	private STATE _state = STATE.SOUL;

	// The current Host with Player in its PossessionZone, set by the host when Player enters the zone
	public Host TargetHost {get; set;} = null;
	public SoulPortal TargetPortal {get; set;} = null;
	private AnimatedSprite2D _animatedSprite = null;
	private CollisionShape2D _collisionShape = null;

	////////////////// ACCESSORS ////////////////////

	private void SetState(STATE value)
	{
		if (_state == value) return;
		_state = value;
		EmitSignal(SignalName.StateChanged);
	}

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("action_button"))
		{
			if (TargetHost != null)
			{
				if (_state == STATE.SOUL)
					Possess();
				else
					Unpossess();
			}
			if (TargetPortal != null)
			{
				TargetPortal.TeleportPlayer(this);
			}
		}

		// to skip controls handling if is possessing an host
		if (_state != STATE.SOUL) return;

		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		var targetVelocity = direction * Speed;
		Velocity = Velocity.Lerp(targetVelocity, FloatingEffectAttenuation * (float)delta);
		MoveAndSlide();
	}

	////////////////// LOGIC ////////////////////

	// Possesses the current TargetHost
	private void Possess()
	{
		TargetHost.SetState(Host.STATE.POSSESSED);
		SetState(STATE.IN_HOST);
	}

	// Unpossesses the current TargetHost
	private void Unpossess()
	{
		TargetHost.SetState(Host.STATE.EMPTY);
		SetState(STATE.SOUL);
	}

	////////////////// SIGNALS RESPONSES ////////////////////

	// if is in host, hide the sprite, deactivate the collisions
	private void OnStateChanged()
	{
		if (_state == STATE.IN_HOST)
		{
			_collisionShape.SetDeferred("disabled", true);
			_animatedSprite.Hide();
		}
		else
		{
			GlobalPosition = TargetHost.GlobalPosition;
			_collisionShape.SetDeferred("disabled", false);
			_animatedSprite.Show();
		}
	}
}
