using System;
using System.Collections.Generic;

namespace PixelGlueCore.Helpers
{
    public static class ActionQueue
    {
        public static Queue<Action> Items = new Queue<Action>();

        public static void EnqueueAction(Action action)
        {
            if (action == null) return;
            Items.Enqueue(action);
        }
    }
}
