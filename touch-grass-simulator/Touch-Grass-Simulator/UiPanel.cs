namespace Touch_Grass_Simulator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;
public class UiPanel
{
    private Texture2D panelFrame;
    private UiItem pinkFlowerSeeds;
    private UiItem blueFlowerSeeds;
    private UiItem sunflowerSeeds;
    private UiItem grassSeeds;
    private UiItem wateringCan;
    private UiItem gardenCutters;

    public UiPanel(Texture2D panelFrame, Texture2D itemFrame, Texture2D pinkFlowerSeeds, Texture2D blueFlowerSeeds,
                    Texture2D sunflowerSeeds, Texture2D grassSeeds, Texture2D wateringCan, Texture2D gardenCutters)
    {
        this.panelFrame = panelFrame;
        
        this.pinkFlowerSeeds = new UiItem(pinkFlowerSeeds, itemFrame, 0);
        this.blueFlowerSeeds = new UiItem(blueFlowerSeeds, itemFrame, 1);
        this.sunflowerSeeds = new UiItem(sunflowerSeeds, itemFrame, 2);
        this.grassSeeds = new UiItem(grassSeeds, itemFrame, 3);
        this.wateringCan = new UiItem(wateringCan, itemFrame, 4);
        this.gardenCutters = new UiItem(gardenCutters, itemFrame, 5);
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(panelFrame, Vector2.Zero, Color.White);
        pinkFlowerSeeds.Draw(_spriteBatch);
        blueFlowerSeeds.Draw(_spriteBatch);
        sunflowerSeeds.Draw(_spriteBatch);
        grassSeeds.Draw(_spriteBatch);
        wateringCan.Draw(_spriteBatch);
        gardenCutters.Draw(_spriteBatch);
    }

    public EMouseMode Update(MouseState mouseState, EMouseMode currentMouseMode)
    {
        EMouseMode newMouseMode = currentMouseMode;
        pinkFlowerSeeds.Update(mouseState, ref newMouseMode);
        blueFlowerSeeds.Update(mouseState, ref newMouseMode);
        sunflowerSeeds.Update(mouseState, ref newMouseMode);
        grassSeeds.Update(mouseState, ref newMouseMode);
        wateringCan.Update(mouseState, ref newMouseMode);
        gardenCutters.Update(mouseState, ref newMouseMode);
        return newMouseMode;
    }
}