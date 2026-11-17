using System.Numerics;
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
	private STATE State = STATE.SOUL;
	
	// The current Host with Player in it's PossessionZone, setted by the host when Player enters the zone
	public Host TargetHost {get; set;} = null;
	public SoulPortal TargetPortal {get; set;} = null;
	private AnimatedSprite2D _animatedSprite = null;
	
	////////////////// ASSESSORS ////////////////////
	private void SetState(STATE value)
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
		if (Input.IsActionJustPressed("action_button"))
		{
			if (TargetHost != null)
			{
				if (State == STATE.SOUL)
				{
					Possess();
				}
				else 
				{
					Unpossess();
				}
			}
			if (TargetPortal != null)
			{
				GetThroughPortal();
			}
		}
		// to skip controls handling if is possessing an host
		if (State != STATE.SOUL) return;
		var direction = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
			"move_down"
		);
		var targetVelocity = direction * Speed;
		Velocity = Velocity.Lerp(targetVelocity, FloatingEffectAttenuation * (float)delta);
		MoveAndSlide();
	}

	////////////////// LOGIC ////////////////////
	
	// Possess the current targetHost

	private void GetThroughPortal()
	{
		var endpoint = TargetPortal.TeleportationPoint;

		if (endpoint != null)
		{
			GlobalPosition = endpoint.GlobalPosition;
		}
	}
	private void Possess()
	{
		TargetHost.SetState(Host.STATE.POSSESSED);
		SetState(STATE.IN_HOST);
	}

	// Unpossesses the current targetHost
	private void Unpossess()
	{
		TargetHost.SetState(Host.STATE.EMPTY);
		SetState(STATE.SOUL);
	}

	////////////////// SIGNALS RESPONSES ////////////////////
	
	// if is in host, hide the sprite, deactivate the collisions
	private void OnStateChanged()
	{
		if (State == STATE.IN_HOST)
		{
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
			GetNode<Camera2D>("Camera2D").Enabled = false;
			_animatedSprite.Hide();
		}
		else
		{
			GlobalPosition = TargetHost.GlobalPosition;
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
			GetNode<Camera2D>("Camera2D").Enabled = true;
			_animatedSprite.Show();
		}
	}
}
