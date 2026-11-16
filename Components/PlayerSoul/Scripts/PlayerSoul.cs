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
	private Sprite2D _sprite = null;
	
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
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("action_button") && TargetHost != null)
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
	
	// if is in host, hide the sprite
	private void OnStateChanged()
	{
		if (State == STATE.IN_HOST)
		{
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
			_sprite.Hide();
		}
		else
		{
			GlobalPosition = TargetHost.GlobalPosition;
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
			_sprite.Show();
		}
	}
}
