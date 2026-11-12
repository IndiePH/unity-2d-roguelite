using NUnit.Framework;
using UnityEngine;

public class CharacterDashTests
{
    [Test]
    public void StandingDashFollowsFacing()
    {
        Assert.AreEqual(Vector2.right, CharacterDash.Direction(Vector2.zero, 1));
        Assert.AreEqual(Vector2.left, CharacterDash.Direction(Vector2.zero, -1));
    }

    [Test]
    public void MovingDashFollowsTheMove()
    {
        Vector2 diagonal = new Vector2(1f, 1f);
        Assert.AreEqual(diagonal.normalized, CharacterDash.Direction(diagonal, -1));
    }

    [Test]
    public void DashStepStopsAtTheDistance()
    {
        Assert.AreEqual(0.32f, CharacterDash.Step(0f, 16f, 0.02f, 0.6842f), 0.0001f);
        Assert.AreEqual(0.1842f, CharacterDash.Step(0.5f, 16f, 0.02f, 0.6842f), 0.0001f);
        Assert.AreEqual(0f, CharacterDash.Step(0.6842f, 16f, 0.02f, 0.6842f), 0.0001f);
    }
}
