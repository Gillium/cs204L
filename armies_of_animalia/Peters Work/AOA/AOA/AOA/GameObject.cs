#region File Description
//-----------------------------------------------------------------------------
// Mode.cs
// Adapted fom Tank.cs, part of the SimpleAnimation XNA sample project
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AOA
{
    /// <summary>
    /// Helper class for drawing a tank model with animated wheels and turret.
    /// </summary>
    public class GameObject
    {
        #region Fields

        Model theShape;

        // Overall scale factor to fit our object into our world
        float objectScale = 0.68f;

        // location and velocity of our object
        Vector3 position;
        Vector3 velocity;

        // filename of the object
        String filename;

        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        Matrix[] boneTransforms;

        //float wingAngle = 0;
        //float wingAngleIncrement = 3;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets the velocity
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Gets or sets the filename
        /// </summary>
        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        /// Gets or sets the model's overall transformation
        /// </summary>
        public Matrix RootTransform
        {
            get { return theShape.Root.Transform; }
            set { theShape.Root.Transform = value; }
        }

        #endregion

        /// <summary>
        /// Loads the model.
        /// </summary>
        public void Load(ContentManager content)
        {
            // Load the tank model from the ContentManager.
            theShape = content.Load<Model>(filename);
            boneTransforms = new Matrix[theShape.Bones.Count];
          //  wingAngle = 0;
        }

        public void initializeMovement(Vector3 pos, Vector3 vel)
        {
            position = pos;
            velocity = vel;
        }

        public void HandleWallCollisions(BoundingBox box)
        {
            if (position.X > box.Max.X ||
                position.X < box.Min.X)
                velocity.X *= -1;

            if (position.Y > box.Max.Y ||
                position.Y < box.Min.Y)
                velocity.Y *= -1;

            if (position.Z > box.Max.Z ||
                position.Z < box.Min.Z)
                velocity.Z *= -1;
        }

        //public bool CollidesWith(Model otherModel, Matrix otherWorld)
        //{
        //    // Loop through each ModelMesh in both objects and compare all bounding
        //    // spheres for collisions
        //    foreach (ModelMesh myModelMeshes in otherModel.Meshes)
        //    {
        //        foreach (ModelMesh hisModelMeshes in otherModel.Meshes)
        //        {
        //            if (myModelMeshes.BoundingSphere.Transform(GetWorld()).Intersects(hisModelMeshes.BoundingSphere.Transform(otherWorld)))
        //                return true;
        //        }
        //    }
        //    return false;
        //}

        public void Update(GameTime time)
        {
            float secondsElapsed = time.ElapsedGameTime.Seconds +
                                   time.ElapsedGameTime.Milliseconds / 1000.0f;
            position += velocity * secondsElapsed;

            //wingAngle += (float)MathHelper.ToRadians(wingAngleIncrement);
            //if (Math.Abs(wingAngle) > Math.PI / 4.0)
            //    wingAngleIncrement *= -1;
        }


        /// <summary>
        /// Draws the tank model, using the current animation settings.
        /// </summary>
        public void Draw(Matrix view, Matrix projection)
        {
            // Set the world matrix as the root transform of the model.
            Matrix scale = Matrix.CreateScale(objectScale,
                          objectScale,
                          objectScale);
            float headingAngle = (float)Math.Atan2(velocity.Y, velocity.X);
            Matrix pitchRotation = Matrix.CreateRotationX(0);
            Matrix yawRotation = Matrix.CreateRotationZ((float)(headingAngle + Math.PI / 2.0));
            Matrix rollRotation = Matrix.CreateRotationY((float)(headingAngle + Math.PI / 2.0));//
//            Matrix rollRotation = Matrix.CreateRotationY((float)Math.PI);//
            Matrix translation = Matrix.CreateTranslation(position + 1.5f * Vector3.UnitZ);

            RootTransform = scale *             //1
                            rollRotation *      //2
                            //yawRotation *       //3
                            //pitchRotation *     //4
                            translation;        //5

            // Look up combined bone matrices for the entire model.
            theShape.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // Draw the model.
            foreach (ModelMesh mesh in theShape.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //if (mesh.Name == "leftWing")
                    //    effect.World = Matrix.CreateTranslation(2, 0, 0) *
                    //                   Matrix.CreateRotationY(wingAngle) *
                    //                   Matrix.CreateTranslation(-2, 0, 0) *
                    //                   boneTransforms[mesh.ParentBone.Index];

                    //else if (mesh.Name == "rightWing")
                    //    effect.World = Matrix.CreateTranslation(-2, 0, 0) *
                    //                   Matrix.CreateRotationY(-wingAngle) *
                    //                   Matrix.CreateTranslation(2, 0, 0) *
                    //                   boneTransforms[mesh.ParentBone.Index];
                    //else
                        effect.World = boneTransforms[mesh.ParentBone.Index];

                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
    }
}
