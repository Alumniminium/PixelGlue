using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Helpers;

namespace PixelGlueCore
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        private double _elapsedTime = 0;
        private double _updateTime = 1f / PixelGlue.FixedUpdateHz;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine(bool vsync)
        {
            SetInitialGraphicsOptions(vsync);
            Content.RootDirectory = "../Content";

            IsMouseVisible = true;

            Window.ClientSizeChanged += (s, e) =>
            {
                PixelGlue.ScreenHeight = Window.ClientBounds.Height;
                PixelGlue.ScreenWidth = Window.ClientBounds.Width;
            };
        }

        private void SetInitialGraphicsOptions(bool vsync)
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = PixelGlue.ScreenWidth;
            _graphics.PreferredBackBufferHeight = PixelGlue.ScreenHeight;
            _graphics.GraphicsProfile = GraphicsProfile.Reach;
            _graphics.SynchronizeWithVerticalRetrace = vsync;
            _graphics.PreferHalfPixelOffset = false;
            _graphics.ApplyChanges();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            PixelGlue.UpdateProfiler.StartMeasuring();
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (SceneManager.QueuedTasks.TryDequeue(out var action))
                action.Invoke();

            foreach (var scene in SceneManager.ActiveScenes)
            {
                if (!scene.IsReady || !scene.IsActive)
                    continue;
                scene.Update(gameTime);
            }
            while (_elapsedTime >= _updateTime)
            {
                _elapsedTime -= _updateTime;
                foreach (var scene in SceneManager.ActiveScenes)
                {
                    if (!scene.IsReady || !scene.IsActive)
                        continue;
                    scene.FixedUpdate(gameTime.TotalGameTime.TotalSeconds);
                }
            }

            while (ActionQueue.Items.Count > 0)
                ActionQueue.Items.Dequeue().Invoke();

            base.Update(gameTime);
            PixelGlue.UpdateProfiler.StopMeasuring();
        }

        protected override void Draw(GameTime gameTime)
        {
            PixelGlue.DrawProfiler.StartMeasuring();
            _graphics.GraphicsDevice.Clear(Color.Black);

            foreach (var scene in SceneManager.ActiveScenes)
                if (scene.IsReady)
                    scene.Draw(_spriteBatch);

            base.Draw(gameTime);
            PixelGlue.DrawProfiler.StopMeasuring();
        }
    }
}
