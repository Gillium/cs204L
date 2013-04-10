using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Menu_Test
{

    /*struct Button
    {
        Texture2D IdleImage;
        Texture2D ClickImage;
        double Height;
        double Width;
        float X;
        float Y;
    }*/


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D StartButton, QuitButton;

        double Center_of_screen_Width;

        SoundEffect MenuMusic;
        SoundEffect ButtonHover;
        SoundEffect ButtonClick;

        Vector2 StartGameButtonPosition;
        Vector2 QuitButtonPosition;

        bool PlayHover;
        bool PlayHover2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            StartButton = Content.Load<Texture2D> ("StartButton");
            QuitButton = Content.Load<Texture2D> ("QuitButton");
            Center_of_screen_Width = (graphics.PreferredBackBufferWidth - StartButton.Width) / 2;
            StartGameButtonPosition = new Vector2((float) Center_of_screen_Width ,
                (float)((graphics.PreferredBackBufferHeight - (2 * StartButton.Height) - 40) / 2.0));
            QuitButtonPosition = new Vector2((float) Center_of_screen_Width, 
                (float)(((graphics.PreferredBackBufferHeight - (2 * StartButton.Height) + 120 ) / 2.0) + 20));
            PlayHover = false;
            PlayHover = false;
            MenuMusic = Content.Load<SoundEffect>("05DeathToAllEnemies");
            ButtonHover = Content.Load<SoundEffect>("b381b6_TLOZ_Ocarina_Of_Time_Shield_Out_Sound_FX");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if ((gameTime.ElapsedGameTime.Duration().TotalMinutes + gameTime.ElapsedGameTime.Duration().TotalSeconds) % 
                97 == 0)
                MenuMusic.Play();

            // TODO: Add your update logic here
            float StartButtonLeft = StartGameButtonPosition.X;
            float StartButtonRight = StartButtonLeft + StartButton.Bounds.Width;
            float StartButtonTop = StartGameButtonPosition.Y;
            float StartButtonBottom = StartButtonTop + StartButton.Bounds.Height;

            float QuitButtonLeft = QuitButtonPosition.X;
            float QuitButtonRight = QuitButtonPosition.X + QuitButton.Bounds.Width;
            float QuitButtonTop = QuitButtonPosition.Y;
            float QuitButtonBottom = QuitButtonTop + QuitButton.Bounds.Height;

            //Logic for testing if the Mouse is within range of the Width of the buttons
            if (Mouse.GetState().X > StartButtonLeft && Mouse.GetState().X < StartButtonRight)
            {
                //Tests to see if the mouse cursor is within the bound of the Start Buttons y coordinates
                if (Mouse.GetState().Y < StartButtonBottom && Mouse.GetState().Y > StartButtonTop)
                {
                    StartButton = Content.Load<Texture2D>("StartButton 2");
                    PlayHover2 = true;
                }
                else
                {
                    StartButton = Content.Load<Texture2D>("StartButton");
                    PlayHover2 = false;
                }
                

            }
            else
            {
                StartButton = Content.Load<Texture2D>("StartButton");
                PlayHover2 = false;
            }

            if (Mouse.GetState().X > QuitButtonLeft && Mouse.GetState().X < QuitButtonRight)
            {
                //Tests to see if the mouse cursor is within the bound of the Start Buttons y coordinates
                if (Mouse.GetState().Y < QuitButtonBottom && Mouse.GetState().Y > QuitButtonTop)
                {
                    QuitButton = Content.Load<Texture2D>("QuitButton 2");
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    PlayHover = true;
                }
                else
                {
                    QuitButton = Content.Load<Texture2D>("QuitButton");
                    PlayHover = false;
                }
                

            }
            else
            {
                QuitButton = Content.Load<Texture2D>("QuitButton");
                PlayHover = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.Draw(StartButton, StartGameButtonPosition, Color.White);

            spriteBatch.Draw(QuitButton, QuitButtonPosition, Color.White);

            if (PlayHover == true)
            {
                ButtonHover.Play();
                PlayHover = false;
            }

            if (PlayHover2 == true)
            {
                ButtonHover.Play();
                PlayHover2 = false;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
