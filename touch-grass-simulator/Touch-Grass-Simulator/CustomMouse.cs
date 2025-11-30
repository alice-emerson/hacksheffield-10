namespace Touch_Grass_Simulator;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;
public class CustomMouse
{
    private Texture2D waterClickedTexture;
    private Texture2D waterStandardTexture;
    private Texture2D pinkSeeds;
    private Texture2D blueSeeds;
    private Texture2D sunSeeds;
    private Texture2D grassSeeds;
    private Texture2D cutters;
    private EMouseMode currentMode;
    private bool isMouseClicked;
    private Vector2 currentMousePos;

    public CustomMouse(Texture2D standardTexture, Texture2D clickedTexture, Texture2D pinkSeeds, 
                        Texture2D blueSeeds, Texture2D sunSeeds, Texture2D grassSeeds, 
                        Texture2D cutters, EMouseMode currentMode)
    {
        this.waterClickedTexture = clickedTexture;
        this.waterStandardTexture = standardTexture;
        this.pinkSeeds = pinkSeeds;
        this.blueSeeds = blueSeeds;
        this.sunSeeds = sunSeeds;
        this.grassSeeds = grassSeeds;
        this.cutters = cutters;
        this.currentMode = currentMode;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        switch (currentMode)
        {
            case EMouseMode.WATERING_CAN:
                {
                    if (isMouseClicked)
                    {
                        _spriteBatch.Draw(waterClickedTexture, currentMousePos, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(waterStandardTexture, currentMousePos, Color.White);
                    }
                    break;
                }
            case EMouseMode.PINK_FLOWER_SEEDS:
                {
                    _spriteBatch.Draw(pinkSeeds, currentMousePos, Color.White);
                    break;
                }
            case EMouseMode.BLUE_FLOWER_SEEDS:
                {
                    _spriteBatch.Draw(blueSeeds, currentMousePos, Color.White);
                    break;
                }
            case EMouseMode.SUNFLOWER_SEEDS:
                {
                    _spriteBatch.Draw(sunSeeds, currentMousePos, Color.White);
                    break;
                }
            case EMouseMode.GRASS_SEEDS:
                {
                    _spriteBatch.Draw(grassSeeds, currentMousePos, Color.White);
                    break;
                }
            case EMouseMode.GARDEN_CUTTERS:
                {
                    _spriteBatch.Draw(cutters, currentMousePos, Color.White);
                    break;
                }
        }

    }

    public void Update(Vector2 currentMousePos, bool isMouseClicked)
    {
        this.isMouseClicked = isMouseClicked;
        this.currentMousePos = currentMousePos;
        // TODO: Add logic for watering plants
    }
}