﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    private const int MF_BYCOMMAND = 0x00000000;
    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();
    static void Main()
    {

        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
        DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
        Console.SetWindowSize(120, 30);
        Console.SetBufferSize(120, 30);

        int distance = GetDistance();

        RaceType raceType = ChooseRaceType();

        int numParticipants = GetNumberOfParticipants();

        var participants = new List<Transport>();
        for (int i = 0; i < numParticipants; i++)
        {
            Console.WriteLine($"Выберите транспорт для участника {i + 1}:");
            participants.Add(ChooseTransport(participants));
        }

        var raceSimulator = new RaceSimulator(distance, raceType);

        try
        {
            raceSimulator.StartRace(participants);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static int GetDistance()
    {
        Console.Write("Введите дистанцию для гонки(1-100):");
        int distance;
        while (!int.TryParse(Console.ReadLine(), out distance) || distance <= 0 || distance > 100)
        {
            Console.Write("Введите корректное положительное число для дистанции:");
        }
        return distance;
    }

    static RaceType ChooseRaceType()
    {
        Console.WriteLine("Выберите тип гонки:");
        Console.WriteLine("1. Только для наземного транспорта");
        Console.WriteLine("2. Только для воздушного транспорта");
        Console.WriteLine("3. Для всех типов транспортных средств");

        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
        {
            Console.WriteLine("Введите корректное число от 1 до 3:");
        }

        return (RaceType)choice;
    }

    static int GetNumberOfParticipants()
    {
        Console.Write("Введите количество транспортных средств для участия в гонке:");
        int numParticipants;
        while (!int.TryParse(Console.ReadLine(), out numParticipants) || numParticipants <= 0)
        {
            Console.WriteLine("Введите корректное положительное число для количества транспортов:");
        }
        Console.Clear();
        return numParticipants;
    }

    static Transport ChooseTransport(List<Transport> participants)
    {
        Console.Clear();
        Console.WriteLine("Зарегистрированные гонщики:");

        Console.WriteLine("|{0, -25}|{1, -8}|{2, -16}|{3, -13}|{4, -23}|", "Транспорт", "Скорость", "Время до отдыха", "Время отдыха", "коэффициентом ускорения");

        foreach (var participant in participants)
        {
            if (participant is GroundTransport)
            {
                var groundTransport = (GroundTransport)participant;
                Console.WriteLine("|{0, -25}|{1, -8}|{2, -16}|{3, -13}|{4, -23}|", participant.Name, participant.Speed, groundTransport.RestStopDistance, groundTransport.RestDuration, "N/A");
            }
            else if (participant is AirTransport)
            {
                var airTransport = (AirTransport)participant;
                Console.WriteLine("|{0, -25}|{1, -8}|{2, -16}|{3, -13}|{4, -23}|", participant.Name, participant.Speed, "N/A", "N/A", airTransport.AccelerationCoefficient);
            }
            else
            {
                Console.WriteLine("|{0, -25}|{1, -8}|{2, -16}|{3, -13}|{4, -23}|", participant.Name, participant.Speed, "N/A", "N/A", "N/A");
            }
        }

        Console.WriteLine("\n1. Ступа Бабы Яги");
        Console.WriteLine("2. Метла");
        Console.WriteLine("3. Сапоги-скороходы");
        Console.WriteLine("4. Карета-тыква");
        Console.WriteLine("5. Ковер-самолет");
        Console.WriteLine("6. Избушка на курьих ножках");
        Console.WriteLine("7. Кентавр");
        Console.WriteLine("8. Летучий корабль");

        Console.WriteLine("Выберите транспорт (введите число от 1 до 8):");
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 8)
        {
            Console.WriteLine("Введите корректное число от 1 до 8:");
        }

        Console.Clear();
        return CreateTransport(choice);
    }


    static Transport CreateTransport(int choice)
    {
        switch (choice)
        {
            case 1: return new AirTransport("Ступа Бабы Яги");
            case 2: return new AirTransport("Метла");
            case 3: return new GroundTransport("Сапоги-скороходы");
            case 4: return new GroundTransport("Карета-тыква");
            case 5: return new AirTransport("Ковер-самолет");
            case 6: return new GroundTransport("Избушка на курьих ножках");
            case 7: return new GroundTransport("Кентавр");
            case 8: return new AirTransport("Летучий корабль");
            default: return new GroundTransport("Неизвестный транспорт");
        }
    }
}

