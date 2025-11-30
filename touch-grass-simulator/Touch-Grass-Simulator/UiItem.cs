namespace Touch_Grass_Simulator;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;

public class UiItem
{
    private Texture2D texture;
    private Texture2D frameTexture;
    private Vector2 outerPosition;
    private Vector2 innerPosition;
    private Color tintColour;
    private EMouseMode mouseMode;
    private const int SIZE = 40;
    public UiItem(Texture2D texture, Texture2D frameTexture, EMouseMode mouseMode, int panelIndex)
    {
        this.texture = texture;
        this.frameTexture = frameTexture;
        this.outerPosition = new Vector2(12, (12 * (panelIndex + 1)) + (SIZE * panelIndex));
        this.innerPosition = new Vector2(outerPosition.X + 4, outerPosition.Y + 4);
        this.tintColour = Color.White;
        this.mouseMode = mouseMode;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (this.mouseMode == EMouseMode.HAND)
        {
            _spriteBatch.Draw(frameTexture, outerPosition, Color.LightGreen);
        }
        else
        {
            _spriteBatch.Draw(frameTexture, outerPosition, this.tintColour);
        }
        _spriteBatch.Draw(texture, innerPosition, this.tintColour);
    }

    public void Update(MouseState mouseState, ref EMouseMode newMouseMode)
    {
        if (WithinSelf(new Vector2(mouseState.X, mouseState.Y)))
        {
            this.tintColour = Color.LightGray;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                newMouseMode = this.mouseMode;
                // Console.WriteLine("Changed mouse mode");
            }
        }
        else
        {
            this.tintColour = Color.White;
        }
    }

    public bool WithinSelf(Vector2 point)
    {
        int leftSide = Game1.GAME_WIN_WIDTH * Game1.RENDER_SCALE + (int)outerPosition.X * Game1.RENDER_SCALE;
        int rightSide = Game1.GAME_WIN_WIDTH * Game1.RENDER_SCALE + ((int)outerPosition.X + SIZE) * Game1.RENDER_SCALE;
        int topSide = (int)outerPosition.Y * Game1.RENDER_SCALE;
        int bottomSide = ((int)outerPosition.Y + SIZE) * Game1.RENDER_SCALE;

        bool result = point.X > leftSide && point.X < rightSide && point.Y > topSide && point.Y < bottomSide;
        // Console.WriteLine(leftSide + " " + rightSide + " " + topSide + " " + bottomSide);
        // Console.WriteLine(point);
        // Console.WriteLine(result);
        return result;
    }
}