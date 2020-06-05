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

                    if (inputComponent.Mouse.ScrollWheelValue > inputComponent.ScrollWheelValue)
                        camera.Zoom = camera.Zoom * 2;
                    else if (inputComponent.Mouse.ScrollWheelValue < inputComponent.ScrollWheelValue)
                        camera.Zoom = camera.Zoom / 2;

                    inputComponent.ScrollWheelValue = inputComponent.Mouse.ScrollWheelValue;


                    if (inputComponent.Keyboard.IsKeyDown(Keys.Left))
                        positionComponent.Rotation -= 0.1f;
                    if (inputComponent.Keyboard.IsKeyDown(Keys.Right))
                        positionComponent.Rotation += 0.1f;


                    var currentKeys = inputComponent.Keyboard.GetPressedKeys();
                    var lastKeys = inputComponent.OldKeys;
                    var destination = positionComponent.Position;
                    
                    for (int i = 0; i < currentKeys.Length; i++)
                    {
                        var key = currentKeys[i];
                        if (UserKeybinds.KeybindsToGeneric.TryGetValue(key, out var pixelButton))
                        {
                            switch (pixelButton)
                            {
                                case PixelGlueButtons.Up:
                                    if (!moveComponent.Moving)
                                        destination.Y = positionComponent.Position.Y - scene.Map.TileHeight;
                                    break;
                                case PixelGlueButtons.Down:
                                    if (!moveComponent.Moving)
                                        destination.Y = positionComponent.Position.Y + scene.Map.TileHeight;
                                    break;
                                case PixelGlueButtons.Left:
                                    if (!moveComponent.Moving)
                                        destination.X =positionComponent.Position.X - scene.Map.TileWidth;
                                    break;
                                case PixelGlueButtons.Right:
                                    if (!moveComponent.Moving)
                                        destination.X = positionComponent.Position.X + scene.Map.TileWidth;
                                    break;

                            }
                        }
                    }

                    if (destination != positionComponent.Position)
                        moveComponent.Destination = destination;

                    for (int i = 0; i < currentKeys.Length; i++)
                    {
                        var key = currentKeys[i];
                        for (int j = 0; j < lastKeys?.Length; j++)
                        {
                            if (lastKeys[j] == key)
                                return;
                        }

                        if (UserKeybinds.KeybindsToGeneric.TryGetValue(key, out var pixelButton))
                        {
                            switch (pixelButton)
                            {
                                case PixelGlueButtons.EscapeMenu:
                                    Environment.Exit(0);
                                    break;
                                case PixelGlueButtons.DbgProfiling:
                                    PixelGlue.Profiling = !PixelGlue.Profiling;
                                    break;
                                case PixelGlueButtons.DbgSwitchScene:
                                    SwitchScene();
                                    break;
                                case PixelGlueButtons.DbgOpenDialog:
                                    OpenDialog(scene);
                                    break;
                                case PixelGlueButtons.DbgBoundingBoxes:
                                    var system = scene.GetSystem<DbgBoundingBoxRenderSystem>();
                                    system.IsActive = !system.IsActive;
                                break;

                            }
                        }
                    }
                    inputComponent.OldKeys = inputComponent.Keyboard.GetPressedKeys();
                }
        }

        private static void SwitchScene()
        {
            var testScene2 = new TestingScene2();
            testScene2.Id = 2;
            testScene2.Systems.Add(new MoveSystem());
            testScene2.Systems.Add(new CameraSystem());
            SceneManager.ActivateScene(testScene2);
            SceneManager.DeactivateScene<TestingScene>();
        }

        private static void OpenDialog(Scene scene)
        {
            var player = scene.Find<Player>();
            scene.AddComponent(player.UniqueId, new DialogComponent(player.UniqueId, 1));
        }
    }
}