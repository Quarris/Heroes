using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Heroes.World;

public class Player
{

    public Color Color { get; }
    public Point Position { get; set; }
    public Player(Color color)
    {
        Color = color;
    }

    public void Draw(SpriteBatch sprites, GameTime gameTime)
    {
        sprites.FillRectangle(Position.ToVector2() * 16 + new Vector2(6, 2), new Size2(4, 12), Color);
        sprites.FillRectangle(Position.ToVector2() * 16 + new Vector2(2, 6), new Size2(12, 4), Color);
    }
}