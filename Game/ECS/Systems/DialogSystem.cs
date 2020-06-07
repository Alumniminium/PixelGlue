using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.Helpers;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.ECS.Systems
{
    public class DialogSystem : IEntitySystem
    {
        public string Name { get; set; } = "Dialog System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(double deltaTime)
        {
            foreach (var scene in SceneManager.ActiveScenes)
            {
                foreach (var kvp in scene.Entities)
                {
                    if (!scene.TryGetComponent<DialogComponent>(kvp.Key,out var dialog))
                        continue;
                    if(!dialog.UpdateRequired)
                        return;

                    var player = scene.Find<Player>();
                    dialog.UpdateRequired=false;

                    switch(dialog.Id)
                    {
                        default:
                        {
                            FConsole.WriteLine($"* dialog {dialog.Id} not implemented! *");
                            break;
                        }
                        case 1:
                        {
                            switch(dialog.Stage)
                                {
                                    case 0:
                                {
                                    FConsole.WriteLine("*dialog opens*");
                                    dialog.Stage++;
                                    break;
                                }
                                case 1:
                                {
                                    FConsole.WriteLine("Hello World from Dialog Stage "+dialog.Stage);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
