using System;

namespace TerribleSockets
{
    public class NeutralBuffer
    {
        public byte[] ReceiveBuffer { get; set; }
        public byte[] SendBuffer { get; set; }
        public int BytesInBuffer, BytesRequired, BytesProcessed;
        public byte[] MergeBuffer { get; set; }

        public Span<byte> GetSpan(int index)
        {
            return Span<byte>.Empty;
        }

        public NeutralBuffer(int receiveBufferSize = 300, int sendBufferSize = 300)
        {
            ReceiveBuffer = new byte[receiveBufferSize];
            SendBuffer = new byte[sendBufferSize];
            MergeBuffer = new byte[receiveBufferSize];
        }
    }
}