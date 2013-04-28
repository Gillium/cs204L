﻿using System;
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

        public Point Spawn {
            get;
            private set;
        }



        char[,] tiles; //
        public char[,] Tiles {
            get { return tiles; }
            protected set { tiles = value; }
        }

        public Map(ContentManager content, string file,  
            Point dimensions, char emptyTile) {
            this.content = content;
            this.emptyTile = emptyTile;

            filename = file;
            tileRegions = new Dictionary<char, GameObject>();
            TileDimensions = dimensions;

            ReadFile();
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
                        //Spawn = new Point(j, i); //i,j
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
                !tiles[movingTileLocation.X, mapHeight - movingTileLocation.Y].Equals('*'))
                return true; // do bounding box intersection here

            return false;
        }

        public bool CheckCollision(Player p)
        {
            Vector3[] corners = p.CollisionBox.GetCorners();
            if (((corners[1].X + 50) / 100) > mapDimensions.X - 1)
                return true;
            if (((corners[0].X + 50) / 100) < 1)
                return true;
            if (((int)((p.PlayerPostion().X + 50) / 100) < mapDimensions.X) && ((int)((p.PlayerPostion().Y + 50) / 100) < mapDimensions.Y) && !tiles[(int)((p.PlayerPostion().X + 50) / 100), (int)((p.PlayerPostion().Y + 50) / 100)].Equals('*'))
            {
                GameObject go = tileRegions[tiles[(int)((p.PlayerPostion().X + 50) / 100), (int)((p.PlayerPostion().Y + 50) / 100)]];
                go.Collision = true;
                return true;
            }
            return false;
        }

        public void Draw(Camera camera, GraphicsDevice g, Player p) {
            if (tileRegions.Count == 0)
                throw new Exception("Tile region must be populated by calling AddRegion");
            else {
                Rectangle bgRect = new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                    (int)(mapDimensions.Y * TileDimensions.Y));
 
                DrawTiles(camera, g, p);
            }
        }

        public void DrawBackground(SpriteBatch spritebatch) {
            spritebatch.Draw(background, new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                     (int)(mapDimensions.Y * TileDimensions.Y)), Color.White);
        }

        private void DrawTiles(Camera camera, GraphicsDevice g, Player p)
        {
            Rectangle bgRect = new Rectangle(0, 0, (int)(mapDimensions.X * TileDimensions.X),
                (int)(mapDimensions.Y * TileDimensions.Y));

            for (int j = 0; j < mapDimensions.Y; j++) {
                for (int i = 0; i < mapDimensions.X; i++) {
                    if (tiles[i, j] != '*') {
                        //look up block type in dictionary and draw the block
                        GameObject tile = tileRegions[tiles[i, j]];
                        tile.Position = new Vector3(TileDimensions.X * i,
                                                         TileDimensions.Y * j, 0);
                        ////tron mode
                        //tile.Draw(camera.ViewMatrix, camera.ProjectionMatrix, g);
                        BoundingBox bb = new BoundingBox(new Vector3(tile.Position.X - (int)(60 * .68), tile.Position.Y, -(int)(60 * .68)), new Vector3(tile.Position.X + (int)(80 * .68), tile.Position.Y + (int)(140 * .68), (int)(60 * .68)));
                        BoundingBoxRenderer.Render(bb, g, camera.ViewMatrix, camera.ProjectionMatrix,  p.CollisionBox.Intersects(bb) ? Color.Red : Color.Green);
                    }
                }
            }
        }
    }
}
