﻿using Irc.ClassExtensions.CSharpTools;
using Irc.Helpers.CSharpTools;
using NUnit.Framework;

namespace Irc.Extensions.NTLM.Tests;

public class NTLM2SessionResponseTests
{
    [Test]
    public void NTLM2SessionResponse_Test()
    {
        var expectedResult = new byte[]
        {
            0x10, 0xd5, 0x50, 0x83, 0x2d, 0x12, 0xb2, 0xcc, 0xb7, 0x9d, 0x5a, 0xd1, 0xf4, 0xee, 0xd3, 0xdf, 0x82, 0xac,
            0xa4, 0xc3, 0x68, 0x1d, 0xd4, 0x55
        };

        var password = "SecREt01".ToUnicodeString();
        var challenge = new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef};

        var lmResponse = new byte[]
        {
            0xff, 0xff, 0xff, 0x00, 0x11, 0x22, 0x33, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        var ntlmAlgorithms = new NtlmResponses();
        var result =
            ntlmAlgorithms.Ntlm2SessionResponse(password, challenge.ToAsciiString(), lmResponse.ToAsciiString());

        Assert.AreEqual(expectedResult, result);
    }
}