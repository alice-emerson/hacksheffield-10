namespace Touch_Grass_Simulator;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class CustomMouse
{
    private Texture2D clickedTexture;
    private Texture2D standardTexture;
    private bool isMouseClicked;
    private Vector2 currentMousePos;

    public CustomMouse(Texture2D standardTexture, Texture2D clickedTexture)
    {
        this.clickedTexture = clickedTexture;
        this.standardTexture = standardTexture;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (isMouseClicked)
        {
            _spriteBatch.Draw(clickedTexture, currentMousePos, Color.White);
        }
        else
        {
            _spriteBatch.Draw(standardTexture, currentMousePos, Color.White);
        }
    }

    public void Update(Vector2 currentMousePos, bool isMouseClicked)
    {
        this.isMouseClicked = isMouseClicked;
        this.currentMousePos = currentMousePos;
        // TODO: Add logic for watering plants
    }
}