using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Helpers;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.ECS.Systems
{
    public class MoveSystem : IEntitySystem
    {
        public string Name { get; set; } = "Move System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(double deltaTime)
        {
            for (int i = 0; i < SceneManager.ActiveScenes.Count; i++)
            {
                var scene = SceneManager.ActiveScenes[i];
                foreach (var kvp in scene.Entities)
                {
                    if (!scene.TryGetComponent<MoveComponent>(kvp.Key, out var movable))
                        continue;
                    if (movable.Destination == Vector2.Zero)
                        continue;
                    if (!scene.TryGetComponent<PositionComponent>(kvp.Key, out var position))
                        continue;
                    if (movable.Destination != position.Position)
                    {
                        movable.Moving = true;
                        var distanceToDest = TwoMath.GetDistance(position.Position, movable.Destination);
                        var moveDistance = movable.Speed * deltaTime;
                        if (distanceToDest > moveDistance)
                        {
                            var velocity = Vector2.Zero;
                            if (position.Position.X < movable.Destination.X)
                                velocity.X += (float)(movable.Speed * deltaTime);
                            if (position.Position.X > movable.Destination.X)
                                velocity.X -= (float)(movable.Speed * deltaTime);
                            if (position.Position.Y < movable.Destination.Y)
                                velocity.Y += (float)(movable.Speed * deltaTime);
                            if (position.Position.Y > movable.Destination.Y)
                                velocity.Y -= (float)(movable.Speed * deltaTime);
                            if (velocity.X != 0 && velocity.Y != 0) // we're moving diagnonally
                                velocity = velocity * 0.707f; // divide by sqrt of 0.5 to fix velocity
                            position.Position = position.Position + (velocity * movable.SpeedMulti);
                        }
                        else
                        {
                            position.Position = movable.Destination;
                            movable.Moving = false;
                        }
                    }
                    else
                        movable.Moving = false;
                }
            }
        }
    }
}