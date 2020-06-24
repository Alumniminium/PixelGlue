using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework.Input;
using System;
using PixelGlueCore.Scenes;
using Microsoft.Xna.Framework;
using PixelGlueCore.Configuration;
using PixelGlueCore.Enums;
using PixelGlueCore.Entities;

namespace PixelGlueCore.ECS.Systems
{
    public class InputSystem : IEntitySystem
    {
        public string Name { get; set; } = "Input System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            foreach (var s in SceneManager.ActiveGameScenes)
            {
                if (s is GameScene scene)
                {
                    foreach (var (_, entity) in scene.Entities)
                    {
                        if (!entity.Has<InputComponent>() ||
                            !entity.Has<MoveComponent>() ||
                            !entity.Has<PositionComponent>() ||
                            !entity.Has<CameraFollowTagComponent>())
                            continue;


                        ref var inputComponent = ref entity.Get<InputComponent>();
                        ref var moveComponent = ref entity.Get<MoveComponent>();
                        ref var positionComponent = ref entity.Get<PositionComponent>();
                        ref var camera = ref entity.Get<CameraFollowTagComponent>();

                        inputComponent.Mouse = Mouse.GetState();
                        inputComponent.Keyboard = Keyboard.GetState();
                        inputComponent.GamePad = GamePad.GetState(PlayerIndex.One);

                        Scrolling(ref inputComponent, ref camera);
                        Rotating(ref inputComponent, ref positionComponent);

                        var destination = positionComponent.Position;

                        if (inputComponent.Mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (entity.Scene != null)
                                moveComponent.Destination = entity.Scene.Camera.ScreenToWorld(inputComponent.Mouse.Position.ToVector2());
                        }
                        if (KeyDown(ref inputComponent, PixelGlueButtons.Sprint) && !moveComponent.Moving)
                            moveComponent.SpeedMulti = 100f;
                        if (KeyDown(ref inputComponent, PixelGlueButtons.Up) && !moveComponent.Moving)
                            destination.Y = positionComponent.Position.Y - (PixelGlue.TileSize * moveComponent.SpeedMulti);
                        if (KeyDown(ref inputComponent, PixelGlueButtons.Down) && !moveComponent.Moving)
                            destination.Y = positionComponent.Position.Y + (PixelGlue.TileSize * moveComponent.SpeedMulti);
                        if (KeyDown(ref inputComponent, PixelGlueButtons.Left) && !moveComponent.Moving)
                            destination.X = positionComponent.Position.X - (PixelGlue.TileSize * moveComponent.SpeedMulti);
                        if (KeyDown(ref inputComponent, PixelGlueButtons.Right) && !moveComponent.Moving)
                            destination.X = positionComponent.Position.X + (PixelGlue.TileSize * moveComponent.SpeedMulti);
                        if (KeyUp(ref inputComponent, PixelGlueButtons.Sprint))
                            moveComponent.SpeedMulti = 1;
                        if (Pressed(ref inputComponent, PixelGlueButtons.EscapeMenu))
                            Environment.Exit(0);
                        if (Pressed(ref inputComponent, PixelGlueButtons.DbgProfiling))
                            PixelGlue.Profiling = !PixelGlue.Profiling;
                        if (Pressed(ref inputComponent, PixelGlueButtons.DbgSwitchScene))
                            SwitchScene();
                        if (Pressed(ref inputComponent, PixelGlueButtons.DbgProfiling))
                            OpenDialog(scene);
                        if (Pressed(ref inputComponent, PixelGlueButtons.DbgBoundingBoxes))
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
                PixelGlue.Z *=2f;
                camera.Zoom *= 2;
                WorldGen.LayerZero=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), DrawableComponent?>();
                WorldGen.TilesLoading=new System.Collections.Concurrent.ConcurrentDictionary<(int x, int y), bool>();
            }
            else if (inputComponent.Mouse.ScrollWheelValue < inputComponent.Scroll)
            {
                PixelGlue.Z /=2f;
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
            SceneManager.DeactivateScene<TestingScene>();
        }

        private static void OpenDialog(GameScene scene)
        {
            var player = scene.Find<Player>();
            player.Add(new DialogComponent(player.EntityId, 1, 0));
            //scene.AddComponent(new DialogComponent(player.UniqueId, 1));
        }
    }
}