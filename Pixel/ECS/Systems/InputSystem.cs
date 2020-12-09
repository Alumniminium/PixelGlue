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
using System.Collections.Generic;

namespace Pixel.ECS.Systems
{
    public class PlayerInputSystem : PixelSystem<KeyboardComponent, PositionComponent>
    {
        private PixelGlueButtons[] _mappedButtons;

        public PlayerInputSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) 
        {
            Name = "Input System";
         }

        public override void Initialize()
        {
            _mappedButtons = (PixelGlueButtons[])Enum.GetValues(typeof(PixelGlueButtons));
            base.Initialize();
        }        

        public override void Update(float deltaTime, GCNeutralList<Entity> Entities)
        {
            for(int i =0; i< Entities.Count; i++)
            {
                ref var entity = ref Entities[i];
                
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
                    ref var dst = ref entity.Has<DestinationComponent>() ? ref entity.Get<DestinationComponent>() : ref entity.Add<DestinationComponent>();
                    var tileX = ((int)pos.Value.X+(Global.TileSize/2)) / Global.TileSize;
                    var tileY = ((int)pos.Value.Y+(Global.TileSize/2)) / Global.TileSize;
                    tileX = tileX * Global.TileSize;
                    tileY = tileY * Global.TileSize;
                    tileX = tileX + (int)(inp.Axis.X * Global.TileSize);
                    tileY = tileY + (int)(inp.Axis.Y * Global.TileSize);
                    
                    dst.Value = new Vector2(tileX,tileY);
                }

                inp.OldButtons.Clear();
                inp.OldButtons.AddRange(inp.Buttons);
                inp.Buttons.Clear();
            }
        }

        private static void EnsureReady(ref KeyboardComponent inp)
        {
            if (inp.Buttons == null)
                inp.Buttons = new System.Collections.Generic.List<PixelGlueButtons>();
            if (inp.OldButtons == null)
                inp.OldButtons = new System.Collections.Generic.List<PixelGlueButtons>();
        }

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

                //1ref var dialog = ref DialogFactory.Create(ref TestingScene.Player, "TEST TEXT", new string[] {"one", "two","three","four"});
                //World.Register(dialog.EntityId);
            }
            if (inp.IsPressed(PixelGlueButtons.EscapeMenu))
                Environment.Exit(0);
        }
    }
}