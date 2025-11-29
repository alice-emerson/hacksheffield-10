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
                    case ETiles.SHORT_BLUE_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.short_blue_flower);
                            break;
                        }
                    case ETiles.MEDIUM_BLUE_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.medium_blue_flower);
                            break;
                        }
                    case ETiles.TALL_BLUE_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.tall_blue_flower);
                            break;
                        }
                    case ETiles.SHORT_PINK_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.short_pink_flower);
                            break;
                        }
                    case ETiles.MEDIUM_PINK_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.medium_pink_flower);
                            break;
                        }
                    case ETiles.TALL_PINK_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.tall_pink_flower);
                            break;
                        }
                    case ETiles.SHORT_SUN_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.short_sun_flower);
                            break;
                        }
                    case ETiles.MEDIUM_SUN_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.medium_sun_flower);
                            break;
                        }
                    case ETiles.TALL_SUN_FLOWER:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.tall_sun_flower);
                            break;
                        }
                    case ETiles.SHORT_GRASS:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.short_grass);
                            break;
                        }
                    case ETiles.MEDIUM_GRASS:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.medium_grass);
                            break;
                        }
                    case ETiles.TALL_GRASS:
                        {
                            Tile.Draw(_spriteBatch, new Vector2(row,col), textures.tall_grass);
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
            string[] lineTiles = reader.ReadLine().Split(' ');
            for (int col = 0; col < MAP_WIDTH; col++)
            {
                switch (lineTiles[col])
                {
                    case "DI": 
                        {
                            tiles[row,col] = ETiles.DIRT;
                            break;
                        }
                    case "SG": 
                        {
                            tiles[row,col] = ETiles.SHORT_GRASS;
                            break;
                        }
                    case "MG": 
                        {
                            tiles[row,col] = ETiles.MEDIUM_GRASS;
                            break;
                        }
                    case "TG": 
                        {
                            tiles[row,col] = ETiles.TALL_GRASS;
                            break;
                        }
                    case "SB": 
                        {
                            tiles[row,col] = ETiles.SHORT_BLUE_FLOWER;
                            break;
                        }
                    case "MB": 
                        {
                            tiles[row,col] = ETiles.MEDIUM_BLUE_FLOWER;
                            break;
                        }
                    case "TB": 
                        {
                            tiles[row,col] = ETiles.TALL_BLUE_FLOWER;
                            break;
                        }
                    case "SS": 
                        {
                            tiles[row,col] = ETiles.SHORT_SUN_FLOWER;
                            break;
                        }
                    case "MS": 
                        {
                            tiles[row,col] = ETiles.MEDIUM_SUN_FLOWER;
                            break;
                        }
                    case "TS": 
                        {
                            tiles[row,col] = ETiles.TALL_SUN_FLOWER;
                            break;
                        }
                    case "SP": 
                        {
                            tiles[row,col] = ETiles.SHORT_PINK_FLOWER;
                            break;
                        }
                    case "MP": 
                        {
                            tiles[row,col] = ETiles.MEDIUM_PINK_FLOWER;
                            break;
                        }
                    case "TP": 
                        {
                            tiles[row,col] = ETiles.TALL_PINK_FLOWER;
                            break;
                        }
                    case "XX":
                        {
                            tiles[row,col] = ETiles.NONE; 
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