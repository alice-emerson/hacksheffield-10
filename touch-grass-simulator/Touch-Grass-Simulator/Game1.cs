using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Touch_Grass_Simulator;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private const int NATIVE_WIN_HEIGHT = 224;
    private const int NATIVE_WIN_WIDTH = 320;
    private const int RENDER_SCALE = 4;
    private Texture2D dirtTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = NATIVE_WIN_WIDTH * RENDER_SCALE;
        _graphics.PreferredBackBufferHeight = NATIVE_WIN_HEIGHT * RENDER_SCALE;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _renderTarget = new RenderTarget2D(GraphicsDevice, NATIVE_WIN_WIDTH, NATIVE_WIN_HEIGHT);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        dirtTexture = Content.Load<Texture2D>("Textures/DIRT");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(dirtTexture, Vector2.Zero, Color.White);
        _spriteBatch.End();

        // TODO: Add your drawing code here
        // Render scaled game
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, new Rectangle(0,0,NATIVE_WIN_WIDTH*RENDER_SCALE,NATIVE_WIN_HEIGHT*RENDER_SCALE), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
