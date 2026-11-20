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
	// The current SoulPortal overlapping the Player, set by the portal when Player enters it
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

	// disabled by the area during room transitions, which locks every player input
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("action_button"))
			HandleAction();

		// to skip controls handling if is possessing an host
		if (_state != STATE.SOUL) return;

		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		var targetVelocity = direction * Speed;
		Velocity = Velocity.Lerp(targetVelocity, FloatingEffectAttenuation * (float)delta);
		MoveAndSlide();
	}

	////////////////// LOGIC ////////////////////

	// Single entry point of the action button: one press triggers exactly one action
	private void HandleAction()
	{
		if (_state == STATE.SOUL)
		{
			// a near host has priority over the portal
			if (TargetHost != null)
				Possess();
			else
				TargetPortal?.RequestTeleport(this);
		}
		else
		{
			// a door in front of the host has priority over unpossessing
			if (TargetHost.TargetDoor != null)
				TargetHost.TargetDoor.RequestTeleport(TargetHost);
			else
				Unpossess();
		}
	}

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
			// to not keep the momentum from before the possession
			Velocity = Vector2.Zero;
			_collisionShape.SetDeferred("disabled", false);
			_animatedSprite.Show();
		}
	}
}
