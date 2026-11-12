using NUnit.Framework;

public class CharacterAnimationTests
{
    [Test]
    public void LocomotionUsesIdleRunningAndSliding()
    {
        Assert.AreEqual(CharacterAnimation.Idle, CharacterAnimation.State(false, false, false, false));
        Assert.AreEqual(CharacterAnimation.Running, CharacterAnimation.State(false, false, false, true));
        Assert.AreEqual(CharacterAnimation.Sliding, CharacterAnimation.State(false, false, true, true));
        Assert.AreEqual(CharacterAnimation.Sliding, CharacterAnimation.State(false, false, true, false));
    }

    [Test]
    public void HurtAndDeathTakeOver()
    {
        Assert.AreEqual(CharacterAnimation.Hurt, CharacterAnimation.State(false, true, true, true));
        Assert.AreEqual(CharacterAnimation.Dying, CharacterAnimation.State(true, true, true, true));
    }
}
