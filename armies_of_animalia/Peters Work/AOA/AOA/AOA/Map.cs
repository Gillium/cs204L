using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AOA
{
    public class Map
    {
        Dictionary<char, GameObject> tileRegions;

        public Texture2D background;
        ContentManager content;
        string filename;
        char emptyTile, SpawnTile;

        public Vector2 mapDimensions = Vector2.Zero;

        public Point TileDimensions {
            get;
            private set;
        }

        char[,] tiles; //
        public char[,] Tiles {
            get { return tiles; }
            protected set { tiles = value; }
        }

        //char emptyTile;

        public GameObject TileSheet {
            get;
            protected set;
        }

        public Point Spawn {
            get;
            private set;
        }

        public Map(ContentManager content, string file,  
            GameObject go, Point dimensions, char emptyTile, char spawnTile) {
            this.content = content;
            this.emptyTile = emptyTile;
            this.SpawnTile = spawnTile;

            filename = file;
            tileRegions = new Dictionary<char, GameObject>();
            TileDimensions = dimensions;

            LoadGameObject(go);

            ReadFile();
        }

        private void LoadGameObject(GameObject go) {
            //go.Load(content);
            TileSheet = go;
        }

        private void ReadFile() {
            StreamReader reader = new StreamReader(TitleContainer.OpenStream(filename));
            int width = 0, height = 0;
            List<string> linesFromFile = new List<string>();

            string line = reader.ReadLine();
            height = line.Length;//width

            while (line != null) {
                width++;//height
                linesFromFile.Add(line);
                line = reader.ReadLine();
            }

            tiles = new char[width, height];

            for (int j = 0; j < width; j++) {
                string l = linesFromFile[j];
                for (int i = 0; i < height; i++) {
                    char c = l[i];
                    if (c.Equals(SpawnTile)) {
                        Spawn = new Point(j, i); //i,j
                        c = emptyTile;
                    }
                    tiles[j, i] = c;//ij
                }
            }

            mapDimensions = new Vector2(width, height);
        }

        public void AddRegion(char tileKey, GameObject region) {
            //can only have one region per key
            if (!tileRegions.ContainsKey(tileKey))
                tileRegions.Add(tileKey, region);
            else
                throw new Exception("Can only have one region per key");
        }

        public void AddBackground(string bgAsset) {
            background = content.Load<Texture2D>(bgAsset);
        }

        public bool CheckCollision(Point movingTileLocation) {
            if (movingTileLocation.X > mapDimensions.X - 1)
                return true;
            int mapHeight = (int)mapDimensions.Y - 1;
            if (movingTileLocation.X < mapDimensions.X && movingTileLocation.Y < mapDimensions.Y &&
                !tiles[movingTileLocation.X, mapHeight - movingTileLocation.Y].Equals(emptyTile))
                return true;

            return false;
        }

        public void Draw(SpriteBatch spritebatch, Camera camera) {
            if (TileSheet == null)
                throw new Exception("Tile sheet cannot be null");
            else if (tileRegions.Count == 0)
                throw new Exception("Tile region must be populated by calling AddRegion");
            else {
                Rectangle bgRect = new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                    (int)(mapDimensions.Y * TileDimensions.Y));
 
                //if (background != null)
                  //  spritebatch.Draw(background, bgRect, Color.White);

                DrawTiles(spritebatch, camera);
            }
        }

        public void DrawBackground(SpriteBatch spritebatch) {
            spritebatch.Draw(background, new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                     (int)(mapDimensions.Y * TileDimensions.Y)), Color.White);
        }

        private void DrawTiles(SpriteBatch spritebatch, Camera camera)
        {
            Rectangle bgRect = new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                (int)(mapDimensions.Y * TileDimensions.Y));

            for (int j = 0; j < mapDimensions.Y; j++) {
                for (int i = 0; i < mapDimensions.X; i++) {
                    if (tiles[i, j] != emptyTile) {
                        //spritebatch.Draw(TileSheet, new Vector2(TileDimensions.X * i,
                        //    bgRect.Height - TileDimensions.Y * (j + 1)), tileRegions[tiles[i, j]],
                        //    Color.White);

                        //look up block type in dictionary and draw the block
                        GameObject tile = tileRegions[tiles[i, j]];

                        tile.Position = new Vector3(TileDimensions.X * i,
                                                         TileDimensions.Y * j, 0);
//                            bgRect.Height - TileDimensions.Y * (j + 1), 10.0f);
                        tile.Draw(camera.ViewMatrix, camera.ProjectionMatrix);
                    }
                }
            }
        }
    }
}
