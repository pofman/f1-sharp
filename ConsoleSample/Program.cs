using System.Text.Json;
using F1Sharp;
using F1Sharp.Packets;

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
            client.OnCarTelemetryDataReceive += Client_OnCarTelemetryDataReceive;

            Console.CursorVisible = false;
            Console.Read();
        }

        private static void Client_OnCarTelemetryDataReceive(CarTelemetryPacket packet)
        {
            var playerData = packet.carTelemetryData[packet.header.playerCarIndex];
            Console.WriteLine($"PLAYER INDEX: {packet.header.playerCarIndex}");
            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"Throtle: {playerData.throttle}");
            Console.WriteLine($"Brake: {playerData.brake}");
            Console.WriteLine($"Gear: {playerData.gear}");
            Console.WriteLine($"Steer: {playerData.steer}");
            Console.WriteLine($"RPM: {playerData.engineRPM}");
        }

        private static void Client_OnLapDataReceive(LapDataPacket packet)
        {
            var playerData = packet.lapData[packet.header.playerCarIndex];
            var lastLapTime = TimeSpan.FromMilliseconds(playerData.lastLapTimeInMS);
            var sector1Time = TimeSpan.FromMilliseconds(playerData.sector1TimeInMS);
            var sector2Time = TimeSpan.FromMilliseconds(playerData.sector2TimeInMS);
            var sector3Time = TimeSpan.FromMilliseconds(playerData.sector3TimeInMS);
            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"PLAYER INDEX: {packet.header.playerCarIndex}");
            Console.WriteLine($"Sector 1: 00:{sector1Time.TotalSeconds}:{sector1Time.TotalMilliseconds}");
            Console.WriteLine($"Sector 2: 00:{sector2Time.TotalSeconds}:{sector2Time.TotalMilliseconds}");
            Console.WriteLine($"Sector 3: 00:{sector3Time.TotalSeconds}:{sector3Time.TotalMilliseconds}");
            Console.WriteLine($"Last Lap: {lastLapTime.TotalMinutes}:{lastLapTime.TotalSeconds}:{lastLapTime.TotalMilliseconds} ms");
        }

        private static void Client_OnCarDamageDataReceive(CarDamagePacket packet)
        {
            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"PLAYER INDEX: {packet.header.playerCarIndex}");
            Console.WriteLine($"{JsonSerializer.Serialize(packet.carDamageData[packet.header.playerCarIndex])}");
            Console.WriteLine("----");
        }
    }
}