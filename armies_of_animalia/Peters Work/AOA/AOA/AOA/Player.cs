using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace AOA
{
    public class Player
    {
        Texture2D texture;
        Vector2 position, previousPosition;
        Vector2 movement;
        Vector2 velocity;
        Vector2 acceleration;

        Rectangle playerRect;

        int tileWidth, tileHeight;
        float depthX, depthY;

        const float gravity = 200f;
        const float speed = 600f;
        const float maxSpeed = 800f;

        KeyboardState currentInput; //, previousInput

        private List<Vector2> BoundPoints;

        Point frameSize = new Point(35, 70); // one frame is 50 pixels////
        Point currentFrame = new Point(0, 0);////
        Point sheetSize = new Point(5, 1); // the sheet has 5 frames, by 1 frames//

        GameObject obj;

        public List<Point> CollisionPoints
        {
            get;
            private set;
        }

        public BoundingBox CollisionBox
        {
            get;
            private set;
        }

        public bool Collision
        {
            get;
            private set;
        }

        public Player(int tileX, int tileY, int tileWidth, int tileHeight)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            position = previousPosition = new Vector2(tileX * tileWidth, tileY * tileHeight);
            velocity = acceleration = Vector2.Zero;

            currentInput = Keyboard.GetState();
            BoundPoints = new List<Vector2>(4);
            CollisionPoints = new List<Point>();
        }

        public Rectangle PlayerRectangle()
        {
            playerRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            return playerRect;
        }

        public Vector2 PlayerPostion()
        {
            return position;
        }

        public void LoadTexture(ContentManager content, string assetname)
        {
            texture = content.Load<Texture2D>(assetname);
        }

        public void LoadObj(GameObject go)
        {
            obj = go;
        }

        public Point GetTileLocation()
        {
            return GetTileLocation(position);
        }

        private Point GetTileLocation(Vector2 position)
        {
            return new Point((int)(position.X / (int)(50 * .68)), (int)(position.Y / (int)(50 * .68)));
        }

        public void Update(GameTime gameTime, bool collision)
        {
            Collision = collision;
            previousPosition = position;
            //previousInput = currentInput;
            currentInput = Keyboard.GetState();

            acceleration.Y = -gravity;

            velocity -= movement;
            movement = Vector2.Zero;

            if (currentInput.IsKeyDown(Keys.Left))
                movement.X -= speed;
            if (currentInput.IsKeyDown(Keys.Right))
            {
                movement.X += speed;
                ++currentFrame.X;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
            }
            if (currentInput.IsKeyDown(Keys.Up))
            {
                movement.Y += speed * 1.5f;
            }
            if (currentInput.IsKeyDown(Keys.Down))
                movement.Y -= speed;

            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity = movement + acceleration;

            position += velocity * deltatime;
            if (position.X <= 0)
                position.X = 0;
            if (position.Y <= 0)
                position.Y = 0;

            CollisionBox = new BoundingBox(new Vector3(position.X - (int)(10 * .68), position.Y, -(int)(60 * .68)),
                new Vector3(position.X + (int)(30 * .68), position.Y + (int)(140 * .68), (int)(60 * .68)));
            
           // PopulateBoundPoints();

           // Point loc = GetTileLocation(BoundPoints[0]);
           // CollisionPoints.Clear();
           // CollisionPoints.Add(loc);

            //for (int i = 1; i < 4; i++)
            //{
            //   loc = GetTileLocation(BoundPoints[i]);
            //    if (!CollisionPoints.Contains(loc))
            //    {
            //        CollisionPoints.Add(loc);
            //    }
            //}
        }

        private void PopulateBoundPoints()
        {
            //BoundPoints.Clear();
            //BoundPoints.Add(position);
            //BoundPoints.Add(Vector2.Add(position, new Vector2((int)(50*.68) - 1, 0)));
            //BoundPoints.Add(Vector2.Add(position, new Vector2(0, (int)(50 * .68) - 1)));
            //BoundPoints.Add(Vector2.Add(position, new Vector2((int)(50 * .68) - 1, (int)(50 * .68) - 1)));
        }

        public void HandleTileCollision()
        {
            position = previousPosition;
        }

        public void HandleTileCollision(Point collisionLocation)
        {
            float tileX = collisionLocation.X * tileWidth;
            float tileY = collisionLocation.Y * tileHeight;
            depthX = depthY = float.MaxValue;

            if (velocity.X < 0)
            {
                depthX = (tileX + tileWidth) - position.X;
            }
            else if (velocity.X > 0)
            {
                depthX = tileX - (position.X + texture.Width);
            }
            if (velocity.Y < 0)
            {
                depthY = (tileY + tileHeight) - position.Y;
            }
            else if (velocity.Y > 0)
            {
                depthY = tileY - (position.Y + texture.Height);
            }

            if (Math.Abs(depthY) <= Math.Abs(depthX))
            {
                position += new Vector2(0, depthY);
            }
            else if (Math.Abs(depthY) > Math.Abs(depthX))
            {
                position += new Vector2(depthX, 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, GraphicsDevice g)
        {
            obj.Position = new Vector3(position.X,position.Y, 0);
            obj.Draw(camera.ViewMatrix, camera.ProjectionMatrix, g);

            BoundingBoxRenderer.Render(
            CollisionBox,
            g,
            camera.ViewMatrix,
            camera.ProjectionMatrix,
            Collision ? Color.Red : Color.Green);
        }
    }
}
