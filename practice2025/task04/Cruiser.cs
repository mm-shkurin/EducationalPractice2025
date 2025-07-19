public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    public void MoveForward() =>
        Console.WriteLine($"Cruiser moving forward at {Speed} units/s");
    public void Rotate(int angle) =>
        Console.WriteLine($"Cruiser rotating by {angle} degrees");
    public void Fire() =>
        Console.WriteLine($"Cruiser firing with power {FirePower}");
}
