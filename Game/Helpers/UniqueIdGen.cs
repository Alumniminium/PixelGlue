namespace PixelGlueCore.Helpers
{
    public static class UniqueIdGen
    {
        private static uint _lastUid = 0;
        public static uint GetNextUID() => (_lastUid = _lastUid + 1);
    }
}