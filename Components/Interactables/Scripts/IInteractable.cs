// Anything a possessed host can use with the action button (door, lever, cracked wall...)
// the object decides which hosts can use it, so adding an object never changes the hosts
public interface IInteractable
{
	bool CanBeUsedBy(Host host);
	void Interact(Host host);
}
