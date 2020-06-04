using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework.Input;
using System;
using PixelGlueCore.Scenes;
using Microsoft.Xna.Framework;
using PixelGlueCore.Entities;

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
                foreach (var kvp in scene.GameObjects)
                {
                    if (!kvp.Value.TryGetComponent<InputComponent>(out var inputComponent))
                        continue;
                    if (!kvp.Value.TryGetComponent<MoveComponent>(out var moveComponent))
                        continue;
                    if (!kvp.Value.TryGetComponent<PositionComponent>(out var positionComponent))
                        continue;

                    kvp.Value.TryGetComponent<CameraFollowTagComponent>(out var camera);

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
                    var lastKeys = inputComponent.OldKeyboard.GetPressedKeys();

                    for (int i = 0; i < currentKeys.Length; i++)
                    {
                        var key = currentKeys[i];
                        for (int j = 0; j < lastKeys.Length; j++)
                        {
                            if (lastKeys[j] == key)
                                return;
                        }

                        switch (key)
                        {
                            case Keys.Escape:
                                Environment.Exit(0);
                                break;

                            case Keys.P:
                                PixelGlue.Profiling = !PixelGlue.Profiling;
                                break;
                            case Keys.T:
                                var testScene2 = new TestingScene2();
                                testScene2.Id = 2;
                                testScene2.Systems.Add(new MoveSystem());
                                testScene2.Systems.Add(new CameraSystem());
                                SceneManager.ActivateScene(testScene2);
                                SceneManager.DeactivateScene<TestingScene>();
                                break;
                            case Keys.O:
                                var player = SceneManager.Find<Player>();
                                player.AddComponent(new DialogComponent(1));
                                break;
                        }
                    }
                    lastKeys = currentKeys;


                    if (moveComponent.Moving)
                        return;

                    var destination = positionComponent.Position;
                    if (inputComponent.Keyboard.IsKeyDown(Keys.A))
                        destination.X -= scene.Map.TileWidth;
                    if (inputComponent.Keyboard.IsKeyDown(Keys.D))
                        destination.X += scene.Map.TileWidth;
                    if (inputComponent.Keyboard.IsKeyDown(Keys.W))
                        destination.Y -= scene.Map.TileHeight;
                    if (inputComponent.Keyboard.IsKeyDown(Keys.S))
                        destination.Y += scene.Map.TileHeight;

                    if (destination != positionComponent.Position)
                        moveComponent.Destination = destination;
                }
        }
    }
}