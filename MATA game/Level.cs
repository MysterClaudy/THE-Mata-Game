using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MATA_game
{
    class Level : IDisposable
    {
        #region Properties
        public Tile[,] tiles;
        Texture2D[] layers;
        const int entityLayer = 2;
        public PlayerClass player;
        GameStates gameStates;

        List<EnemyClass> enemies = new List<EnemyClass>();

        static readonly Point InvalidPosition = new Point(-1, -1);
        Point exit = InvalidPosition;
        Random random = new Random();
        bool reachedExit;
        public GraphicsDeviceManager graphics;

        ContentManager content;

        #endregion

        #region Collectors
        public PlayerClass Player
        {
            get { return player; }
        }

        public ContentManager Content
        {
            get { return content; }
        } 

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

        public Level(IServiceProvider serviceProvider, Stream fileStream, int levelIndex)
        {
            content = new ContentManager(serviceProvider, "Content");

            LoadTiles(fileStream);

        }

        void LoadTiles(Stream fileStream)
        {
            int width;
            List<string> lines = new List<string>();

            using  (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(string.Format("the length of line {0} is different the otheres.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            tiles = new Tile[width, lines.Count];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }
        }

        Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '.':
                    return new Tile(null, TileCollision.Passable);
                case 'X':
                    return LoadExitTile(x, y);
                case 'A':
                    return LoadEnemyTile(x, y, "EnemyA");
                /*case 'S':
                    return LoadSpawnTile(x, y);*/
                case 'W':
                    return LoadVarietyTile("WallA", 1, TileCollision.Impassable);
                case '1':
                    return LoadVarietyTile("WallB", 1, TileCollision.Impassable);

                default:
                    throw new NotSupportedException(string.Format("{0} at position {1}, {2} is not a supported tile type", tileType, x, y));

            }
        }

        Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + name), collision);
        }

        Tile LoadVarietyTile(string baseName, int variationCount, TileCollision collision)
        {
            int index = random.Next(variationCount);
            return LoadTile(baseName + index, collision);
        }

        Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("only 1 exit Allowed");

            exit = GetBounds(x, y).Center;

            return LoadTile("Exit", TileCollision.Passable);
        }

        Tile LoadEnemyTile(int x, int y, string spriteSet)
        {
            Vector2 position = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            enemies.Add(new EnemyClass());

            return new Tile(null, TileCollision.Passable);
        }

        public void Dispose()
        {
            Content.Unload();
        }

        public TileCollision GetCollision(int x, int y)
        {
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            if (y < 0 || y >= Height)
                return TileCollision.Passable;
            return tiles[x, y].collision;
        }

        void UpdateEnemies(GameTime gameTime)
        {
            foreach (EnemyClass enemy in enemies)
            {
                enemy.Update(gameTime);
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);

            foreach (EnemyClass enemy in enemies)
                enemy.Draw(gameTime, spriteBatch);
        }

        void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Texture2D texture = tiles[x, y].texture;
                    if (texture != null)
                    {
                        Vector2 position = new Vector2(x, y) * Tile.size;
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }
        #endregion
    }
}
