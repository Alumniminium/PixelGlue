using Pixel.ECS.Components;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel.Scenes;
using Microsoft.Xna.Framework;
using Pixel.Configuration;
using Pixel.Enums;
using Pixel.Entities;
using Pixel.ECS.Systems;
using Pixel.Helpers;

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
                    foreach (var entity in CompIter.Get<InputComponent,VelocityComponent,PositionComponent,CameraFollowTagComponent>())
                    {
                        ref var ic = ref entity.Get<InputComponent>();
                        ref var vc = ref entity.Get<VelocityComponent>();
                        ref var pc = ref entity.Get<PositionComponent>();
                        ref var cf = ref entity.Get<CameraFollowTagComponent>();

                        ic.Mouse = Mouse.GetState();
                        ic.Keyboard = Keyboard.GetState();
                        ic.GamePad = GamePad.GetState(PlayerIndex.One);

                        Scrolling(ref ic, ref cf);
                        Rotating(ref ic, ref pc);

                        var destination = pc.Position;

                        if (ic.Mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (entity.Scene != null)
                                pc.Destination = entity.Scene.Camera.ScreenToWorld(ic.Mouse.Position.ToVector2());
                        }
                        //if (KeyDown(ref inputComponent, PixelGlueButtons.Sprint))
                        //    moveComponent.Velocity *= 10;
                        //if (KeyUp(ref inputComponent, PixelGlueButtons.Sprint))
                        //    moveComponent.Velocity /= 10;
                        if (KeyDown(ref ic, PixelGlueButtons.Up) && pc.Position == pc.Destination)
                            destination.Y -= PixelShared.Pixel.TileSize;
                        if (KeyDown(ref ic, PixelGlueButtons.Down) && pc.Position == pc.Destination)
                            destination.Y += PixelShared.Pixel.TileSize;
                        if (KeyDown(ref ic, PixelGlueButtons.Left) && pc.Position == pc.Destination)
                            destination.X -= PixelShared.Pixel.TileSize;
                        if (KeyDown(ref ic, PixelGlueButtons.Right) && pc.Position == pc.Destination)
                            destination.X += PixelShared.Pixel.TileSize;
                        
                        if (Pressed(ref ic, PixelGlueButtons.EscapeMenu))
                            Environment.Exit(0);
                        if (Pressed(ref ic, PixelGlueButtons.DbgProfiling))
                            Global.Profiling = !Global.Profiling;
                        if (Pressed(ref ic, PixelGlueButtons.DbgSwitchScene))
                            SwitchScene();
                        if (Pressed(ref ic, PixelGlueButtons.DbgProfiling))
                            OpenDialog(scene);
                        if (Pressed(ref ic, PixelGlueButtons.DbgBoundingBoxes))
                        {
                            var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                            system.IsActive = !system.IsActive;
                        }

                        if (destination != pc.Position)
                            pc.Destination = destination;

                        ic.OldKeys = ic.Keyboard.GetPressedKeys();
                    }
                }          

        private static void Rotating(ref InputComponent inputComponent, ref PositionComponent positionComponent)
        {
            if (inputComponent.Keyboard.IsKeyDown(Keys.Left))
                positionComponent.Rotation -= 0.1f;
            if (inputComponent.Keyboard.IsKeyDown(Keys.Right))
                positionComponent.Rotation += 0.1f;
        }

        private static void Scrolling(ref InputComponent inputComponent, ref CameraFollowTagComponent camera)
        {
            if (inputComponent.Mouse.ScrollWheelValue > inputComponent.Scroll)
            {
                camera.Zoom *= 2;
                WorldGen.LayerZero=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), DrawableComponent?>();
                WorldGen.TilesLoading=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), bool>();
            }
            else if (inputComponent.Mouse.ScrollWheelValue < inputComponent.Scroll)
            {
                camera.Zoom /= 2;
                WorldGen.LayerZero=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), DrawableComponent?>();
                WorldGen.TilesLoading=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), bool>();
            }

            inputComponent.Scroll = inputComponent.Mouse.ScrollWheelValue;
        }
        public bool KeyDown(ref InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return component.Keyboard.IsKeyDown(realKey.defaultBind) || component.Keyboard.IsKeyDown(realKey.userBind);
            return false;
        }
        public bool KeyUp(ref InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return component.Keyboard.IsKeyUp(realKey.defaultBind) || component.Keyboard.IsKeyUp(realKey.userBind);
            return false;
        }
        public bool Pressed(ref InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
            {
                for (int i = 0; i < component.OldKeys?.Length; i++)
                {
                    if (component.OldKeys[i] == realKey.defaultBind || component.OldKeys[i] == realKey.userBind)
                        return false;
                }
                return component.Keyboard.IsKeyDown(realKey.userBind) || component.Keyboard.IsKeyDown(realKey.defaultBind);
            }
            return false;
        }

        private static void SwitchScene()
        {
            var testScene2 = new TestingScene2
            {
                Id = 2
            };
            testScene2.Systems.Add(new MoveSystem());
            testScene2.Systems.Add(new CameraSystem());
            SceneManager.ActivateScene(testScene2);
            //SceneManager.DeactivateScene<TestingScene>();
        }

        private static void OpenDialog(Scene scene)
        {
            var player = scene.Find<Player>();
            player.Add(new DialogComponent(player.EntityId, 1, 0));
            //scene.AddComponent(new DialogComponent(player.UniqueId, 1));
        }
    }
}