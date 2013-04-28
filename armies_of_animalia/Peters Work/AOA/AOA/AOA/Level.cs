using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AOA
{
    class Level
    {
        public Player Player
        {
            get;
            private set;
        }

        Map activateMap;

        public Level(Map map)
        {
            Player = new Player(map.Spawn.X, map.Spawn.Y, map.TileDimensions.X, map.TileDimensions.Y);
            activateMap = map;
        }

        public void Update(GameTime gameTime)
        {
            bool collision = false;
           
            //foreach (Point p in Player.CollisionPoints)
            //{
            //    if (activateMap.CheckCollision(p))
            //    {
            //        Player.HandleTileCollision(p);
            //        collision = true;
            //    }
            //}
            if (activateMap.CheckCollision(Player))
            {
                //Player.HandleTileCollision();
                collision = true;
            }
            Player.Update(gameTime, collision);
            Game1.collision = collision;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, GraphicsDevice g)
        {
            Player.Draw(spriteBatch, camera, g);
        }

        public void DrawMap(GameTime gameTime, Camera camera, GraphicsDevice g)
        {
            if (activateMap != null)
                activateMap.Draw(camera, g, Player);
        }
    }
}
