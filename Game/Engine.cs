using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS;
using PixelGlueCore.Helpers;
using PixelGlueCore.Scenes;

namespace PixelGlueCore
{
    public class Engine : Game
    {
        private double _elapsedTime = 0;
        private readonly double _updateTime = 1f / PixelGlue.FixedUpdateHz;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine(bool vsync)
        {
            SetInitialGraphicsOptions(vsync);
            Content.RootDirectory = "../Content";
            PixelGlue.ContentManager = Content;

            IsMouseVisible = true;

            Window.ClientSizeChanged += (s, e) =>
            {
                PixelGlue.ScreenHeight = Window.ClientBounds.Height;
                PixelGlue.ScreenWidth = Window.ClientBounds.Width;
            };
        }

        private void SetInitialGraphicsOptions(bool vsync)
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = PixelGlue.ScreenWidth,
                PreferredBackBufferHeight = PixelGlue.ScreenHeight,
                GraphicsProfile = GraphicsProfile.Reach,
                SynchronizeWithVerticalRetrace = vsync,
                PreferHalfPixelOffset = false,
                HardwareModeSwitch = false
            };
            _graphics.ApplyChanges();
            PixelGlue.Device=_graphics.GraphicsDevice;
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

            foreach (var scene in SceneManager.ActiveGameScenes)
            {
                if (!scene.IsReady || !scene.IsActive)
                    continue;
                scene.Update(gameTime);
            }
            while (_elapsedTime >= _updateTime)
            {
                _elapsedTime -= _updateTime;
                foreach (var scene in SceneManager.ActiveGameScenes)
                {
                    if (!scene.IsReady || !scene.IsActive)
                        continue;
                    scene.FixedUpdate((float)gameTime.TotalGameTime.TotalSeconds);
                }
            }

            while (ActionQueue.Items.Count > 0)
                ActionQueue.Items.Dequeue().Invoke();

            base.Update(gameTime);
            PixelGlue.UpdateProfiler.StopMeasuring();
        }

        protected override void Draw(GameTime gameTime)
        {
            PixelGlue.DrawCalls=0;
            PixelGlue.DrawProfiler.StartMeasuring();
           // _graphics.GraphicsDevice.Clear(Color.Black);
            
            foreach (var scene in SceneManager.ActiveGameScenes)
                if (scene.IsReady)
                    scene.Draw(_spriteBatch);
            foreach (var scene in SceneManager.ActiveUIScenes)
                if (scene.IsReady)
                    scene.Draw(_spriteBatch);

            base.Draw(gameTime);
            PixelGlue.DrawProfiler.StopMeasuring();
        }
    }
}
