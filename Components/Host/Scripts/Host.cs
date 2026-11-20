using Godot;

public partial class Host : CharacterBody2D
{
	[Export] public float Speed {get; set;} = 100f;
	[Signal] public delegate void StateChangedEventHandler();
	public enum STATE
	{
		EMPTY,
		POSSESSED,
	}
	// Read-only from outside: use SetState() so the StateChanged signal is emitted
	public STATE State {get; private set;} = STATE.EMPTY;
	public MaterialDoor TargetDoor {get; set;} = null;
	private AnimatedSprite2D _animatedSprite = null;

	////////////////// ACCESSORS ////////////////////

	public void SetState(STATE value)
	{
		if (State == value) return;
		State = value;
		EmitSignal(SignalName.StateChanged);
	}

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		// to skip controls handling if is not possessed
		if (State != STATE.POSSESSED) return;

		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (Input.IsActionJustPressed("action_button") && TargetDoor != null)
		{
			TargetDoor.TeleportPlayer(this);
		}
		Velocity = direction * Speed;
		MoveAndSlide();
	}

	////////////////// SIGNALS RESPONSES ////////////////////

	private void OnPossessionAreaBodyEntered(Node2D body)
	{
		if (body is PlayerSoul playerSoul && playerSoul.TargetHost == null)
		{
			playerSoul.TargetHost = this;
		}
	}

	private void OnPossessionAreaBodyExited(Node2D body)
	{
		if (body is PlayerSoul playerSoul && State != STATE.POSSESSED)
		{
			playerSoul.TargetHost = null;
		}
	}

	private void OnStateChanged()
	{
		_animatedSprite.Play(State == STATE.POSSESSED ? "idle_possessed" : "idle");
	}
}
