using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AOA
{
    class Camera
    {
        public Matrix transform;//drawing the camera on the screen
        Viewport view;//setting where the camera is looking
        Vector2 center;//will be the character

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, Player player)
        {
            center = new Vector2(player.PlayerPostion().X + (player.PlayerRectangle().Width / 2) - 300,
            player.PlayerPostion().Y + (player.PlayerRectangle().Height / 2) - 400);//400 half of screen, 0 is y in description of video
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * //how far zoomed in, 1 is null size
            Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
