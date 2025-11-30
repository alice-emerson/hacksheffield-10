using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Touch_Grass_Simulator.Types;

using System.IO;
using System.Collections;
using System;
using InfluxDB3.Client.Write;

namespace Touch_Grass_Simulator;

public class TileMap
{
    private ETiles[,] tiles;
    private const int MAP_WIDTH = 20;
    private const int MAP_HEIGHT = 14;
    private int elapsedTimeSinceToolUse = 0; // in miliseconds
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

    public SessionStats Update(MouseState currentMouseState, EMouseMode currentTool, int elapsedTimeSinceToolUse, InfluxDB db, SessionStats currentStats)
    {
        if (currentMouseState.X < Game1.GAME_WIN_WIDTH * Game1.RENDER_SCALE 
            && currentMouseState.LeftButton == ButtonState.Pressed
            && this.elapsedTimeSinceToolUse > 500) // Not in menu, button pressed, cooldown expired.
        {   
            this.elapsedTimeSinceToolUse = 0;

            // Watering Plants

            if (currentTool == EMouseMode.WATERING_CAN)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                bool wateredPlant = true;
                
                switch (tiles[currentTile.Item1, currentTile.Item2])
                {
                    case ETiles.SHORT_BLUE_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.MEDIUM_BLUE_FLOWER;
                            break;
                        }
                    case ETiles.MEDIUM_BLUE_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.TALL_BLUE_FLOWER;
                            break;
                        }

                    case ETiles.SHORT_PINK_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.MEDIUM_PINK_FLOWER;
                            break;
                        }
                    case ETiles.MEDIUM_PINK_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.TALL_PINK_FLOWER;
                            break;
                        }

                    case ETiles.SHORT_SUN_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.MEDIUM_SUN_FLOWER;
                            break;
                        }
                    case ETiles.MEDIUM_SUN_FLOWER:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.TALL_SUN_FLOWER;
                            break;
                        }

                    case ETiles.SHORT_GRASS:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.MEDIUM_GRASS;
                            break;
                        }
                    case ETiles.MEDIUM_GRASS:
                        {
                            tiles[currentTile.Item1, currentTile.Item2] = ETiles.TALL_GRASS;
                            break;
                        }

                    default:
                            {
                                wateredPlant = false;
                                break;
                            }
                }

                if (wateredPlant)
                {
                    db.dbClient.WritePointAsync(PointData.Measurement("Watered Plant").SetField("Count", 1));
                }
            }

            // Adding new plants

            if (currentTool == EMouseMode.PINK_FLOWER_SEEDS)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                tiles[currentTile.Item1, currentTile.Item2] = ETiles.SHORT_PINK_FLOWER;
                currentStats.total_pink_flowers += 1;
                currentStats.total_plants += 1;
                PostSessionStats(db, currentStats);
            }

            if (currentTool == EMouseMode.BLUE_FLOWER_SEEDS)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                tiles[currentTile.Item1, currentTile.Item2] = ETiles.SHORT_BLUE_FLOWER;
                currentStats.total_blue_flowers += 1;
                currentStats.total_plants += 1;
                PostSessionStats(db, currentStats);
            }

            if (currentTool == EMouseMode.SUNFLOWER_SEEDS)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                tiles[currentTile.Item1, currentTile.Item2] = ETiles.SHORT_SUN_FLOWER;
                currentStats.total_sun_flowers += 1;
                currentStats.total_plants += 1;
                PostSessionStats(db, currentStats);
            }

            if (currentTool == EMouseMode.GRASS_SEEDS)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                tiles[currentTile.Item1, currentTile.Item2] = ETiles.SHORT_GRASS;
                currentStats.total_grass += 1;
                currentStats.total_plants += 1;
                PostSessionStats(db, currentStats);
            }

            // Touching Grass
            
            if (currentTool == EMouseMode.HAND)
            {
                (int,int) currentTile = GetSelectedTile(currentMouseState);
                if (tiles[currentTile.Item1, currentTile.Item2] == ETiles.SHORT_GRASS ||
                    tiles[currentTile.Item1, currentTile.Item2] == ETiles.MEDIUM_GRASS ||
                    tiles[currentTile.Item1, currentTile.Item2] == ETiles.TALL_GRASS)
                {
                    db.dbClient.WritePointAsync(PointData.Measurement("Touched Grass").SetField("Count", 1));
                    Console.WriteLine("Touched Grass");
                }
                else if (tiles[currentTile.Item1, currentTile.Item2] == ETiles.NONE) // Must be a flower since not grass or nothing
                {
                    db.dbClient.WritePointAsync(PointData.Measurement("Touched Flower").SetField("Count", 1));
                    Console.WriteLine("Touched Flower");
                }
            }
        }

        // Finish up

        this.elapsedTimeSinceToolUse += elapsedTimeSinceToolUse;
        if (this.elapsedTimeSinceToolUse > 10000) this.elapsedTimeSinceToolUse = 10000;

        return currentStats;
    }

    public (int, int) GetSelectedTile(MouseState currentMouseState)
    {
        int column = (int)((currentMouseState.X / Game1.RENDER_SCALE) / Tile.TILE_SIZE);
        int row = (int)((currentMouseState.Y / Game1.RENDER_SCALE) / Tile.TILE_SIZE);
        return(row, column);
    } 

    public void PostSessionStats(InfluxDB db, SessionStats stats)
    {
        db.dbClient.WritePointAsync(PointData.Measurement("SessionStats")
                                        .SetField("Total Blue Flowers", stats.total_blue_flowers)
                                        .SetField("Total Pink Flowers", stats.total_pink_flowers)
                                        .SetField("Total Sunflowers", stats.total_sun_flowers)
                                        .SetField("Total Grass", stats.total_grass)
                                        .SetField("Overall Total Plants", stats.total_plants));
    }
}