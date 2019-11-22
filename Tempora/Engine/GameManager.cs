using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tempora.Engine
{
    /// <summary>
    /// Manages the game and all it's states
    /// </summary>
    public class GameManager : Game
    {
        //Interal references to stuff
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //External references

        /// <summary>
        /// Used to load stuff from the content managed
        /// </summary>
        public static ContentManager ContentManager;

        /// <summary>
        /// The GraphicsDevice instance associated with this game
        /// </summary>
        public static GraphicsDevice GraphicsDeviceInstance;

        /// <summary>
        /// The current GameState
        /// </summary>
        public static GameState State;

        //Utility

        /// <summary>
        /// A 1x1 white rectangle texture, used for drawing primitive shapes with no textures
        /// </summary>
        public static Texture2D WHITE_SOLID;

        /// <summary>
        /// Initializes our game
        /// </summary>
        /// <param name="state">The state to intialize the game with</param>
        public GameManager(GameState state)
        {
            //Set our state
            State = state;

            //Store refence to graphics device
            graphics = new GraphicsDeviceManager(this);

            //Configure window
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            //Set up solid texture
            WHITE_SOLID = new Texture2D(graphics.GraphicsDevice, 1, 1);
            WHITE_SOLID.SetData(new Color[] { Color.White });

            //Set out root direct and other stuff
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize our game
        /// </summary>
        protected override void Initialize()
        {
            //Initialize monogame
            base.Initialize();

            //Set up physics
            Physics.Initialize();

            //Set up game state
            State.Initialize();
        }

        /// <summary>
        /// Load content for our game
        /// </summary>
        protected override void LoadContent()
        {
            //Create our sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Store external reference to content manager
            ContentManager = this.Content;

            //Load the state
            State.Load();
        }

        /// <summary>
        /// Tick out game
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            //Easy quit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Tick the game state
            State.Tick(gameTime);

            
            //Tick monogame
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //Store external reference to graphics device (this seems wrong but will fix later)
            if(GraphicsDeviceInstance == null)
                GraphicsDeviceInstance = GraphicsDevice;

            //Clear the screen to black
            GraphicsDevice.Clear(Color.Black);

            //Draw the game state
            State.Draw(spriteBatch, gameTime);

            //Draw the UI layer
            State.DrawUI(spriteBatch, gameTime, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Draw monogame
            base.Draw(gameTime);
        }
    }
}