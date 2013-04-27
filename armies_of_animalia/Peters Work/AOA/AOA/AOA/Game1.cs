using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AOA {
    /// <summary>
    /// Armies of Animalia Game final submission
    /// Team Members: Joe G, Kyle G, Peter P
    /// 04/16/2013
    /// </summary>
 
    public class Game1 : Microsoft.Xna.Framework.Game {        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Player player;
        Camera camera;
        public static bool collision = false;
        //GameObject player;

        //Backgrounds
        Level level1;
        Map map1;

        //Scrolling background;
        Scrolling scrolling1;
        Scrolling scrolling2;
        Scrolling scrolling3;
        Scrolling scrolling4;
        Scrolling scrolling5;
        Scrolling scrolling6;
        Scrolling scrolling7;//extended level
        Scrolling scrolling8;//extended level
        Scrolling scrolling9;//extended level

        //Building blocks
        GameObject basicBlock;
        GameObject windowBlock;
        GameObject topBlock;
        GameObject rightBlock;
        GameObject leftBlock;
        GameObject rightCornerBlock;
        GameObject leftCornerBlock;
        GameObject belconyBlock;

        //Gamestate commands
        enum GameState { TitleScreen = 0, GameStarted, GameEnded };
        GameState currentGameState;
        KeyboardState currentInput;

        //Titlescreen
        Texture2D StartButton, QuitButton;
        double Center_of_screen_Width;
        Song MenuMusic;
        SoundEffect ButtonHover;
        Vector2 StartGameButtonPosition;
        Vector2 QuitButtonPosition;
        bool OnButton1;
        bool OnButton2;

        Texture2D TitleBackground;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

//            RasterizerState rState = new RasterizerState();
//            rState.CullMode = CullMode.None;
        }

        protected override void Initialize() {
            this.IsMouseVisible = true;
            Window.AllowUserResizing = true;
            camera = new Camera(GraphicsDevice.Viewport);

            basicBlock = new GameObject();
            basicBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            windowBlock = new GameObject();
            windowBlock.initializeMovement(new Vector3(0,0,0),
                new Vector3(0, 0, 0));
            topBlock = new GameObject();
            topBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            rightBlock = new GameObject();
            rightBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            leftBlock = new GameObject();
            leftBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            rightCornerBlock = new GameObject();
            rightCornerBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            leftCornerBlock = new GameObject();
            leftCornerBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));
            belconyBlock = new GameObject();
            belconyBlock.initializeMovement(new Vector3(0, 0, 0),
                new Vector3(0, 0, 0));

            //player = new GameObject();
            //player.initializeMovement(new Vector3(0, 0, 0),
            //    new Vector3(0, 0, 0));

            map1 = new Map(Content, Path.Combine(Content.RootDirectory, "level1.txt"),
                topBlock, new Point(50, 50), '*', 'P');
                //tileSheet, new Point(50, 50), '*', 'P');
            map1.AddRegion('X', windowBlock);
            map1.AddRegion('T', topBlock);
            map1.AddRegion('>', rightBlock);
            map1.AddRegion('<', leftBlock);
            map1.AddRegion('E', windowBlock); // enemy
            map1.AddRegion('B', windowBlock); // boss
            map1.AddRegion('R', windowBlock);
            map1.AddRegion('A', leftCornerBlock);
            map1.AddRegion('C', rightCornerBlock);
            map1.AddBackground("Textures/StarsBG");
            level1 = new Level(map1);
            currentGameState = GameState.TitleScreen;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            level1.Player.LoadTexture(Content, "Textures/SpriteSheet");
            //level1.Player.LoadTexture(Content, "Textures/char Sheetcorret Sizes");

            // Paralax background loads
            int height = 1000;//(int)map1.mapDimensions.Y;
            scrolling1 = new Scrolling(Content.Load<Texture2D>(@"Textures\ForeGround"), new Rectangle(0, height, 2048, 500));  //800,500 default size
            scrolling2 = new Scrolling(Content.Load<Texture2D>(@"Textures\ForeGround"), new Rectangle(2048, height, 2048, 500));
            scrolling7 = new Scrolling(Content.Load<Texture2D>(@"Textures\ForeGround"), new Rectangle(4096, height, 2048, 500));

            scrolling3 = new Scrolling(Content.Load<Texture2D>(@"Textures\MidGroundCity"), new Rectangle(0, height, 2048, 500));
            scrolling4 = new Scrolling(Content.Load<Texture2D>(@"Textures\MidGroundCity"), new Rectangle(2048, height, 2048, 500));
            scrolling8 = new Scrolling(Content.Load<Texture2D>(@"Textures\MidGroundCity"), new Rectangle(4096, height, 2048, 500));

            scrolling5 = new Scrolling(Content.Load<Texture2D>(@"Textures\BackCity"), new Rectangle(0, height, 2048, 500));
            scrolling6 = new Scrolling(Content.Load<Texture2D>(@"Textures\BackCity"), new Rectangle(2048, height, 2048, 500));
            scrolling9 = new Scrolling(Content.Load<Texture2D>(@"Textures\BackCity"), new Rectangle(4096, height, 2048, 500));

            // load block model's
            basicBlock.Filename = "Objects\\basicBlock";
            basicBlock.Load(Content);
            windowBlock.Filename = "Objects\\WindowBlock";
            windowBlock.Load(Content);
            topBlock.Filename = "Objects\\topBlock";
            topBlock.Load(Content);
            leftBlock.Filename = "Objects\\leftBlock";
            leftBlock.Load(Content);
            rightBlock.Filename = "Objects\\rightBlock";
            rightBlock.Load(Content);
            leftCornerBlock.Filename = "Objects\\leftCornerBlock";
            leftCornerBlock.Load(Content);
            rightCornerBlock.Filename = "Objects\\rightCornerBlock";
            rightCornerBlock.Load(Content);
            //belconyBlock.Filename = "Objects\\belconyBlock";
            //belconyBlock.Load(Content);

            // Load player model
            // player.Filename = "Objects\\FoxPlayer";
            // player.Load(Content);

            // Titlescreen loads
            StartButton = Content.Load<Texture2D>(@"Textures\StartButton");
            QuitButton = Content.Load<Texture2D>(@"Textures\QuitButton");
            Center_of_screen_Width = (graphics.PreferredBackBufferWidth - StartButton.Width) / 2;
            StartGameButtonPosition = new Vector2((float)Center_of_screen_Width,
                (float)((graphics.PreferredBackBufferHeight - (2 * StartButton.Height) - 40) / 2.0));
            QuitButtonPosition = new Vector2((float)Center_of_screen_Width,
                (float)(((graphics.PreferredBackBufferHeight - (2 * StartButton.Height) + 120) / 2.0) + 20));
            OnButton1 = false;
            OnButton2 = false;
            MenuMusic = Content.Load<Song>(@"Sounds\05DeathToAllEnemies");
            MediaPlayer.IsRepeating = true;
            ButtonHover = Content.Load<SoundEffect>(@"Sounds\b381b6_TLOZ_Ocarina_Of_Time_Shield_Out_Sound_FX");

            TitleBackground = Content.Load<Texture2D>(@"Textures\TitleBackground");
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            currentInput = Keyboard.GetState();

            // TODO: Add your update logic here
            if (currentGameState == GameState.TitleScreen) {
                if ((gameTime.ElapsedGameTime.Duration().TotalMinutes + gameTime.ElapsedGameTime.Duration().TotalSeconds) % 
                    97 == 0)
                MediaPlayer.Play(MenuMusic);

                float StartButtonLeft = StartGameButtonPosition.X;
                float StartButtonRight = StartButtonLeft + StartButton.Bounds.Width;
                float StartButtonTop = StartGameButtonPosition.Y;
                float StartButtonBottom = StartButtonTop + StartButton.Bounds.Height;

                float QuitButtonLeft = QuitButtonPosition.X;
                float QuitButtonRight = QuitButtonPosition.X + QuitButton.Bounds.Width;
                float QuitButtonTop = QuitButtonPosition.Y;
                float QuitButtonBottom = QuitButtonTop + QuitButton.Bounds.Height;

                //Logic for testing if the Mouse is within range of the Width of the buttons
                if (Mouse.GetState().X > StartButtonLeft && Mouse.GetState().X < StartButtonRight) {
                    //Tests to see if the mouse cursor is within the bound of the Start Buttons y coordinates
                    if (Mouse.GetState().Y < StartButtonBottom && Mouse.GetState().Y > StartButtonTop) {
                        if (!OnButton1) {
                            StartButton = Content.Load<Texture2D>(@"Textures\StartButton 2");
                            ButtonHover.Play();
                            OnButton1 = true;
                        }

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                            currentGameState = GameState.GameStarted;
                            MediaPlayer.Stop();
                        }
                    } else {
                        StartButton = Content.Load<Texture2D>(@"Textures\StartButton");
                        OnButton1 = false;
                    }
                } else {
                    StartButton = Content.Load<Texture2D>(@"Textures\StartButton");
                    OnButton1 = false;
                }

                if (Mouse.GetState().X > QuitButtonLeft && Mouse.GetState().X < QuitButtonRight) {
                    //Tests to see if the mouse cursor is within the bound of the Start Buttons y coordinates
                    if (Mouse.GetState().Y < QuitButtonBottom && Mouse.GetState().Y > QuitButtonTop) {
                        if (!OnButton2) {
                            QuitButton = Content.Load<Texture2D>(@"Textures\QuitButton 2");
                            OnButton2 = true;
                            ButtonHover.Play();
                        }

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                            this.Exit();
                        }
                    } else {
                        QuitButton = Content.Load<Texture2D>(@"Textures\QuitButton");
                        OnButton2 = false;
                    }
                } else {
                    QuitButton = Content.Load<Texture2D>(@"Textures\QuitButton");
                    OnButton2 = false;
                }
            } else if (currentGameState == GameState.GameStarted) {
                level1.Update(gameTime);
                camera.Update(gameTime, level1.Player);

                //scrolling background
                if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                    scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
                if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                    scrolling2.rectangle.X = scrolling7.rectangle.X + scrolling7.texture.Width;
                if (scrolling7.rectangle.X + scrolling7.texture.Width <= 0)
                    scrolling7.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

                if (scrolling3.rectangle.X + scrolling3.texture.Width <= 0)
                    scrolling3.rectangle.X = scrolling4.rectangle.X + scrolling4.texture.Width;
                if (scrolling4.rectangle.X + scrolling4.texture.Width <= 0)
                    scrolling4.rectangle.X = scrolling8.rectangle.X + scrolling8.texture.Width;
                if (scrolling8.rectangle.X + scrolling8.texture.Width <= 0)
                    scrolling8.rectangle.X = scrolling3.rectangle.X + scrolling3.texture.Width;

                if (scrolling5.rectangle.X + scrolling5.texture.Width <= 0)
                    scrolling5.rectangle.X = scrolling6.rectangle.X + scrolling6.texture.Width;
                if (scrolling6.rectangle.X + scrolling6.texture.Width <= 0)
                    scrolling6.rectangle.X = scrolling9.rectangle.X + scrolling9.texture.Width;
                if (scrolling9.rectangle.X + scrolling9.texture.Width <= 0)
                    scrolling9.rectangle.X = scrolling5.rectangle.X + scrolling5.texture.Width;

                scrolling1.Update();
                scrolling2.Update();
                scrolling7.Update();

                scrolling3.Update2();
                scrolling4.Update2();
                scrolling8.Update2();

                scrolling5.Update3();
                scrolling6.Update3();
                scrolling9.Update3();
            } else if (currentGameState == GameState.GameEnded) {
                // end commands eg Credits, gameover screen
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            if (currentGameState == GameState.TitleScreen) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(StartButton, StartGameButtonPosition, Color.White);
                spriteBatch.Draw(QuitButton, QuitButtonPosition, Color.White);
                spriteBatch.Draw(TitleBackground, new Rectangle(0, 0, TitleBackground.Width, TitleBackground.Height), Color.White);
                spriteBatch.End();
            }

            if (currentGameState == GameState.GameStarted) {

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                //map1.DrawBackground(spriteBatch);

                //draw paralaxingBackGround here
                //scrolling5.Draw(spriteBatch);
                //scrolling6.Draw(spriteBatch);
                //scrolling9.Draw(spriteBatch);

                //scrolling3.Draw(spriteBatch);
                //scrolling4.Draw(spriteBatch);
                //scrolling8.Draw(spriteBatch);

                //scrolling1.Draw(spriteBatch);
                //scrolling2.Draw(spriteBatch);
                //scrolling7.Draw(spriteBatch);

                //draw background here
                level1.DrawBackgound(gameTime, spriteBatch, camera);

                // draw player here
                level1.Draw(gameTime, spriteBatch);

                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}