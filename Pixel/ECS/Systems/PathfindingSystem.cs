using Shared.ECS;
using Pixel.Scenes;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class PathfindingSystem : PixelSystem
    {
        public override string Name { get; set; } = "Pathfinding System";
        public Scene Scene => SceneManager.ActiveScene;

        public PathfindingSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override bool MatchesFilter(Entity entity) => entity.Has<PositionComponent, DestinationComponent, SpeedComponent>();
        
        
        //public static void Move(Player player, ushort x, ushort y)
        //{
        //    var xOffset = 0;
        //    var yOffset = 0;

        //    if(playxer.Y > y) // we need to move North
        //        yOffset=-1;
        //    else // we need to move South
        //        yOffset=1;
        //    if(player.X > x) // we need to move West
        //        xOffset = -1;
        //    else // we need to move East
        //        xOffset = 1;
        //    
        //    var newX = player.X + xOffset;
        //    var newY = player.Y + yOffset;

        //    if(CanWalk(newX,newY))
        //    {
        //        player.X = newX;
        //        player.Y =
        //    }
        //}
    }
}