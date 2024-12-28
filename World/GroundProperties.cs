namespace Heroes.World;

public class GroundProperties
{
    public static readonly GroundProperties Default = new GroundProperties();
    public float PathingCost { get; set; } = 1f;
    public bool CanPathOnto { get; set; } = true;
    public GroundProperties()
    {

    }
}