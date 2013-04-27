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
            Player.Update(gameTime);
            foreach (Point p in Player.CollisionPoints)
            {
                if (activateMap.CheckCollision(p))
                {
                    Player.HandleTileCollision(p);
                    collision = true;
                }
            }
            Game1.collision = collision;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            Player.Draw(spriteBatch, camera);
        }

        public void DrawBackgound(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            if (activateMap != null)
                activateMap.Draw(spriteBatch, camera);
        }
    }
}
