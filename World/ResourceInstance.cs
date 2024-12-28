namespace Heroes.World;

public class ResourceInstance {
    public Resource ResourceType { get; }
    private int _amount;

    public ResourceInstance(Resource resource, int amount) {
        ResourceType = resource;
        _amount = amount;
    }

    public int Amount => _amount;
}