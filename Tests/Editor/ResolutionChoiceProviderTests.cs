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

    static readonly Vector2Int[] k_typicalMonitorSizes =
    {
        new Vector2Int(2560, 1440),
        new Vector2Int(2560, 1600),
        new Vector2Int(1920, 1200),
        new Vector2Int(1920, 1080),
        new Vector2Int(1680, 1050),
        new Vector2Int(1600, 900),
        new Vector2Int(1366, 768),
        new Vector2Int(1280, 720),
    };

    [Test]
    public void PickAutoSizeFullscreenReturnsNative()
    {
        var native = new Vector2Int(2560, 1440);
        Assert.AreEqual(native,
            ResolutionChoiceProvider.PickAutoSize(native, k_typicalMonitorSizes, windowed: false));
    }

    [Test]
    public void PickAutoSizeWindowedStepsDownAtSameAspect()
    {
        // 16:9 native steps down past the 16:10 entries to the largest smaller 16:9 size.
        Assert.AreEqual(new Vector2Int(1920, 1080),
            ResolutionChoiceProvider.PickAutoSize(new Vector2Int(2560, 1440), k_typicalMonitorSizes, windowed: true));

        // 16:10 native likewise ignores the 16:9 entries.
        Assert.AreEqual(new Vector2Int(1680, 1050),
            ResolutionChoiceProvider.PickAutoSize(new Vector2Int(1920, 1200), k_typicalMonitorSizes, windowed: true));
    }

    [Test]
    public void PickAutoSizeWindowedRequiresBothAxesSmaller()
    {
        // 1920x1080 is not "smaller" than a 1920x1200 native (equal width), so it must not be picked
        // even before the aspect check rules it out.
        var candidates = new[] { new Vector2Int(1920, 1080), new Vector2Int(1600, 1000) };
        Assert.AreEqual(new Vector2Int(1600, 1000),
            ResolutionChoiceProvider.PickAutoSize(new Vector2Int(1920, 1200), candidates, windowed: true));
    }

    [Test]
    public void PickAutoSizeWindowedFallsBackToNativeWhenNoSmallerSameAspect()
    {
        var native = new Vector2Int(1280, 720);
        Assert.AreEqual(native,
            ResolutionChoiceProvider.PickAutoSize(native, new[] { new Vector2Int(1280, 800) }, windowed: true));
    }

    [Test]
    public void PickAutoSizeWindowedToleratesNearIdenticalAspects()
    {
        // 1366x768 (1.7786) counts as 16:9 (1.7778) for the step-down.
        Assert.AreEqual(new Vector2Int(1366, 768),
            ResolutionChoiceProvider.PickAutoSize(new Vector2Int(1600, 900),
                new[] { new Vector2Int(1366, 768), new Vector2Int(1280, 800) }, windowed: true));
    }
}
