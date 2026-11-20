using Godot;

// Base of every host: possession, movement, action. Each host (Human, Ogre...) inherits it
// and only overrides what makes it different. Its scene inherits host_base.tscn.
public abstract partial class Host : CharacterBody2D
{
	[Export] public float Speed {get; set;} = 100f;
	// used by the pressure plates, set per host in the inspector
	[Export] public WEIGHT Weight {get; set;} = WEIGHT.NORMAL;
	[Signal] public delegate void StateChangedEventHandler();
	public enum STATE
	{
		EMPTY,
		POSSESSED,
	}
	public enum WEIGHT
	{
		LIGHT,
		NORMAL,
		HEAVY,
	}
	// Read-only from outside: use SetState() so the StateChanged signal is emitted
	public STATE State {get; private set;} = STATE.EMPTY;
	// The interactable in front of the host, set by the interactable, used when the action is pressed
	public IInteractable TargetInteractable {get; set;} = null;
	protected AnimatedSprite2D AnimatedSprite {get; private set;} = null;

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
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AnimatedSprite.Play("idle");

		// connected in code: host_base.tscn has no script, so it can not hold the connections
		var possessionArea = GetNode<Area2D>("PossessionArea");
		possessionArea.BodyEntered += OnPossessionAreaBodyEntered;
		possessionArea.BodyExited += OnPossessionAreaBodyExited;
		StateChanged += OnStateChanged;
	}

	// movement only: the action button is read by PlayerSoul, which calls HandleAction()
	public override void _PhysicsProcess(double delta)
	{
		// to skip controls handling if is not possessed
		if (State != STATE.POSSESSED) return;

		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = ComputeVelocity(direction, delta);
		MoveAndSlide();
	}

	////////////////// LOGIC ////////////////////

	// Called by PlayerSoul when the action button is pressed while possessing
	// returns false if the host did nothing, so PlayerSoul unpossesses
	public virtual bool HandleAction()
	{
		if (TargetInteractable == null || !TargetInteractable.CanBeUsedBy(this)) return false;

		TargetInteractable.Interact(this);
		return true;
	}

	// override to move differently (inertia, acceleration...)
	protected virtual Vector2 ComputeVelocity(Vector2 direction, double delta)
	{
		return direction * Speed;
	}

	////////////////// SIGNALS RESPONSES ////////////////////

	private void OnPossessionAreaBodyEntered(Node2D body)
	{
		if (body is PlayerSoul playerSoul)
		{
			playerSoul.TargetHost ??= this;
		}
	}

	private void OnPossessionAreaBodyExited(Node2D body)
	{
		// to not clear another host targeted by the soul, nor the host it is possessing
		if (body is PlayerSoul playerSoul && playerSoul.TargetHost == this && State != STATE.POSSESSED)
		{
			playerSoul.TargetHost = null;
		}
	}

	protected virtual void OnStateChanged()
	{
		AnimatedSprite.Play(State == STATE.POSSESSED ? "idle_possessed" : "idle");
	}
}
