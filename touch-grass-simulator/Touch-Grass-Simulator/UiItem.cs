namespace Touch_Grass_Simulator;

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
    public UiItem(Texture2D texture, Texture2D frameTexture, int panelIndex)
    {
        this.texture = texture;
        this.frameTexture = frameTexture;
        this.outerPosition = new Vector2(12, (12 * (panelIndex + 1)) + (40 * panelIndex));
        this.innerPosition = new Vector2(outerPosition.X + 4, outerPosition.Y + 4);
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(frameTexture, outerPosition, Color.White);
        _spriteBatch.Draw(texture, innerPosition, Color.White);
    }

    public void Update(MouseState mouseState, ref EMouseMode newMouseMode)
    {
        
    }
}