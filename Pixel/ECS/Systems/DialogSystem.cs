using Pixel.Entities;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class DialogSystem : PixelSystem
    {
        public override string Name { get; set; } = "Dialog System";
        
        public DialogSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
    }
}