using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Heroes.World;

public class Ground
{
    public GroundProperties Properties { get; }
    public Color Color { get; }
    public Ground(Color color, GroundProperties properties)
    {
        Color = color;
        Properties = properties;
    }

    public Ground(Color color) : this(color, GroundProperties.Default) {}

    public void Draw(SpriteBatch sprites, Point position, GameTime gameTime)
    {
        sprites.FillRectangle(position.ToVector2() * 16, new Size2(16, 16), Color);
    }
}