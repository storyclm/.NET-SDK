using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace StoryCLM.SDK.IoT
{
    public class HashableStream : Stream
    {
        Stream _stream;
        HashAlgorithm _hashProvider;

        public long FinalLenght { get; private set; }

        public byte[] Hash { get; private set; }

        public HashableStream(Stream stream, HashAlgorithm hashProvider)
        {
            _stream = stream;
            _hashProvider = hashProvider;
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            int lenght = _stream.Read(buffer, offset, count);
            FinalLenght += lenght;

            if (lenght == 0)
            {
                _hashProvider.TransformFinalBlock(buffer, 0, 0);
                Hash = _hashProvider.Hash;
            }
            else
                _hashProvider.TransformBlock(buffer, offset, lenght, null, 0);

            return lenght;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
