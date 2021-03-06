﻿namespace Shared.TerribleSockets
{
    public class TerribleBuffer
    {
        public byte[] ReceiveBuffer { get; set; }
        public byte[] SendBuffer { get; set; }
        public int BytesInBuffer, BytesRequired, BytesProcessed;
        public byte[] MergeBuffer { get; set; }

        public TerribleBuffer(int receiveBufferSize = 300, int sendBufferSize = 300)
        {
            ReceiveBuffer = new byte[receiveBufferSize];
            SendBuffer = new byte[sendBufferSize];
            MergeBuffer = new byte[receiveBufferSize];
        }
    }
}