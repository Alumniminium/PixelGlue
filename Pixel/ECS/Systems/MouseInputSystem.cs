using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.IO;

namespace Pixel.ECS.Systems
{
    public class MouseInputSystem : PixelSystem
    {
        public override string Name { get; set; } = "Mouse Input System";

        public MouseInputSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override bool MatchesFilter(Entity entity) => entity.Has<MouseComponent>();
        
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
                ref var mos = ref entity.Get<MouseComponent>();

                mos.OldState = mos.CurrentState;
                mos.CurrentState = Mouse.GetState();

                if(mos.OldState==mos.CurrentState)
                    continue;

                var worldPos = cam.ScreenToWorld(mos.CurrentState.Position.ToVector2());
                mos.WorldX = worldPos.X;
                mos.WorldY = worldPos.Y;
                
                if (mos.CurrentState.LeftButton == ButtonState.Pressed && mos.OldState.LeftButton != ButtonState.Pressed)
                {
                    var tilePos = new Vector2((int)worldPos.X / Global.TileSize,(int)worldPos.Y / Global.TileSize);
                    tilePos.X = (int)tilePos.X * Global.TileSize;
                    tilePos.Y = (int)tilePos.Y * Global.TileSize;
                    List<Entity> foundEntities = new List<Entity>();
                    foreach (var e in World.GetEntities())
                    {
                        if (e.Has<PositionComponent>())
                        {
                            ref readonly var pos = ref e.Get<PositionComponent>();
                            var tileCenter = pos.Value + Global.HalfTileSizeVector;
                            
                            if (pos.Value == tilePos || Math.Abs(Vector2.Distance(tileCenter, worldPos)) <= Global.TileSize/2)
                            {
                                foundEntities.Add(e);
                                FConsole.WriteLine($"[MouseInputSystem] Click at {worldPos.X},{worldPos.Y} found Entity #{e.EntityId}");
                                if(Global.Prefabs.TryGetValue(e,out var prefab))
                                {
                                    FConsole.WriteLine($"Found Prefab for Entity {e.EntityId}!");
                                    FConsole.WriteLine($"It's a {prefab.ToString()}");

                                    if(prefab is IInteractable iPrefab)
                                        iPrefab.Activate();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}