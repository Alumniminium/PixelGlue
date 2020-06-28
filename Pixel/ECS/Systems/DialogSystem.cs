using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;

namespace Pixel.ECS.Systems
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
                    //if (!scene.TryGetComponent<DialogComponent>(kvp.Key,out var dialog))
                     
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