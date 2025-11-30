using Grpc.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;


namespace Touch_Grass_Simulator;

public struct TextureGroup
{
    public Texture2D dirt;

    public Texture2D short_grass;
    public Texture2D medium_grass;
    public Texture2D tall_grass;

    public Texture2D short_blue_flower;
    public Texture2D medium_blue_flower;
    public Texture2D tall_blue_flower;

    public Texture2D short_pink_flower;
    public Texture2D medium_pink_flower;
    public Texture2D tall_pink_flower;

    public Texture2D short_sun_flower;
    public Texture2D medium_sun_flower;
    public Texture2D tall_sun_flower;
}

public struct SessionStats
{
    public int total_plants;
    public int total_blue_flowers;
    public int total_pink_flowers;
    public int total_sun_flowers;
    public int total_grass;
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _gameRenderTarget;
    private RenderTarget2D _uiRenderTarget;
    public const int GAME_WIN_HEIGHT = 448;
    public const int GAME_WIN_WIDTH = 640;
    public const int UI_WIN_HEIGHT = 448;
    public const int UI_WIN_WIDTH = 64;
    public const int RENDER_SCALE = 2;
    private TextureGroup gameTextures;
    private TileMap backgroundTileMap;
    private TileMap foregroundTileMap;
    private CustomMouse customMouse;
    private EMouseMode currentToolOption = EMouseMode.WATERING_CAN;
    private UiPanel uiPanel;
    private InfluxDB db = new InfluxDB();
    private SessionStats stats;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GAME_WIN_WIDTH * RENDER_SCALE + UI_WIN_WIDTH * RENDER_SCALE;
        _graphics.PreferredBackBufferHeight = GAME_WIN_HEIGHT * RENDER_SCALE;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _gameRenderTarget = new RenderTarget2D(GraphicsDevice, GAME_WIN_WIDTH, GAME_WIN_HEIGHT);
        _uiRenderTarget = new RenderTarget2D(GraphicsDevice, UI_WIN_WIDTH, UI_WIN_HEIGHT);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Game map textures

        gameTextures.dirt = Content.Load<Texture2D>("Textures/DIRT");

        gameTextures.short_blue_flower = Content.Load<Texture2D>("Textures/small_b_flower");
        gameTextures.medium_blue_flower = Content.Load<Texture2D>("Textures/med_b_flower");;
        gameTextures.tall_blue_flower = Content.Load<Texture2D>("Textures/long_b_flower");;

        gameTextures.short_pink_flower = Content.Load<Texture2D>("Textures/small_pink_flower");
        gameTextures.medium_pink_flower = Content.Load<Texture2D>("Textures/med_pink_flower");
        gameTextures.tall_pink_flower = Content.Load<Texture2D>("Textures/long_pink_flower");

        gameTextures.short_sun_flower = Content.Load<Texture2D>("Textures/small_sunflower");
        gameTextures.medium_sun_flower= Content.Load<Texture2D>("Textures/med_sunflower");
        gameTextures.tall_sun_flower = Content.Load<Texture2D>("Textures/tall_sunflower");

        gameTextures.short_grass = Content.Load<Texture2D>("Textures/small_grass");
        gameTextures.medium_grass = Content.Load<Texture2D>("Textures/medium_grass");
        gameTextures.tall_grass = Content.Load<Texture2D>("Textures/long_grass");

        backgroundTileMap = new TileMap("./Tile-Maps/bg_tile_map.txt");
        foregroundTileMap = new TileMap("./Tile-Maps/fg_tile_map.txt");

        // Tool textures

        Texture2D defaultMouseTexture = Content.Load<Texture2D>("Textures/void");
        Mouse.SetCursor(MouseCursor.FromTexture2D(defaultMouseTexture, 0, 0));

        Texture2D wateringCanOn = Content.Load<Texture2D>("Textures/can_watering");
        Texture2D wateringCanOff = Content.Load<Texture2D>("Textures/can_still");
        Texture2D wateringCanIcon = Content.Load<Texture2D>("Textures/can_icon");

        // Temp tool textures
        Texture2D pinkSeeds = Content.Load<Texture2D>("Textures/small_pink_flower");
        Texture2D blueSeeds = Content.Load<Texture2D>("Textures/small_b_flower");
        Texture2D sunSeeds = Content.Load<Texture2D>("Textures/small_sunflower");
        Texture2D grassSeeds = Content.Load<Texture2D>("Textures/small_grass");
        Texture2D cutters = Content.Load<Texture2D>("Textures/cutter-temp");
        Texture2D hand = Content.Load<Texture2D>("Textures/hand-temp");

        customMouse = new CustomMouse(wateringCanOff, wateringCanOn, pinkSeeds, blueSeeds, sunSeeds, grassSeeds, cutters, hand, currentToolOption);

        Texture2D uiFrame = Content.Load<Texture2D>("Textures/UI_Panel");
        Texture2D itemFrame = Content.Load<Texture2D>("Textures/Item_Frame");

        uiPanel = new UiPanel(uiFrame, itemFrame, pinkSeeds, blueSeeds, sunSeeds, grassSeeds, wateringCanIcon, cutters, hand);

        stats.total_blue_flowers = 0;
        stats.total_grass = 0;
        stats.total_pink_flowers = 0;
        stats.total_sun_flowers = 0;
        stats.total_plants = 0;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        MouseState currentMouseState = Mouse.GetState();
        bool isMouseClicked;
        if (currentMouseState.LeftButton == ButtonState.Pressed) isMouseClicked = true;
        else isMouseClicked = false;
        customMouse.Update(new Vector2(currentMouseState.X, currentMouseState.Y), currentToolOption, isMouseClicked);

        currentToolOption = uiPanel.Update(currentMouseState, currentToolOption);

        stats = foregroundTileMap.Update(currentMouseState, currentToolOption, gameTime.ElapsedGameTime.Milliseconds, db, stats);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Render Game
        GraphicsDevice.SetRenderTarget(_gameRenderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        backgroundTileMap.Draw(_spriteBatch, gameTextures);
        foregroundTileMap.Draw(_spriteBatch, gameTextures);
        _spriteBatch.End();

        // Render UI
        GraphicsDevice.SetRenderTarget(_uiRenderTarget);
        _spriteBatch.Begin();
        uiPanel.Draw(_spriteBatch);
        _spriteBatch.End();

        // Render scaled game
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_gameRenderTarget, new Rectangle(0,0,GAME_WIN_WIDTH*RENDER_SCALE, GAME_WIN_HEIGHT*RENDER_SCALE), Color.White);
        _spriteBatch.Draw(_uiRenderTarget, new Rectangle(GAME_WIN_WIDTH*RENDER_SCALE,0,UI_WIN_WIDTH*RENDER_SCALE, UI_WIN_HEIGHT*RENDER_SCALE), Color.White);
        customMouse.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
