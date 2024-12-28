using System;
using Heroes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Heroes.Client;

public class ClientState
{
    private readonly SpriteBatch _sprites;
    private readonly GraphicsDeviceManager _deviceManager;
    //public float Scale { get; set; } = 1;
    public SpriteBatch Sprites => _sprites;
    private Vector2 _scaledMouse;
    public Vector2 MousePosition => _scaledMouse;
    public Point HoveredPoint => MousePosition.ToPoint();
    private Camera _camera;
    public Camera Camera => _camera;


    public ClientState(GameWindow window, SpriteBatch sprites, GraphicsDeviceManager manager)
    {
        _sprites = sprites;
        _deviceManager = manager;
        
        var viewportAdapter = new BoxingViewportAdapter(window, manager.GraphicsDevice, 640, 480);
        _camera = new Camera(viewportAdapter);
    }

    public void Update(GameTime gameTime)
    {
        float scroll = Inputs.Mouse.DeltaScrollWheelValue;
        if (scroll != 0)
        {
            _camera.Zoom += scroll / 120 / 2;
        }
        
        _scaledMouse = _camera.ScreenToWorld(Inputs.Mouse.Position.ToVector2()) / Constants.TileSize;

        if (Inputs.Keyboard.WasKeyJustUp(Keys.F12))
        {
            _deviceManager.IsFullScreen = !_deviceManager.IsFullScreen;
            _deviceManager.ApplyChanges();
        }

        _camera.Update(gameTime);
    }

    public void BeginDraw()
    {
        Sprites.Begin(transformMatrix: _camera.Transform);
    }

    public void EndDraw()
    {
        Sprites.End();
    }
}