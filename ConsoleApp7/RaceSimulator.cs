using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;
public class RaceSimulator
{
    private int distance;
    private RaceType raceType;

    public RaceSimulator(int distance, RaceType raceType)
    {
        this.distance = distance;
        this.raceType = raceType;
    }

    public void StartRace(List<Transport> participants)
    {

        Console.WriteLine($"Гонка началась на дистанции {distance} условных единиц!");

        // Проверка типа гонки
        CheckRaceType(participants);

        // Инициализация точек старта и финиша
        int startPoint = 0;
        int finishPoint = distance;

        // Установка цветов для курсора, соперников, старта и финиша
        Console.ForegroundColor = ConsoleColor.Cyan; // цвет курсора
        Console.BackgroundColor = ConsoleColor.Black; // цвет фона курсора

        // Визуализация старта и финиша
        Console.SetCursorPosition(startPoint, 1);
        Console.BackgroundColor = ConsoleColor.Green;
        Console.Write("S");

        Console.SetCursorPosition(finishPoint, 1);
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write("F");

        Console.ResetColor(); 

        // Создание и инициализация списков для хранения позиций и результатов
        var positions = new List<int>(participants.Count);
        var results = new List<(Transport, int)>();
        var place = 1;
        for (int i = 0; i < participants.Count; i++)
        {
            positions.Add(startPoint);
        }

        // Визуализация гонки
        while (!positions.TrueForAll(p => p >= finishPoint))
        {
            Console.Clear(); // Очищаем консоль перед каждым шагом

            // Визуализация точек старта и финиша на каждой строке
            for (int i = 0; i < participants.Count; i++)
            {
                Console.SetCursorPosition(startPoint, i);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("S");

                Console.SetCursorPosition(finishPoint, i);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("F");
            }

            // Визуализация курсоров (транспортов)
            for (int i = 0; i < participants.Count; i++)
            {
                Console.SetCursorPosition(positions[i], i);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(">");
            }

            // Ожидание для визуализации
            Thread.Sleep(100);

            // Движение транспортов
            for (int i = 0; i < participants.Count; i++)
            {
                positions[i] += (int)participants[i].Speed;

                // Ограничение, чтобы избежать выхода за пределы финиша
                if (positions[i] > finishPoint)
                {
                    positions[i] = finishPoint;
                }
            }
        }

        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("Гонка завершена!");

        
        // Заполнение списка результатов
        for (int i = 0; i < participants.Count; i++)
        {
            results.Add((participants[i], place++));
        }

        // Сортировка результатов по позиции финиша
        results.Sort((a, b) => a.Item2.CompareTo(b.Item2) * -1);

        // Вывод результатов
        Console.ForegroundColor = ConsoleColor.White; // цвет текста
        Console.BackgroundColor = ConsoleColor.Black; // цвет фона
        Console.Clear();
        Console.ResetColor();
        Console.WriteLine("Результаты гонки:");

        Console.WriteLine("position");
        foreach (int position in positions)
        {
            Console.WriteLine(position);
        }

        Console.WriteLine("transport");
        foreach ((Transport transport, int value) in results)
        {
            Console.WriteLine($"Transport: {transport.Name}, Value: {value}");
        }


        for (int i = 0; i < results.Count; i++)
        {
            //Console.WriteLine($"Место {results[i].Item2}: Транспорт {results[i].Item1.Name}");
            Console.WriteLine($"Место {i + 1}: Транспорт {results[i].Item1.Name}");
        }

        Console.ReadKey();
    }


    private Dictionary<string, int> nameOccurrences = new Dictionary<string, int>();
    private void CheckRaceType(List<Transport> participants)
    {
        List<Transport> incompatibleTransports = new List<Transport>();

        foreach (var transport in participants)
        {
            string originalName = transport.Name;

            if (nameOccurrences.ContainsKey(originalName))
            {
                nameOccurrences[originalName]++;
                transport.Name = $"{originalName} - {nameOccurrences[originalName]}";
            }
            else
            {
                nameOccurrences.Add(originalName, 1);
            }

           // if (raceType == RaceType.GroundRace && !(transport is GroundTransport))
           // {
           //    incompatibleTransports.Add(transport);
           // }

           // if (raceType == RaceType.AirRace && !(transport is AirTransport))
           // {
           //     incompatibleTransports.Add(transport);
           // }

        }

        if (incompatibleTransports.Count > 0)
        {
            Console.WriteLine("Транспорты, которые не подходят на гонку:");
            foreach (var incompatibleTransport in incompatibleTransports)
            {
                Console.WriteLine($"{incompatibleTransport.Name} - {incompatibleTransport.GetType().Name}");
            }

            throw new InvalidTransportTypeException("Один или несколько транспортов не подходят для выбранного типа гонки.");
        }
    }
}

// Исключение для неверного типа транспорта
public class InvalidTransportTypeException : Exception
{
    public InvalidTransportTypeException(string message) : base(message)
    {
    }
}

// Типы гонок
public enum RaceType
{
    GroundRace,
    AirRace,
    AllTypesRace
}
