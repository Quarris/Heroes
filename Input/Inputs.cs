using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Heroes.Input;

public class Inputs
{
    private static KeyboardStateExtended _keyboard;
    private static MouseStateExtended _mouse;

    public static KeyboardStateExtended Keyboard => _keyboard;
    public static MouseStateExtended Mouse => _mouse;

    public static void Update(GameTime gameTime)
    {
        _keyboard = KeyboardExtended.GetState();
        _mouse = MouseExtended.GetState();
    }
}