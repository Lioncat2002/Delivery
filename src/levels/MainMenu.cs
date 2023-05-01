using Apos.Gui;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Delivery.src.levels
{
    public class MainMenu : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public MainMenu(Game game) : base(game) { }
        public override void LoadContent()
        {

            Game.gameColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup(gameTime);
            Game._ui.UpdateStart(gameTime);

                
                MenuPanel.Push();
                if (Button.Put("Start", 30, Color.AliceBlue).Clicked)
                {
                    Game.currentLevel = Utils.Level.Level1;
                    Game.hasLevelChanged = false;
                    Game.gameColor = Color.White;
                }
            if (Button.Put("Quit", 30, Color.AliceBlue).Clicked)
            {
                Game.Exit();
            }
            MenuPanel.Pop();
           
            Game._ui.UpdateEnd(gameTime);
            GuiHelper.UpdateCleanup();
        }
        
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Game.gameColor);
            Game._ui.Draw(gameTime);
        }
    }
}
