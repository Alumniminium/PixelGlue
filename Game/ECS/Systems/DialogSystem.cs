using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.Enums;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.ECS.Systems
{
    public class DialogSystem : IEntitySystem
    {
        public string Name { get; set; } = "Dialog System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float deltaTime)
        {
        }

        public void Update(float deltaTime)
        {
            foreach (var scene in SceneManager.ActiveGameScenes)
            {
                foreach (var (_,_) in scene.Entities)
                {
                    //if (!scene.TryGetComponent<DialogComponent>(kvp.Key,out var dialog))
                        continue;
                    //if(!dialog.UpdateRequired)
                    //    return;
//
                    //var player = scene.Find<Player>();
                    //dialog.UpdateRequired=false;
//
                    //switch(dialog.Id)
                    //{
                    //    default:
                    //    {
                    //        FConsole.WriteLine($"* dialog {dialog.Id} not implemented! *");
                    //        break;
                    //    }
                    //    case 1:
                    //    {
                    //        switch(dialog.Stage)
                    //            {
                    //                case 0:
                    //            {
                    //                FConsole.WriteLine("*dialog opens*");
                    //                dialog.Stage++;
                    //                break;
                    //            }
                    //            case 1:
                    //            {
                    //                FConsole.WriteLine("Hello World from Dialog Stage "+dialog.Stage);
                    //                break;
                    //            }
                    //        }
                    //        break;
                    //    }
                    //}
                }
            }
        }
    }
}
