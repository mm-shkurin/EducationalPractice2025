using Xunit;
using Moq;
using System.Linq;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveLowerFirePowerThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    [Fact]
    public void MoveForward_ShouldBeCallable()
    {
        var mock = new Mock<ISpaceship>();
        mock.Setup(m => m.Speed).Returns(100);
        mock.Object.MoveForward();
        mock.Verify(m => m.MoveForward(), Times.Once);
    }

    [Fact]
    public void Rotate_ShouldHandleDifferentAngles()
    {
        var mock = new Mock<ISpaceship>();
        var angles = new[] { 30, 45, 90, 180 };
        angles.ToList().ForEach(angle => mock.Object.Rotate(angle));
        mock.Verify(m => m.Rotate(It.IsAny<int>()), Times.Exactly(4));
        mock.Verify(m => m.Rotate(90), Times.Once);
    }

    [Fact]
    public void Fire_ShouldBeExecuted()
    {
        var mock = new Mock<ISpaceship>();

        Enumerable.Range(0, 2).ToList().ForEach(_ => mock.Object.Fire());

        mock.Verify(m => m.Fire(), Times.Exactly(2));
    }

    [Fact]
    public void Cruiser_ShouldImplementISpaceship()
    {
        var cruiser = new Cruiser();
        Assert.IsAssignableFrom<ISpaceship>(cruiser);
    }

    [Fact]
    public void Fighter_ShouldImplementISpaceship()
    {
        var fighter = new Fighter();
        Assert.IsAssignableFrom<ISpaceship>(fighter);
    }
}
