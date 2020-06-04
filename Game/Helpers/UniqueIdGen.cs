namespace PixelGlueCore.Helpers
{
    public static class UniqueIdGen
    {
        private static int _lastUid = 0;
        public static int GetNextUID() => (_lastUid = _lastUid + 1);
    }
}