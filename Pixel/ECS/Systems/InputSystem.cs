using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using Pixel.World;
using System;
using System.Collections.Concurrent;
using PixelShared.Enums;
using PixelShared;
using Pixel.Entities;

namespace Pixel.ECS.Systems
{
    public class PlayerInputSystem : PixelSystem
    {
        public override string Name { get; set; } = "Input System";
        private PixelGlueButtons[] _mappedButtons;

        public override void Initialize()
        {
            _mappedButtons = (PixelGlueButtons[])Enum.GetValues(typeof(PixelGlueButtons));
            IsActive = true;
        }
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<InputComponent>() && entity.Has<DestinationComponent>()&& entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entity in Entities)
            {
                ref var inp = ref entity.Get<InputComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                EnsureReady(ref inp);

                var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
                var keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                var gamepad = GamePad.GetState(PlayerIndex.One);

                foreach (var mappedButton in _mappedButtons)
                    if (KeyboardHelper.IsDown(ref keyboard, mappedButton))
                        inp.Buttons.Add(mappedButton);

                Mouse(ref mouse, ref inp);
                Keyboard(ref inp);

                if(pos.Value == dst.Value)
                    dst.Value = pos.Value + (inp.Axis * Global.TileSize);

                inp.OldButtons.Clear();
                inp.OldButtons.AddRange(inp.Buttons);
                inp.Buttons.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void Mouse(ref MouseState mouse, ref InputComponent inp)
        {
            var scene = SceneManager.ActiveScene;
            if (inp.Scroll != mouse.ScrollWheelValue)
                WorldGen.TilesLoading = new ConcurrentDictionary<(int x, int y), bool>();

            inp.OldScroll = inp.Scroll;
            inp.Scroll = mouse.ScrollWheelValue;

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                var point = scene.Camera.ScreenToWorld(mouse.Position.ToVector2());
                var dir = point - scene.Player.Get<PositionComponent>().Value;
                dir.Normalize();
                inp.Axis = dir;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void EnsureReady(ref InputComponent inp)
        {
            if (inp.Buttons == null)
                inp.Buttons = new System.Collections.Generic.List<PixelGlueButtons>();
            if (inp.OldButtons == null)
                inp.OldButtons = new System.Collections.Generic.List<PixelGlueButtons>();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void Keyboard(ref InputComponent inp)
        {
            var scene = SceneManager.ActiveScene;
            var axis = Vector2.Zero;

            if (inp.IsDown(PixelGlueButtons.Up))
                axis.Y = -1;
            else if (inp.IsDown(PixelGlueButtons.Down))
                axis.Y = 1;
            if (inp.IsDown(PixelGlueButtons.Left))
                axis.X = -1;
            else if (inp.IsDown(PixelGlueButtons.Right))
                axis.X = 1;
            inp.Axis = axis;
            if (inp.IsPressed(PixelGlueButtons.DbgBoundingBoxes))
            {
                var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                system.IsActive = !system.IsActive;
                var system2 = scene.GetSystem<NameTagRenderSystem>();
                system2.IsActive = !system2.IsActive;
            }
            if (inp.IsPressed(PixelGlueButtons.EscapeMenu))
                Environment.Exit(0);
        }
    }
}