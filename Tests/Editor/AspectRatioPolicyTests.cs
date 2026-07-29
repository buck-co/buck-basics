// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class AspectRatioPolicyTests
{
    AspectRatioPolicy GetPolicy(AspectRatioPolicy.Modes mode, float minAspect, float maxAspect)
    {
        AspectRatioPolicy policy = ScriptableObject.CreateInstance<AspectRatioPolicy>();
        policy.SetValues(mode, minAspect, maxAspect);
        return policy;
    }

    // A minimum of 1.6 is the "no 4:3" policy: it admits 16:10 and everything wider.
    AspectRatioPolicy GetSixteenTenMinimum()
        => GetPolicy(AspectRatioPolicy.Modes.Minimum, 1.6f, 2.4f);

    [Test]
    public void OffModeAllowsEveryAspectRatio()
    {
        AspectRatioPolicy policy = GetPolicy(AspectRatioPolicy.Modes.Off, 1.6f, 2.4f);

        Assert.IsFalse(policy.IsActive);
        Assert.IsTrue(policy.IsAllowed(1024, 768));
        Assert.IsTrue(policy.IsAllowed(1280, 1024));
        Assert.IsTrue(policy.IsAllowed(1920, 1080));
        Assert.IsTrue(policy.IsAllowed(5120, 1440));
    }

    [Test]
    public void MinimumModeAllowsSixteenTenAndWider()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();

        Assert.IsTrue(policy.IsActive);

        // 1280x800 is exactly 1.6 and sits on the bound. The Steam Deck's native size, so it must pass.
        Assert.IsTrue(policy.IsAllowed(1280, 800));
        Assert.IsTrue(policy.IsAllowed(1920, 1200));

        // 1366x768 is 683:384, not 16:9, but still 1.7786.
        Assert.IsTrue(policy.IsAllowed(1366, 768));
        Assert.IsTrue(policy.IsAllowed(1360, 768));

        Assert.IsTrue(policy.IsAllowed(1280, 720));
        Assert.IsTrue(policy.IsAllowed(1920, 1080));
        Assert.IsTrue(policy.IsAllowed(3840, 2160));

        // 2560x1080 is 64:27 and 3440x1440 is 43:18. Both are "21:9" in marketing terms.
        Assert.IsTrue(policy.IsAllowed(2560, 1080));
        Assert.IsTrue(policy.IsAllowed(3440, 1440));

        // 32:9.
        Assert.IsTrue(policy.IsAllowed(5120, 1440));
    }

    [Test]
    public void MinimumModeRejectsFourThreeAndFiveFour()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();

        // 4:3.
        Assert.IsFalse(policy.IsAllowed(640, 480));
        Assert.IsFalse(policy.IsAllowed(1024, 768));
        Assert.IsFalse(policy.IsAllowed(1152, 864));
        Assert.IsFalse(policy.IsAllowed(1600, 1200));

        // 5:4.
        Assert.IsFalse(policy.IsAllowed(1280, 1024));

        // 3:2 is narrower than 16:10, so a 1.6 minimum drops it too.
        Assert.IsFalse(policy.IsAllowed(2256, 1504));
    }

    [Test]
    public void MaximumModeRejectsAnythingWider()
    {
        AspectRatioPolicy policy = GetPolicy(AspectRatioPolicy.Modes.Maximum, 1.6f, 1.778f);

        // 1920x1080 is 1.77778, just over a hand-typed 1.778. The epsilon must let it through.
        Assert.IsTrue(policy.IsAllowed(1920, 1080));
        Assert.IsTrue(policy.IsAllowed(1280, 800));

        Assert.IsFalse(policy.IsAllowed(2560, 1080));
        Assert.IsFalse(policy.IsAllowed(3440, 1440));

        // Maximum mode ignores the minimum, so narrow ratios still pass.
        Assert.IsTrue(policy.IsAllowed(1024, 768));
    }

    [Test]
    public void RangeModeRejectsBothExtremes()
    {
        AspectRatioPolicy policy = GetPolicy(AspectRatioPolicy.Modes.Range, 1.6f, 1.8f);

        Assert.IsTrue(policy.IsAllowed(1280, 800));
        Assert.IsTrue(policy.IsAllowed(1920, 1080));
        Assert.IsTrue(policy.IsAllowed(1366, 768));

        Assert.IsFalse(policy.IsAllowed(1024, 768));
        Assert.IsFalse(policy.IsAllowed(3440, 1440));
    }

    [Test]
    public void ExactBoundsAreAllowed()
    {
        AspectRatioPolicy policy = GetPolicy(AspectRatioPolicy.Modes.Range, 1.6f, 2.388889f);

        // Both of these land exactly on a bound.
        Assert.IsTrue(policy.IsAllowed(1280, 800));
        Assert.IsTrue(policy.IsAllowed(3440, 1440));
    }

    [Test]
    public void NonPositiveSizesAreNeverAllowed()
    {
        AspectRatioPolicy minimum = GetSixteenTenMinimum();

        Assert.IsFalse(minimum.IsAllowed(0, 1080));
        Assert.IsFalse(minimum.IsAllowed(1920, 0));
        Assert.IsFalse(minimum.IsAllowed(0, 0));
        Assert.IsFalse(minimum.IsAllowed(-1920, -1080));

        // Off is not an excuse to accept a garbage size.
        AspectRatioPolicy off = GetPolicy(AspectRatioPolicy.Modes.Off, 1.6f, 2.4f);
        Assert.IsFalse(off.IsAllowed(0, 1080));
        Assert.IsFalse(off.IsAllowed(1920, 0));
    }

    [Test]
    public void GetRatioHandlesNonPositiveSizes()
    {
        Assert.IsTrue(Mathf.Abs(AspectRatioPolicy.GetRatio(1920, 1080) - 1.777778f) < 0.0001f);
        Assert.IsTrue(Mathf.Abs(AspectRatioPolicy.GetRatio(1280, 800) - 1.6f) < 0.0001f);
        Assert.IsTrue(AspectRatioPolicy.GetRatio(1920, 0) == 0f);
        Assert.IsTrue(AspectRatioPolicy.GetRatio(0, 1080) == 0f);
    }

    [Test]
    public void ResolveNearestAllowedReturnsDesiredWhenAlreadyAllowed()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();
        Vector2Int[] candidates = { new Vector2Int(1280, 720) };

        // Already allowed, so it must not snap to something else.
        Assert.AreEqual(new Vector2Int(1920, 1080),
            policy.ResolveNearestAllowed(new Vector2Int(1920, 1080), candidates));
    }

    [Test]
    public void ResolveNearestAllowedPrefersLargestSizeThatFits()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();
        Vector2Int[] candidates =
        {
            new Vector2Int(1920, 1080),
            new Vector2Int(1280, 720),
            new Vector2Int(1024, 576),
            new Vector2Int(800, 600)
        };

        // 1920x1080 and 1280x720 are allowed but wider than the display. 800x600 is 4:3.
        // Nearest-by-area would wrongly pick 1280x720, which does not fit on a 1024x768 screen.
        Assert.AreEqual(new Vector2Int(1024, 576),
            policy.ResolveNearestAllowed(new Vector2Int(1024, 768), candidates));
    }

    [Test]
    public void ResolveNearestAllowedFallsBackToSmallestWhenNothingFits()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();
        Vector2Int[] candidates = { new Vector2Int(1920, 1080), new Vector2Int(1280, 720) };

        Assert.AreEqual(new Vector2Int(1280, 720),
            policy.ResolveNearestAllowed(new Vector2Int(640, 480), candidates));
    }

    [Test]
    public void ResolveNearestAllowedReturnsDesiredWhenNothingIsAllowed()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();
        Vector2Int[] candidates = { new Vector2Int(800, 600), new Vector2Int(640, 480) };

        // A genuinely 4:3-only display. Never leave the caller without a usable size.
        Assert.AreEqual(new Vector2Int(1024, 768),
            policy.ResolveNearestAllowed(new Vector2Int(1024, 768), candidates));
    }

    [Test]
    public void ResolveNearestAllowedHandlesEmptyAndNullCandidates()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();
        Vector2Int desired = new Vector2Int(1024, 768);

        Assert.AreEqual(desired, policy.ResolveNearestAllowed(desired, new Vector2Int[0]));
        Assert.AreEqual(desired, policy.ResolveNearestAllowed(desired, null));
    }

    [Test]
    public void ResolveNearestAllowedIsOrderIndependent()
    {
        AspectRatioPolicy policy = GetSixteenTenMinimum();

        // Both are allowed, both fit, and both have an area of 2073600. The width tie-break decides.
        Vector2Int wide = new Vector2Int(2880, 720);
        Vector2Int tall = new Vector2Int(1920, 1080);
        Vector2Int desired = new Vector2Int(4000, 4000);

        Assert.AreEqual(wide, policy.ResolveNearestAllowed(desired, new[] { wide, tall }));
        Assert.AreEqual(wide, policy.ResolveNearestAllowed(desired, new[] { tall, wide }));
    }
}
