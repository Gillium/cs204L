using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AOA
{
    public class Camera
    {
        public Vector3 position, viewAt, worldUp;
        public Matrix projectionMatrix, viewMatrix;
        public Matrix transform;//drawing the camera on the screen
        public int windowWidth, windowHeight;
        Viewport view;//setting where the camera is looking
        Vector2 center;//will be the character
        public float nearDistance, farDistance;
        private float fieldOfView;

        public Camera(Viewport newView)
        {

            windowWidth = newView.Width;
            windowHeight = newView.Height;
            fieldOfView = MathHelper.Pi / 4.0f;
            nearDistance = 2.0f;
            farDistance = 10000.0f;
            view = newView;
            position = new Vector3(25, 25, 1000); //view.X, view.Y, view.MinDepth;
            viewAt = new Vector3(25, 25, 0); //view.X, view.Y, 20.0f;
            worldUp = new Vector3(0.0f, 1.0f, 0.0f);
            UpdateView();
            UpdateProjection();
        }

        public void Update(GameTime gameTime, Player player)
        {
            center = new Vector2(player.PlayerPostion().X + (player.PlayerRectangle().Width / 2) - 300,
            player.PlayerPostion().Y + (player.PlayerRectangle().Height / 2) - 400);//400 half of screen, 0 is y in description of video
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * //how far zoomed in, 1 is null size
            Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
            position.X = player.PlayerPostion().X;
            position.Y = player.PlayerPostion().Y;
            viewAt.X = position.X;
            viewAt.Y = position.Y;
            UpdateView();
        }

        // The camera's view matrix
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }

        // The camera's projection matrix
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }

        private void UpdateView()
        {
            viewMatrix = Matrix.CreateLookAt(position, viewAt, worldUp);
        }

        private void UpdateProjection()
        {
            float windowAspect = windowWidth / (float)windowHeight;
            projectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(fieldOfView,
                                                    windowAspect,
                                                    nearDistance,
                                                    farDistance);
        }

    }
}
