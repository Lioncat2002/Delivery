using Apos.Gui;
using Delivery.src.levels;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Delivery.src
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public IMGUI _ui;

        public MainMenu mainMenu;
        public Level1 level1;
        public Level2 level2;

        public Texture2D player;

        private readonly ScreenManager _screenManager;

       
        public Utils.Level currentLevel;
        public bool hasLevelChanged = false;
        public Color gameColor=Color.White;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            level1 = new Level1(this);
            level2 = new Level2(this);
            mainMenu = new MainMenu(this);
            _screenManager = new ScreenManager();
            
            Components.Add(_screenManager);
        }
        private void LoadMainMenu()
        {
            _screenManager.LoadScreen(mainMenu, new FadeTransition(GraphicsDevice, Color.White));
        }
        private void LoadScreen1()
        {
            _screenManager.LoadScreen(level1, new FadeTransition(GraphicsDevice,Color.White));
        }

        private void LoadScreen2()
        {
            _screenManager.LoadScreen(level2, new FadeTransition(GraphicsDevice, Color.White));
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 850;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();
            base.Initialize();
            currentLevel=Utils.Level.MainMenu;
            hasLevelChanged=true;
            
            LoadMainMenu();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            #region UI
            FontSystem fontSystem = new FontSystem();
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dogicapixel.ttf")); GuiHelper.Setup(this, fontSystem);
            _ui = new IMGUI();
            #endregion
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            if(!hasLevelChanged)
            switch (currentLevel)
            {
                    case Utils.Level.MainMenu:
                        LoadMainMenu();
                        break;
                case Utils.Level.Level1:
                        gameColor = Color.White;
                        LoadScreen1();
                        hasLevelChanged = true;
                    
                    break;
                case Utils.Level.Level2:
                        gameColor = Color.White;
                        LoadScreen2();
                        hasLevelChanged = true;
                        
                    break;
            }
            //if (keyboardState.IsKeyDown(Keys.A))
            //{
            //    LoadScreen1();
            //}
            //else if (keyboardState.IsKeyDown(Keys.S))
            //{
            //    LoadScreen2();
            //}
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            

            base.Draw(gameTime);
        }
    }
}