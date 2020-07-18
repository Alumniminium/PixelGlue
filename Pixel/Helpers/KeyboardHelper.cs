using Microsoft.Xna.Framework.Input;
using Pixel.Configuration;
using Pixel.ECS.Components;
using Shared.Enums;

namespace Pixel.ECS
{
    public static class KeyboardHelper
    {
        public static bool IsDown(ref KeyboardState kb, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return kb.IsKeyDown(realKey.defaultBind) || kb.IsKeyDown(realKey.userBind);
            return false;
        }
        public static bool IsUp(ref KeyboardState kb, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
                return kb.IsKeyUp(realKey.defaultBind) || kb.IsKeyUp(realKey.userBind);
            return false;
        }
        public static bool IsPressed(ref KeyboardComponent component, PixelGlueButtons key)
        {
            if (UserKeybinds.GenericToKeybinds.TryGetValue(key, out var realKey))
            {
                for (int i = 0; i < component.OldButtons?.Count; i++)
                {
                    if (component.OldButtons[i] == key)
                        return false;
                }
                return Keyboard.GetState().IsKeyDown(realKey.userBind) || Keyboard.GetState().IsKeyDown(realKey.defaultBind);
            }
            return false;
        }
    }
}