using F1Sharp;
using F1Sharp.Data;
using F1Sharp.Packets;
using System.Runtime.InteropServices;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Listening for F1 23...");

            TelemetryClient client = new(20777);

            client.OnCarDamageDataReceive += Client_OnCarDamageDataReceive;
            client.OnLapDataReceive += Client_OnLapDataReceive;

            Console.CursorVisible = false;
            Console.Read();
        }

        private static void Client_OnLapDataReceive(LapDataPacket packet)
        {
            int index = 0;
            Console.Clear();
            foreach (LapData data in packet.lapData)
            {
                var lastLapTime = TimeSpan.FromMilliseconds(data.lastLapTimeInMS);
                Console.WriteLine($"INDEX: {index}");
                Console.WriteLine($"Sectory 1: {data.sector1TimeInMS} ms");
                Console.WriteLine($"Sectory 2: {data.sector2TimeInMS} ms");
                Console.WriteLine($"Sectory 3: - ms");
                Console.WriteLine($"Last Lap: {lastLapTime.TotalMinutes}:{lastLapTime.TotalSeconds}:{lastLapTime.TotalMilliseconds} ms");
                Console.WriteLine("----");
                index++;
                if (index == 5)
                {
                    break;
                }
            }

            Console.WriteLine($"{packet.lapData[packet.header.playerCarIndex]}");
        }

        private static void Client_OnCarDamageDataReceive(CarDamagePacket packet)
        {
            int index = 0;
            Console.Clear();
            foreach (CarDamageData data in packet.carDamageData)
            {
                Console.WriteLine($"INDEX: {index}");
                Console.WriteLine($"{data}");
                Console.WriteLine("----");
                index++;
                if (index == 5)
                {
                    break;
                }
            }

            Console.WriteLine($"{packet.carDamageData[packet.header.playerCarIndex]}");
        }
    }
}