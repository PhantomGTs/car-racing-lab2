using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Transport
{
    public string Name { get; set; }
    public double Speed { get; set; }

    public Transport(string name)
    {
        Name = name;
    }
}

public class GroundTransport : Transport
{
    public int RestStopDistance { get; private set; }
    public int TravelTimeToRest { get; private set; }
    public int RestDuration { get; private set; }

    public GroundTransport(string name, double speed, int restStopDistance, int travelTimeToRest) : base(name)
    {
        Speed = Math.Round(GetAdjustedSpeed(speed, restStopDistance, travelTimeToRest));
        RestStopDistance = restStopDistance;
        TravelTimeToRest = travelTimeToRest;
        RestDuration = 2 * RestStopDistance;
    }

    private double GetAdjustedSpeed(double initialSpeed, double restStopDistance, double travelTimeToRest)
    {
        double timeToRest = TravelTimeToRest;
        //double adjustedSpeed = initialSpeed - (timeToRest / RestDuration) * initialSpeed;
        double adjustedSpeed = initialSpeed - timeToRest;

        adjustedSpeed = Math.Max(adjustedSpeed, 0);

        return adjustedSpeed;
    }
}


public class AirTransport : Transport
{
    public double AccelerationCoefficient { get; set; }
    public double StopCoefficient { get; private set; }

    public AirTransport(string name, double speed, double accelerationCoefficient) : base(name)
    {
        Speed = Math.Round(CalculateStopCoefficient(speed, accelerationCoefficient)); ;
        AccelerationCoefficient = accelerationCoefficient;
    }

    private double CalculateStopCoefficient(double speed ,double accelerationCoefficient)
    {
        return speed * accelerationCoefficient / 2.0;
    }
}

