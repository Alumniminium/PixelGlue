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
            var keyboard= Keyboard.GetState();
            var gamepad = GamePad.GetState(PlayerIndex.One);

            foreach (var entity in CompIter<InputComponent, PositionComponent, CameraFollowTagComponent>.Get())
            {
                ref var ic = ref ComponentArray<InputComponent>.Get(entity);
                ref var pc = ref ComponentArray<PositionComponent>.Get(entity);
                ref var cf = ref ComponentArray<CameraFollowTagComponent>.Get(entity);

                Scrolling(ref ic,ref mouse, ref cf);
                Rotating(ref keyboard, ref pc);

                var destination = pc.Position;

                if (mouse.LeftButton == ButtonState.Pressed)
                    pc.Destination = scene.Camera.ScreenToWorld(mouse.Position.ToVector2());
                //if (KeyDown(ref inputComponent, PixelGlueButtons.Sprint))
                //    moveComponent.Velocity *= 10;
                //if (KeyUp(ref inputComponent, PixelGlueButtons.Sprint))
                //    moveComponent.Velocity /= 10;
                if (KeyDown(ref keyboard, PixelGlueButtons.Up) && pc.Position == pc.Destination)
                    destination.Y -= Global.TileSize;
                if (KeyDown(ref keyboard, PixelGlueButtons.Down) && pc.Position == pc.Destination)
                    destination.Y += Global.TileSize;
                if (KeyDown(ref keyboard, PixelGlueButtons.Left) && pc.Position == pc.Destination)
                    destination.X -= Global.TileSize;
                if (KeyDown(ref keyboard, PixelGlueButtons.Right) && pc.Position == pc.Destination)
                    destination.X += Global.TileSize;

                if (Pressed(ref ic,ref keyboard, PixelGlueButtons.EscapeMenu))
                    Environment.Exit(0);
                if (Pressed(ref ic,ref keyboard, PixelGlueButtons.DbgProfiling))
                    Global.Profiling = !Global.Profiling;
                if (Pressed(ref ic,ref keyboard, PixelGlueButtons.DbgSwitchScene))
                    SwitchScene();
                if (Pressed(ref ic,ref keyboard, PixelGlueButtons.DbgProfiling))
                    OpenDialog(scene);
                if (Pressed(ref ic,ref keyboard, PixelGlueButtons.DbgBoundingBoxes))
                {
                    var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                    system.IsActive = !system.IsActive;
                    var system2 = scene.GetSystem<NameTagRenderSystem>();
                    system2.IsActive = !system2.IsActive;
                }

                if (destination != pc.Position)
                    pc.Destination = destination;
            }
        }

        private static void Rotating(ref KeyboardState kb, ref PositionComponent positionComponent)
        {
            if (kb.IsKeyDown(Keys.Left))
                positionComponent.Rotation -= 0.1f;
            if (kb.IsKeyDown(Keys.Right))
                positionComponent.Rotation += 0.1f;
        }

        private static void Scrolling(ref InputComponent inputComponent, ref MouseState m, ref CameraFollowTagComponent camera)
        {
            if(inputComponent.Scroll == m.ScrollWheelValue)
                return;
            
            inputComponent.Scroll = m.ScrollWheelValue;

            if (m.ScrollWheelValue > inputComponent.Scroll)
                camera.Zoom *= 2;
            else if (m.ScrollWheelValue < inputComponent.Scroll)
                camera.Zoom /= 2;
            
            WorldGen.TilesLoading=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), bool>();
        }
        public bool KeyDown(ref KeyboardState kb, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return kb.IsKeyDown(realKey.defaultBind) || kb.IsKeyDown(realKey.userBind);
            return false;
        }
        public bool KeyUp(ref KeyboardState kb, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return kb.IsKeyUp(realKey.defaultBind) || kb.IsKeyUp(realKey.userBind);
            return false;
        }
        public bool Pressed(ref InputComponent component,ref KeyboardState kb, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
            {
                for (int i = 0; i < component.OldButtons?.Length; i++)
                {
                    if (component.OldButtons[i] == key)
                        return false;
                }
                return kb.IsKeyDown(realKey.userBind) || kb.IsKeyDown(realKey.defaultBind);
            }
            return false;
        }

        private static void SwitchScene()
        {
            
        }

        private static void OpenDialog(Scene scene)
        {
            var player = scene.Find<Player>();
            player.Add(new DialogComponent(player.EntityId, 1, 0));
            //scene.AddComponent(new DialogComponent(player.UniqueId, 1));
        }
    }
}