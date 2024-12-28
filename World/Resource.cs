using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Heroes.World;

public class Resource
{
    public Color Color { get; }
    public Resource(Color color)
    {
        Color = color;
    }

    public void Draw(SpriteBatch sprites, Point position, GameTime gameTime)
    {
        sprites.FillRectangle(position.ToVector2() * 16 + new Vector2(4), new Size2(8, 8), Color);
    }
}