using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;

using System.IO;

namespace Touch_Grass_Simulator;

public class TileMap
{
    private ETiles[,] tiles;
    private const int MAP_WIDTH = 20;
    private const int MAP_HEIGHT = 14;
    public TileMap(ETiles[,] tiles)
    {
        this.tiles = tiles;
    }

    public TileMap(string filePath)
    {
        this.tiles = ArrayFromText(filePath);
    }

    public void Draw(SpriteBatch _spriteBatch, TextureGroup textures)
    {
        for (int row = 0; row < MAP_HEIGHT; row++)
        {
            for (int col = 0; col < MAP_WIDTH; col++)
            {
                switch (tiles[row,col])
                {
                    case ETiles.DIRT:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.dirt);
                            break;
                        }
                    default: break;
                }
            }
        }
    }

    private ETiles[,] ArrayFromText(string fileName)
    {
        FileStream fs = File.Open(fileName, FileMode.Open);
        StreamReader reader = new(fs);
        ETiles[,] tiles = new ETiles[MAP_HEIGHT,MAP_WIDTH];
        for (int row = 0; row < MAP_HEIGHT; row++)
        {
            string line = reader.ReadLine();
            for (int col = 0; col < MAP_WIDTH; col++)
            {
                switch (line[col])
                {
                    case 'D': 
                        {
                            tiles[row,col] = ETiles.DIRT;
                            break;
                        }
                    default:
                        {
                           tiles[row,col] = ETiles.NONE; 
                           break;
                        } 
                }
            }
        }
        return tiles;
    }
}