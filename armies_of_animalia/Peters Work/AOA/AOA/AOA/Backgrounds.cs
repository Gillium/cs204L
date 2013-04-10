using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;//

namespace AOA
{
    class Backgrounds
    {
        public Texture2D texture;
        public Rectangle rectangle;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    class Scrolling : Backgrounds
    {
        KeyboardState currentInput;

        public Scrolling(Texture2D newTexture, Rectangle newRectangle)
        {
            texture = newTexture;
            rectangle = newRectangle;
        }

        public void Update()
        {
            currentInput = Keyboard.GetState();

            if (currentInput.IsKeyDown(Keys.Left))
                rectangle.X += 3;
            if (currentInput.IsKeyDown(Keys.Right))
            {
                rectangle.X -= 3;
            }
        }

        public void Update2()
        {
            currentInput = Keyboard.GetState();

            if (currentInput.IsKeyDown(Keys.Left))
                rectangle.X += 2;
            if (currentInput.IsKeyDown(Keys.Right))
            {
                rectangle.X -= 2;
            }
        }

        public void Update3()
        {
            currentInput = Keyboard.GetState();

            if (currentInput.IsKeyDown(Keys.Left))
                rectangle.X += 1;
            if (currentInput.IsKeyDown(Keys.Right))
            {
                rectangle.X -= 1;
            }
        }
    }
}
