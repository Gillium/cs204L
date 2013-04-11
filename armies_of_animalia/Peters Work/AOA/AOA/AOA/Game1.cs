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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        public static bool collision = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level1;
        Map map1;

        //Player player;
        Camera camera;

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

        GameObject buildingBlock;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;//800 default size
            graphics.PreferredBackBufferHeight = 720;//500 default size
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);

            buildingBlock = new GameObject();
            buildingBlock.initializeMovement(new Vector3(0,0,0),
                                      new Vector3(0, 0, 0));

            //map1 = new Map(Content, Path.Combine(Content.RootDirectory, "level1.txt"),
            //    "Textures/AllBuildingTXT", new Point(50, 50), '*', 'P');
            //map1.AddRegion('X', new Rectangle(0, 0, 50, 50));
            //map1.AddRegion('T', new Rectangle(222, 0, 50, 50));
            //map1.AddRegion('>', new Rectangle(165, 0, 50, 50));
            //map1.AddRegion('<', new Rectangle(110, 0, 50, 50));
            //map1.AddRegion('E', new Rectangle(280, 0, 50, 50));
            //map1.AddRegion('B', new Rectangle(280, 0, 50, 50));
            //map1.AddRegion('R', new Rectangle(55, 0, 50, 50));
            //map1.AddRegion('A', new Rectangle(390, 0, 50, 50));
            //map1.AddRegion('C', new Rectangle(335, 0, 50, 50));

            map1 = new Map(Content, Path.Combine(Content.RootDirectory, "level1.txt"),
                buildingBlock, new Point(50, 50), '*', 'P');
            map1.AddRegion('X', buildingBlock);
            map1.AddRegion('T', buildingBlock);
            map1.AddRegion('>', buildingBlock);
            map1.AddRegion('<', buildingBlock);
            map1.AddRegion('E', buildingBlock); // enemy
            map1.AddRegion('B', buildingBlock); // boss
            map1.AddRegion('R', buildingBlock);
            map1.AddRegion('A', buildingBlock);
            map1.AddRegion('C', buildingBlock);

            map1.AddBackground("Textures/StarsBG");
            level1 = new Level(map1);

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

            // load the hunter's model
            buildingBlock.Filename = "Objects\\BuilldingBlock";
            buildingBlock.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform); //

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
            base.Draw(gameTime);
        }
    }
}