// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class FormattingTests
    {
        [Theory]
        [InlineData('G', true, "True")]
        [InlineData('G', false, "False")]
        [InlineData('l', true, "true")]
        [InlineData('l', false, "false")]
        [InlineData(' ', true, "True")]
        [InlineData(' ', false, "False")]
        public void BooleanUtf8(char format, bool value, string expected)
        {
            ParsedFormat f = format == ' ' ? default(ParsedFormat) : new ParsedFormat(format);
            byte[] buffer = new byte[256];
            Assert.True(Utf8Formatter.TryFormat(value, buffer, out int bytesWritten, f));
            var actual = Text.Encoding.UTF8.GetString(buffer, 0, bytesWritten);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData('G', true, "True")]
        [InlineData('G', false, "False")]
        [InlineData('l', true, "true")]
        [InlineData('l', false, "false")]
        [InlineData(' ', true, "True")]
        [InlineData(' ', false, "False")]
        public void BooleanUtf16(char format, bool value, string expected)
        {
            ParsedFormat f = format == ' ' ? default(ParsedFormat) : new ParsedFormat(format);
            byte[] buffer = new byte[256];
            Assert.True(Utf16Formatter.TryFormat(value, buffer, out int bytesWritten, f));
            var actual = Text.Encoding.Unicode.GetString(buffer, 0, bytesWritten);
            Assert.Equal(expected, actual);
        }
    }
}
