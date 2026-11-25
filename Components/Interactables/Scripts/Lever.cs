using Godot;

// Switched on/off by a possessed host with the action button. It only emits Toggled:
// the objects it controls (gate, bridge...) are connected to it in the level scene
public partial class Lever : StaticBody2D, IInteractable
{
	[Signal] public delegate void ToggledEventHandler(bool isOn);
	private bool _isOn = false;
	// The host currently standing in the DetectionZone
	private Host _hostInZone = null;
	private AnimatedSprite2D _animatedSprite = null;

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		UpdateSprite();
	}

	////////////////// LOGIC ////////////////////

	// only a human can pull a lever
	public bool CanBeUsedBy(Host host)
	{
		return host is Human;
	}

	public void Interact(Host host)
	{
		_isOn = !_isOn;
		UpdateSprite();
		EmitSignal(SignalName.Toggled, _isOn);
	}

	private void UpdateSprite()
	{
		_animatedSprite.Play(_isOn ? "on" : "off");
	}

	////////////////// SIGNAL RESPONSES ////////////////////

	private void OnDetectionZoneBodyEntered(Node2D body)
	{
		if (body is not Host playerHost) return;

		_hostInZone = playerHost;
		playerHost.TargetInteractable = this;
	}

	private void OnDetectionZoneBodyExited(Node2D body)
	{
		if (body is not Host playerHost || playerHost != _hostInZone) return;

		// to not clear another interactable entered right after this one
		if (playerHost.TargetInteractable == this)
			playerHost.TargetInteractable = null;
		_hostInZone = null;
	}
}
