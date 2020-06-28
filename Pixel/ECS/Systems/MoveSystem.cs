using Microsoft.Xna.Framework;
using PixelShared.Maths;
using Pixel.Helpers;
using Pixel.Scenes;
using Pixel.ECS.Components;
using Pixel.Enums;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : IEntitySystem
    {
        public string Name { get; set; } = "Move System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            for (int i = 0; i < SceneManager.ActiveGameScenes.Count; i++)
            {
                var scene = SceneManager.ActiveGameScenes[i];
                foreach (var entity in CompIter.Get<MoveComponent,PositionComponent>(scene))
                {
                    ref var mov = ref entity.Get<MoveComponent>();
                    ref var pc = ref entity.Get<PositionComponent>();

                    MoveOneTile(deltaTime, ref mov, ref pc);
                }
            }
        }

        private static void MoveOneTile(float deltaTime, ref MoveComponent movable, ref PositionComponent position)
        {
            if (movable.Destination != position.Position)
            {
                movable.Moving = true;
                var distanceToDest = PixelMath.GetDistance(position.Position,movable.Destination);
                var moveDistance = movable.Speed * deltaTime;
                if (distanceToDest > moveDistance)
                {
                    var velocity = Vector2.Zero;
                    if (position.Position.X < movable.Destination.X)
                        velocity.X += movable.Speed * deltaTime;
                    if (position.Position.X > movable.Destination.X)
                        velocity.X -= movable.Speed * deltaTime;
                    if (position.Position.Y < movable.Destination.Y)
                        velocity.Y += movable.Speed * deltaTime;
                    if (position.Position.Y > movable.Destination.Y)
                        velocity.Y -= movable.Speed * deltaTime;
                    if (velocity.X != 0 && velocity.Y != 0) // we're moving diagnonally
                        velocity *= 0.707f; // divide by sqrt of 0.5 to fix velocity
                    position.Position += velocity * movable.SpeedMulti;
                }
                else
                {
                    position.Position = movable.Destination;
                    movable.Moving = false;
                }
            }
            else
            {
                movable.Moving = false;
            }
        }
    }
}