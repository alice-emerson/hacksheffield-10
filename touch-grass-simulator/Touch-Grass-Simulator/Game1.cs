using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;


namespace Touch_Grass_Simulator;

public struct TextureGroup
{
    public Texture2D dirt;
}
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private const int NATIVE_WIN_HEIGHT = 448;
    private const int NATIVE_WIN_WIDTH = 640;
    private const int RENDER_SCALE = 2;
    private TextureGroup gameTextures;
    private TileMap backgroundTileMap;

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

        gameTextures.dirt = Content.Load<Texture2D>("Textures/DIRT");
        backgroundTileMap = new TileMap("./Tile-Maps/bg_tile_map.txt");
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
        backgroundTileMap.Draw(_spriteBatch, gameTextures);
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
