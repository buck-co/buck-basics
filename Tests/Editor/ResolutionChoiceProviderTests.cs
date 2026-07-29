// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class ResolutionChoiceProviderTests
{
    [Test]
    public void ToIdFormatsSize()
    {
        Assert.AreEqual("1920x1080", ResolutionChoiceProvider.ToId(new Vector2Int(1920, 1080)));
        Assert.AreEqual("1280x800", ResolutionChoiceProvider.ToId(new Vector2Int(1280, 800)));
    }

    [Test]
    public void TryParseIdRoundTripsToId()
    {
        Vector2Int original = new Vector2Int(2560, 1440);

        Assert.IsTrue(ResolutionChoiceProvider.TryParseId(ResolutionChoiceProvider.ToId(original), out var parsed));
        Assert.AreEqual(original, parsed);
    }

    [Test]
    public void TryParseIdRejectsMalformedInput()
    {
        // The ID format is machine generated, so anything that isn't exactly "<w>x<h>" is a bad save value.
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId(null, out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId(string.Empty, out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("1920", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("axb", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("1920x1080x60", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("1920X1080", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId(" 1920x1080", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("-100x50", out _));
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("0x0", out _));
    }

    [Test]
    public void TryParseIdLeavesSizeAtDefaultOnFailure()
    {
        Assert.IsFalse(ResolutionChoiceProvider.TryParseId("nonsense", out var size));
        Assert.AreEqual(default(Vector2Int), size);
    }
}
