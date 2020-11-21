using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using System;
using Shared.Enums;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Pixel.Helpers;
using Pixel.Scenes;

namespace Pixel.ECS.Systems
{
    public class PlayerInputSystem : PixelSystem
    {
        public override string Name { get; set; } = "Input System";
        private PixelGlueButtons[] _mappedButtons;

        public PlayerInputSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void Initialize()
        {
            _mappedButtons = (PixelGlueButtons[])Enum.GetValues(typeof(PixelGlueButtons));
            base.Initialize();
        }

        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<KeyboardComponent, PositionComponent>())
                base.AddEntity(entityId);
        }

        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var inp = ref entity.Get<KeyboardComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                EnsureReady(ref inp);

                var keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();

                foreach (var mappedButton in _mappedButtons)
                    if (KeyboardHelper.IsDown(ref keyboard, mappedButton))
                        inp.Buttons.Add(mappedButton);

                Keyboard(ref inp);

                if (inp.Axis.Length() > 0)
                {
                    if(entity.Has<DestinationComponent>())
                    {
                        ref var dst = ref entity.Get<DestinationComponent>();
                        var gridPos = pos.Value * Global.TileSize;
                        gridPos = gridPos / Global.TileSize;
                        dst.Value = gridPos + (inp.Axis * Global.TileSize);
                    }
                    else
                    {
                        ref var dst = ref entity.Add<DestinationComponent>();
                        var gridPos = pos.Value * Global.TileSize;
                        gridPos = gridPos / Global.TileSize;
                        dst.Value = gridPos + (inp.Axis * Global.TileSize);
                    }
                }

                inp.OldButtons.Clear();
                inp.OldButtons.AddRange(inp.Buttons);
                inp.Buttons.Clear();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureReady(ref KeyboardComponent inp)
        {
            if (inp.Buttons == null)
                inp.Buttons = new System.Collections.Generic.List<PixelGlueButtons>();
            if (inp.OldButtons == null)
                inp.OldButtons = new System.Collections.Generic.List<PixelGlueButtons>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Keyboard(ref KeyboardComponent inp)
        {
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
                var system = World.GetSystem<DbgBoundingBoxRenderSystem>();
                system.IsActive = !system.IsActive;
            }
            if (inp.IsPressed(PixelGlueButtons.NameTags))
            {
                var system = World.GetSystem<TextRenderSystem>();
                system.IsActive = !system.IsActive;

                ref var dialog = ref DialogFactory.Create(ref TestingScene.Player, "TEST TEXT", new string[] {"one", "two","three","four"});
                World.Register(ref dialog);
                foreach(var childId in dialog.Children)
                World.Register(childId);
            }
            if (inp.IsPressed(PixelGlueButtons.EscapeMenu))
                Environment.Exit(0);
        }
    }
}