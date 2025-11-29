namespace Touch_Grass_Simulator;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;

public static class Tile
{
    public const int TILE_SIZE = 32;
    public static void Draw(SpriteBatch _spriteBatch, Vector2 arrayPos, Texture2D texture)
    {
        _spriteBatch.Draw(texture, new Vector2(arrayPos.Y * TILE_SIZE, arrayPos.X * TILE_SIZE), Color.White);
    }
}