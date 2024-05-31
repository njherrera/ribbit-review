﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpers.StringReaderStream
{
    /// <summary>
    /// Convert string to byte stream.
    /// <para>
    /// Slower than <see cref="Encoding.GetBytes()"/>, but saves memory for a large string.
    /// </para>
    /// </summary>

    // taken from xmedeko's answer to this stack overflow question: https://stackoverflow.com/questions/26168205/reading-string-as-a-stream-without-copying/55170901#55170901
    public class StringReaderStream : Stream
    {
        private string input;
        private readonly Encoding encoding;
        private int maxBytesPerChar;
        private int inputLength;
        private int inputPosition;
        private readonly long length;
        private long position;

        public StringReaderStream(string input)
            : this(input, Encoding.UTF8)
        { }

        public StringReaderStream(string input, Encoding encoding)
        {
            this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            this.input = input;
            inputLength = input == null ? 0 : input.Length;
            if (!string.IsNullOrEmpty(input))
                length = encoding.GetByteCount(input);
            maxBytesPerChar = encoding == Encoding.ASCII ? 1 : encoding.GetMaxByteCount(1);
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => length;

        public override long Position
        {
            get => position;
            set => throw new NotImplementedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (inputPosition >= inputLength)
                return 0;
            if (count < maxBytesPerChar)
                throw new ArgumentException("count has to be greater or equal to max encoding byte count per char");
            int charCount = Math.Min(inputLength - inputPosition, count / maxBytesPerChar);
            int byteCount = encoding.GetBytes(input, inputPosition, charCount, buffer, offset);
            inputPosition += charCount;
            position += byteCount;
            return byteCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
