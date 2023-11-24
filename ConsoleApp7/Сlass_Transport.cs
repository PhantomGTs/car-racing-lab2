using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Transport
{
    public string Name { get; set; }
    public int Speed { get; set; }

    public Transport(string name)
    {
        Name = name;
    }
}

public class GroundTransport : Transport
{
    private static readonly Random random = new Random();

    public int RestStopDistance { get; private set; }
    public int TravelTimeToRest { get; private set; }
    public int RestDuration { get; private set; }

    public GroundTransport(string name) : base(name)
    {
        Speed = random.Next(2, 11);
        RestStopDistance = random.Next(1, 11);
        TravelTimeToRest = random.Next(1, 6);
        RestDuration = 2 * RestStopDistance;
    }

    public double GetAdjustedSpeed()
    {
        double timeToRest = TravelTimeToRest;
        double adjustedSpeed = Speed - (timeToRest / RestDuration) * Speed;

        adjustedSpeed = Math.Max(adjustedSpeed, 0);

        return adjustedSpeed;
    }
}



public class AirTransport : Transport
{
    private static readonly Random random = new Random();

    public double AccelerationCoefficient { get; set; }
    public double StopCoefficient { get; private set; }

    public AirTransport(string name) : base(name)
    {
        Speed = GenerateRandomSpeed();
        AccelerationCoefficient = GenerateRandomAccelerationCoefficient();
        StopCoefficient = CalculateStopCoefficient(AccelerationCoefficient);
    }

    private int GenerateRandomSpeed()
    {
        return random.Next(1, 11);
    }

    private double GenerateRandomAccelerationCoefficient()
    {
        return random.NextDouble() * (5.0 - 1.0) + 1.0;
    }

    private double CalculateStopCoefficient(double accelerationCoefficient)
    {
        return accelerationCoefficient / 2.0;
    }
}

