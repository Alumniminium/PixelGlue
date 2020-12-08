using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Scenes;
using Shared;

namespace Pixel
{
    public class Engine : Game
    {
        private double _elapsedTime;
        private readonly double _updateTime = 1f / Global.FixedUpdateHz;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine(bool vsync)
        {
            Thread.CurrentThread.Name = "Main/Render Thread";
            _graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.Reach,
                SynchronizeWithVerticalRetrace=vsync,
                PreferHalfPixelOffset = false,
                HardwareModeSwitch = false
            };
            _graphics.ApplyChanges();
            Content.RootDirectory = "../Content";
            Global.ContentManager = Content;

            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Global.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Global.ScreenHeight;
            _graphics.ApplyChanges();
            Global.Device = GraphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Window.ClientSizeChanged += (s, e) =>
            {
                Global.ScreenHeight = Window.ClientBounds.Height;
                Global.ScreenWidth = Window.ClientBounds.Width;
            };
        }
        protected override void Update(GameTime gameTime)
        {
            Global.UpdateTime = 0;
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (SceneManager.QueuedTasks.TryDequeue(out var action))
                action.Invoke();

            SceneManager.ActiveScene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Global.DrawTime = 0;
            Global.FrameCounter++;
            _graphics.GraphicsDevice.Clear(Color.Black);

            if (SceneManager.ActiveScene.IsReady)
                SceneManager.ActiveScene.Draw(_spriteBatch);

            Global.Metrics = GraphicsDevice.Metrics;
            base.Draw(gameTime);
        }
    }
}
