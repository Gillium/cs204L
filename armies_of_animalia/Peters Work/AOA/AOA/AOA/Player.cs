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

        float gravity = 200f;
        const float speed = 600f;
        const float maxSpeed = 800f;

        KeyboardState currentInput; //, previousInput

        private List<Vector2> BoundPoints;

        Point frameSize = new Point(35, 70); // one frame is 50 pixels////
        Point currentFrame = new Point(0, 0);////
        Point sheetSize = new Point(5, 1); // the sheet has 5 frames, by 1 frames//

        GameObject obj;
        BoundingBox mergedBox;

        public float Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

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

        public BoundingBox MergedBox
        {
            get { return mergedBox; }
            set { mergedBox = value; }
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

        public void Update(GameTime gameTime, bool collision, Map map)
        {
            int i = (int)((PlayerPostion().X + 50) / 100);
            int j = (int)((PlayerPostion().Y + 0) / 100);
            Collision = collision;
            previousPosition = position;
            //previousInput = currentInput;
            currentInput = Keyboard.GetState();

            if (!Collision)
              acceleration.Y = -gravity;

            velocity -= movement;
            movement = Vector2.Zero;

            if (currentInput.IsKeyDown(Keys.Left))
            {
                movement.X -= speed;
                if ((j > 0) && map.isEmptyTile(i, j - 1))
                    gravity = 200f;
            }
            if (currentInput.IsKeyDown(Keys.Right))
            {
                movement.X += speed;
                if ((j > 0) && map.isEmptyTile(i, j - 1))
                    gravity = 200f;
            }
            if (currentInput.IsKeyDown(Keys.Up))
            {
                movement.Y += speed * 1.5f;
                gravity = 200f;
            }
            //if (currentInput.IsKeyDown(Keys.Down))
                //movement.Y -= speed;

            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity = movement + acceleration;

            position += velocity * deltatime;
            if (position.X <= 0)
                position.X = 0;
            if (position.Y <= 0)
                position.Y = 0;
            if (position.X > (114 * 100))
                position.X = (114 * 100);
            if (position.Y > (30 * 100))
                position.Y = (30 * 100);

            CollisionBox = new BoundingBox(new Vector3(position.X - (int)(10 * .68), position.Y, -(int)(60 * .68)),
                new Vector3(position.X + (int)(30 * .68), position.Y + (int)(140 * .68), (int)(60 * .68)));       
        }

        public void HandleTileCollision(Map map, bool collision, GameTime gameTime)
        {
                depthX = depthY = float.MaxValue;
                int i = (int)((PlayerPostion().X + 50) / 100);
                int j = (int)((PlayerPostion().Y + 0) / 100);
                Vector3 pos = new Vector3(map.TileDimensions.X * i, map.TileDimensions.Y * j, 0);
                BoundingBox bb = new BoundingBox(new Vector3(pos.X - (int)(80 * .68), pos.Y, -(int)(80 * .68)), new Vector3(pos.X + (int)(80 * .68), pos.Y + (int)(140 * .68), (int)(60 * .68)));
                float deltaX, deltaY;               

                if (velocity.Y > 0)
                {
                    deltaY = -CollisionBox.GetCorners()[0].Y + bb.GetCorners()[3].Y;
                }
                else if (velocity.Y < 0)
                {
                    deltaY = CollisionBox.GetCorners()[0].Y - bb.GetCorners()[0].Y;
                    deltaY -= 50;
                    gravity = 0f;
                }
                else
                    deltaY = 0;

                if (velocity.X > 0)
                {
                    deltaX = CollisionBox.GetCorners()[1].X - bb.GetCorners()[0].X;
                    deltaY = 0;
                }
                else if (velocity.X < 0)
                {
                    deltaX = CollisionBox.GetCorners()[0].X - bb.GetCorners()[1].X;
                    deltaY = 0;
                }
                else
                    deltaX = 0;

                position.X -= deltaX;
                position.Y += deltaY;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, GraphicsDevice g)
        {
            obj.Position = new Vector3(position.X,position.Y, 0);
            obj.Draw(camera.ViewMatrix, camera.ProjectionMatrix, g);

            //BoundingBoxRenderer.Render(
            //CollisionBox,
            //g,
            //camera.ViewMatrix,
            //camera.ProjectionMatrix,
            //Collision ? Color.Red : Color.Green);
        }
    }
}
