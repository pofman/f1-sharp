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
            foreach (LapData data in packet.lapData)
            {
                var lLapTime = TimeSpan.FromMilliseconds(data.lastLapTimeInMS);
                Console.WriteLine($"INDEX: {index}");
                Console.WriteLine($"Sector 1: {data.sector1TimeInMS} ms");
                Console.WriteLine($"Sector 2: {data.sector2TimeInMS} ms");
                Console.WriteLine($"Sector 3: {data.sector3TimeInMS} ms");
                Console.WriteLine($"Last Lap: {lLapTime.TotalMinutes}:{lLapTime.TotalSeconds}:{lLapTime.TotalMilliseconds} ms");
                Console.WriteLine("----");
                index++;
                if (index == 5)
                {
                    break;
                }
            }

            var playerData = packet.lapData[packet.header.playerCarIndex];
            var lastLapTime = TimeSpan.FromMilliseconds(playerData.lastLapTimeInMS);
            var sector1Time = TimeSpan.FromMilliseconds(playerData.sector1TimeInMS);
            var sector2Time = TimeSpan.FromMilliseconds(playerData.sector2TimeInMS);
            var sector3Time = TimeSpan.FromMilliseconds(playerData.sector3TimeInMS);
            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"Sector 1: 00:{sector1Time.TotalSeconds}:{sector1Time.TotalMilliseconds}");
            Console.WriteLine($"Sector 2: 00:{sector2Time.TotalSeconds}:{sector2Time.TotalMilliseconds}");
            Console.WriteLine($"Sector 3: 00:{sector3Time.TotalSeconds}:{sector3Time.TotalMilliseconds}");
            Console.WriteLine($"Last Lap: {lastLapTime.TotalMinutes}:{lastLapTime.TotalSeconds}:{lastLapTime.TotalMilliseconds} ms");
            Console.WriteLine($"{playerData}");
        }

        private static void Client_OnCarDamageDataReceive(CarDamagePacket packet)
        {
            int index = 0;
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

            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"INDEX: {packet.header.playerCarIndex}");
            Console.WriteLine($"{packet.carDamageData[packet.header.playerCarIndex]}");
            Console.WriteLine("----");
        }
    }
}