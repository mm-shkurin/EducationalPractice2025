public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 70;
    public void MoveForward() =>
        Console.WriteLine($"Fighter moving forward at {Speed} units/s");
    public void Rotate(int angle) =>
        Console.WriteLine($"Fighter rotating by {angle} degrees");
    public void Fire() =>
        Console.WriteLine($"Fighter firing with power {FirePower}");
}
