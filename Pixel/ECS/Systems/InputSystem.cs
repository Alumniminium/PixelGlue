using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.Configuration;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using Pixel.World;
using PixelShared;
using System;

namespace Pixel.ECS.Systems
{
    public class InputSystem : IEntitySystem
    {
        public string Name { get; set; } = "Input System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            var scene = SceneManager.ActiveScene;
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            var gamepad = GamePad.GetState(PlayerIndex.One);

            foreach (var entity in CompIter<InputComponent,  DestinationComponent>.Get())
            {
                ref var ic = ref ComponentArray<InputComponent>.Get(entity);
                ref var dc = ref ComponentArray<DestinationComponent>.Get(entity);

                if (ic.Buttons == null)
                    ic.Buttons = new System.Collections.Generic.List<PixelGlueButtons>();
                if (ic.OldButtons == null)
                    ic.OldButtons = new System.Collections.Generic.List<PixelGlueButtons>();

                //Scrolling(ref ic, ref mouse, ref cf);
             
                var axis = Vector2.Zero;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    var point = scene.Camera.ScreenToWorld(mouse.Position.ToVector2());
                    var dir = point - scene.Player.Get<PositionComponent>().Value;
                    dir.Normalize();
                    axis = dir;
                }
                if (axis == Vector2.Zero)
                {
                    if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.Up))
                        axis.Y = -1;
                    else if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.Down))
                        axis.Y = 1;
                    if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.Left))
                        axis.X = -1;
                    else if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.Right))
                        axis.X = 1;
                }

                ic.Axis = axis;

                if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.EscapeMenu))
                    ic.Buttons.Add(PixelGlueButtons.EscapeMenu);
                if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.DbgProfiling))
                    ic.Buttons.Add(PixelGlueButtons.DbgProfiling);
                if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.DbgSwitchScene))
                    ic.Buttons.Add(PixelGlueButtons.DbgSwitchScene);
                if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.DbgProfiling))
                    ic.Buttons.Add(PixelGlueButtons.DbgProfiling);
                if (KeyboardHelper.IsDown(ref keyboard, PixelGlueButtons.DbgBoundingBoxes))
                    ic.Buttons.Add(PixelGlueButtons.DbgBoundingBoxes);

                if(KeyboardHelper.IsPressed(ref ic, PixelGlueButtons.DbgBoundingBoxes))
                {
                    var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                    system.IsActive = !system.IsActive;
                    var system2 = scene.GetSystem<NameTagRenderSystem>();
                    system2.IsActive = !system2.IsActive;
                }

                ic.OldButtons.Clear();
                ic.OldButtons.AddRange(ic.Buttons);
                ic.Buttons.Clear();
            }
        }

        private static void Scrolling(ref InputComponent inputComponent, ref MouseState m, ref CameraFollowTagComponent camera)
        {
            if (inputComponent.Scroll == m.ScrollWheelValue)
                return;

            if (m.ScrollWheelValue > inputComponent.Scroll)
                camera.Zoom *= 2;
            else if (m.ScrollWheelValue < inputComponent.Scroll)
                camera.Zoom /= 2;

            inputComponent.Scroll = m.ScrollWheelValue;

            WorldGen.TilesLoading = new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), bool>();
        }
        
    }
}