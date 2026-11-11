using NUnit.Framework;

public class OpenSpawnPointTests
{
    [Test]
    public void RollZeroSkipsClosedPoints()
    {
        int index = OpenSpawnPoint.Index(new[] { false, true, false, true }, 0);

        Assert.AreEqual(1, index);
    }

    [Test]
    public void SecondRollTakesTheNextOpenPoint()
    {
        int index = OpenSpawnPoint.Index(new[] { false, true, false, true }, 1);

        Assert.AreEqual(3, index);
    }

    [Test]
    public void RollPastTheOpenPointsFails()
    {
        int index = OpenSpawnPoint.Index(new[] { false, true }, 1);

        Assert.AreEqual(-1, index);
    }

    [Test]
    public void NoOpenPointFails()
    {
        int index = OpenSpawnPoint.Index(new[] { false, false }, 0);

        Assert.AreEqual(-1, index);
    }
}
