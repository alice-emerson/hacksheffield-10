using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Touch_Grass_Simulator;

public class TileMap
{
    private ETiles[,] tiles;
    public TileMap(ETiles[,] tiles)
    {
        this.tiles = tiles;
    }

    public void draw(SpriteBatch _spriteBatch) {}
}