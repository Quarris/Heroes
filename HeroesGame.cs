using Heroes.Input;
using Heroes.Client.Ui;
using Heroes.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Heroes.Client;
using MonoGame.Extended;
using System;

namespace Heroes;

public class HeroesGame : Game
{
    public static HeroesGame Instance { get; private set; }
    private GraphicsDeviceManager _graphicsDeviceManager;
    private ClientState _clientState;
    public Map Map { get; private set; } = new Map(20, 10);

    public Hud Hud { get; private set; } = new Hud();

    public HeroesGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
    }

    protected override void Initialize()
    {
        base.Initialize();
        _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
        _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
        _clientState = new ClientState(Window, new SpriteBatch(GraphicsDevice), _graphicsDeviceManager);
        ObjectStorage.Cursor = Instance.Content.Load<Texture2D>("Cursor");
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        Inputs.Update(gameTime);
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);

        _clientState.Update(gameTime);
        Map.Update(_clientState, gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _clientState.BeginDraw();
        Map.Draw(_clientState, gameTime);
        _clientState.EndDraw();
        base.Draw(gameTime);
    }
}
