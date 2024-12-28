

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Heroes.World;

public class ObjectStorage
{
    // Ground
    public static Ground Grass { get; set; } = new Ground(Color.Green);
    public static Ground Road { get; set; } = new Ground(Color.SandyBrown, new GroundProperties() { PathingCost = 0.1f });
    public static Ground Stone { get; set; } = new Ground(Color.DimGray, new GroundProperties { CanPathOnto = false });

    // Resources
    public static Resource Crystals { get; set; } = new Resource(Color.Cyan);

    // Terrain

    // Textures
    public static Texture2D Cursor { get; set; }
}