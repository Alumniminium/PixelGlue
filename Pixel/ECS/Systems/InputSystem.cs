using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using Pixel.World;
using System;
using System.Collections.Concurrent;

namespace Pixel.ECS.Systems
{
    public class InputSystem : IEntitySystem
    {
        public string Name { get; set; } = "Input System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        private PixelGlueButtons[] _mappedButtons;

        public void Initialize()
        {
            _mappedButtons = (PixelGlueButtons[])Enum.GetValues(typeof(PixelGlueButtons));
            IsReady = true;
            IsActive = true;
        }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            var scene = SceneManager.ActiveScene;
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(PlayerIndex.One);

            foreach (var entity in CompIter<InputComponent>.Get())
            {
                ref var inp = ref ComponentArray<InputComponent>.Get(entity);

                if (inp.Buttons == null)
                    inp.Buttons = new System.Collections.Generic.List<PixelGlueButtons>();
                if (inp.OldButtons == null)
                    inp.OldButtons = new System.Collections.Generic.List<PixelGlueButtons>();
                if (inp.Scroll != mouse.ScrollWheelValue)
                    WorldGen.TilesLoading = new ConcurrentDictionary<(int x, int y), bool>();

                inp.OldScroll = inp.Scroll;
                inp.Scroll = mouse.ScrollWheelValue;

                var axis = Vector2.Zero;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    var point = scene.Camera.ScreenToWorld(mouse.Position.ToVector2());
                    var dir = point - scene.Player.Get<PositionComponent>().Value;
                    dir.Normalize();
                    axis = dir;
                }

                foreach (var mappedButton in _mappedButtons)
                    if (KeyboardHelper.IsDown(ref keyboard, mappedButton))
                        inp.Buttons.Add(mappedButton);

                if (inp.Buttons.Contains(PixelGlueButtons.Up))
                    axis.Y = -1;
                else if (inp.Buttons.Contains(PixelGlueButtons.Down))
                    axis.Y = 1;
                if (inp.Buttons.Contains(PixelGlueButtons.Left))
                    axis.X = -1;
                else if (inp.Buttons.Contains(PixelGlueButtons.Right))
                    axis.X = 1;

                if(KeyboardHelper.IsPressed(ref inp,PixelGlueButtons.DbgBoundingBoxes))
                {
                    var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                    system.IsActive = !system.IsActive;
                    var system2 = scene.GetSystem<NameTagRenderSystem>();
                    system2.IsActive = !system2.IsActive;
                }

                inp.Axis = axis;
                inp.OldButtons.Clear();
                inp.OldButtons.AddRange(inp.Buttons);
                inp.Buttons.Clear();
            }
        }
    }
}