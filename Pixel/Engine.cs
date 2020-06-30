using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Scenes;
using PixelShared;

namespace Pixel
{
    public class Engine : Game
    {
        private double _elapsedTime = 0;
        private readonly double _updateTime = 1f / Global.FixedUpdateHz;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine(bool vsync)
        {
            SetInitialGraphicsOptions(vsync);
            Content.RootDirectory = "../Content";
            Global.ContentManager = Content;

            IsMouseVisible = true;

            Window.ClientSizeChanged += (s, e) =>
            {
                Global.ScreenHeight = Window.ClientBounds.Height;
                Global.ScreenWidth = Window.ClientBounds.Width;
            };
        }

        private void SetInitialGraphicsOptions(bool vsync)
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Global.ScreenWidth,
                PreferredBackBufferHeight = Global.ScreenHeight,
                GraphicsProfile = GraphicsProfile.Reach,
                SynchronizeWithVerticalRetrace = vsync,
                PreferHalfPixelOffset = false,
                HardwareModeSwitch = false
            };
            _graphics.ApplyChanges();
            Global.Device = _graphics.GraphicsDevice;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Global.UpdateProfiler.StartMeasuring();
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (SceneManager.QueuedTasks.TryDequeue(out var action))
                action.Invoke();

            SceneManager.ActiveScene.Update(gameTime);

            while (_elapsedTime >= _updateTime)
            {
                _elapsedTime -= _updateTime;
                SceneManager.ActiveScene.FixedUpdate((float)gameTime.TotalGameTime.TotalSeconds);
            }

            base.Update(gameTime);
            Global.UpdateProfiler.StopMeasuring();
        }

        protected override void Draw(GameTime gameTime)
        {
            Global.DrawCalls = 0;
            Global.DrawProfiler.StartMeasuring();
            _graphics.GraphicsDevice.Clear(Color.Black);

            if (SceneManager.ActiveScene.IsReady)
                SceneManager.ActiveScene.Draw(_spriteBatch);

            base.Draw(gameTime);
            Global.DrawProfiler.StopMeasuring();
        }
    }
}
