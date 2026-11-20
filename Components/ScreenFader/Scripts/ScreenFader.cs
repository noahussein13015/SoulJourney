using Godot;

// Full screen black overlay used to hide the teleportations during room transitions
public partial class ScreenFader : CanvasLayer
{
	[Export] public float FadeDuration {get; set;} = 0.2f;

	private ColorRect _overlay = null;
	private Tween _tween = null;

	////////////////// GODOT ////////////////////

	public override void _Ready()
	{
		_overlay = GetNode<ColorRect>("Overlay");
		_overlay.Modulate = Colors.Transparent;
	}

	////////////////// LOGIC ////////////////////

	// Awaitable: fades the screen to black
	public SignalAwaiter FadeOut() => FadeTo(1f);

	// Awaitable: fades the screen back from black
	public SignalAwaiter FadeIn() => FadeTo(0f);

	private SignalAwaiter FadeTo(float alpha)
	{
		// to never have two fades fighting over the overlay
		_tween?.Kill();
		_tween = CreateTween();
		_tween.TweenProperty(_overlay, "modulate:a", alpha, FadeDuration);
		return ToSignal(_tween, Tween.SignalName.Finished);
	}
}
