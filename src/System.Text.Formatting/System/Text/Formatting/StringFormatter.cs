﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text;

namespace System.Text.Formatting
{
    public class StringFormatter : ITextOutput, IDisposable
    {
        byte[] _buffer;
        int _count;
        EncodingData _culture = EncodingData.InvariantUtf16;
        ArrayPool<byte> _pool;

        public StringFormatter(ArrayPool<byte> pool) : this(64, pool)
        {
            Clear();
        }

        public StringFormatter(int capacity, ArrayPool<byte> pool)
        {
            _pool = pool;
            _buffer = _pool.Rent(capacity * 2);
        }

        public void Dispose()
        {
            _pool.Return(_buffer);
            _buffer = null;
            _count = 0;
        }

        public void Append(char character) {
            _buffer[_count++] = (byte)character;
            _buffer[_count++] = (byte)(character >> 8);
        }

        //TODO: this should use Span<byte>
        public void Append(string text)
        {
            foreach (char character in text)
            {
                Append(character);
            }
        }

        //TODO: this should use Span<byte>
        public void Append(ReadOnlySpan<char> substring)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                Append(substring[i]);
            }
        }

        public void Clear()
        {
            _count = 0;
        }
        public override string ToString()
        {
            var text = Text.Encoding.Unicode.GetString(_buffer, 0, _count);
            return text;
        }

        Span<byte> IOutput.Buffer
        {
            get { return new Span<byte>(_buffer, _count, _buffer.Length - _count); }
        }

        public EncodingData Encoding
        {
            get
            {
                return _culture;
            }
            set
            {
                _culture = value;
            }
        }

        void IOutput.Enlarge(int desiredBufferLength)
        {
            var newSize = desiredBufferLength + _buffer.Length - _count;
            if(desiredBufferLength == 0){
                newSize = _buffer.Length * 2;
            }

            var temp = _buffer;
            _buffer = _pool.Rent(newSize);
            Array.Copy(temp, 0, _buffer, 0, _count);
            _pool.Return(temp);
        }

        void IOutput.Advance(int bytes)
        {
            _count += bytes;
        }
    }
}
