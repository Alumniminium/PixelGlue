using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework.Input;
using System;
using PixelGlueCore.Scenes;
using Microsoft.Xna.Framework;
using PixelGlueCore.Entities;
using PixelGlueCore.Configuration;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public class InputSystem : IEntitySystem
    {
        public string Name { get; set; } = "Input System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(double deltaTime)
        {
            foreach (var scene in SceneManager.ActiveScenes)
            {
                foreach (var kvp in scene.Entities)
                {
                    if (!scene.TryGetComponent<InputComponent>(kvp.Key, out var inputComponent))
                        continue;
                    if (!scene.TryGetComponent<MoveComponent>(kvp.Key, out var moveComponent))
                        continue;
                    if (!scene.TryGetComponent<PositionComponent>(kvp.Key, out var positionComponent))
                        continue;

                    scene.TryGetComponent<CameraFollowTagComponent>(kvp.Key, out var camera);

                    inputComponent.Mouse = Mouse.GetState();
                    inputComponent.Keyboard = Keyboard.GetState();
                    inputComponent.GamePad = GamePad.GetState(PlayerIndex.One);
                    
                    Scrolling(inputComponent, camera);
                    Rotating(inputComponent, positionComponent);

                    var destination = positionComponent.Position;

                    if (KeyDown(inputComponent, PixelGlueButtons.Up) && !moveComponent.Moving)
                        destination.Y = positionComponent.Position.Y - scene.Map.TileHeight;
                    if (KeyDown(inputComponent, PixelGlueButtons.Down) && !moveComponent.Moving)
                        destination.Y = positionComponent.Position.Y + scene.Map.TileHeight;
                    if (KeyDown(inputComponent, PixelGlueButtons.Left) && !moveComponent.Moving)
                        destination.X = positionComponent.Position.X - scene.Map.TileWidth;
                    if (KeyDown(inputComponent, PixelGlueButtons.Right) && !moveComponent.Moving)
                        destination.X = positionComponent.Position.X + scene.Map.TileWidth;
                    if (KeyDown(inputComponent, PixelGlueButtons.Sprint) && !moveComponent.Moving)
                        moveComponent.SpeedMulti = 2.5f;
                    if (KeyUp(inputComponent, PixelGlueButtons.Sprint))
                        moveComponent.SpeedMulti = 1;
                    if (Pressed(inputComponent, PixelGlueButtons.EscapeMenu))
                        Environment.Exit(0);
                    if (Pressed(inputComponent, PixelGlueButtons.DbgProfiling))
                        PixelGlue.Profiling = !PixelGlue.Profiling;
                    if (Pressed(inputComponent, PixelGlueButtons.DbgSwitchScene))
                        SwitchScene();
                    if (Pressed(inputComponent, PixelGlueButtons.DbgProfiling))
                        OpenDialog(scene);
                    if (Pressed(inputComponent, PixelGlueButtons.DbgBoundingBoxes))
                    {
                        var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                        system.IsActive = !system.IsActive;
                    }

                    if (destination != positionComponent.Position)
                        moveComponent.Destination = destination;

                    inputComponent.OldKeys = inputComponent.Keyboard.GetPressedKeys();
                }
            }
        }

        private static void Rotating(InputComponent inputComponent, PositionComponent positionComponent)
        {
            if (inputComponent.Keyboard.IsKeyDown(Keys.Left))
                positionComponent.Rotation -= 0.1f;
            if (inputComponent.Keyboard.IsKeyDown(Keys.Right))
                positionComponent.Rotation += 0.1f;
        }

        private static void Scrolling(InputComponent inputComponent, CameraFollowTagComponent camera)
        {
            if (inputComponent.Mouse.ScrollWheelValue > inputComponent.ScrollWheelValue)
                camera.Zoom *= 2;
            else if (inputComponent.Mouse.ScrollWheelValue < inputComponent.ScrollWheelValue)
                camera.Zoom /= 2;

            inputComponent.ScrollWheelValue = inputComponent.Mouse.ScrollWheelValue;
        }

        public bool KeyDown(InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return component.Keyboard.IsKeyDown(realKey.defaultBind) || component.Keyboard.IsKeyDown(realKey.userBind);
            return false;
        }
        public bool KeyUp(InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return component.Keyboard.IsKeyUp(realKey.defaultBind) || component.Keyboard.IsKeyUp(realKey.userBind);
            return false;
        }
        public bool Pressed(InputComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
            {
                for (int i = 0; i < component.OldKeys?.Length; i++)
                {
                    if (component.OldKeys[i] == realKey.defaultBind || component.OldKeys[i] == realKey.userBind)
                        return false;
                }
                return component.Keyboard.IsKeyDown(realKey.userBind) ||component.Keyboard.IsKeyDown(realKey.defaultBind);
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
            SceneManager.DeactivateScene<TestingScene>();
        }

        private static void OpenDialog(Scene scene)
        {
            var player = scene.Find<Player>();
            scene.AddComponent(new DialogComponent(player.UniqueId, 1));
        }
    }
}