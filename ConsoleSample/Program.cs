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
            PrintIfGratherThanZero(playerData.speed, "Speed: ");
            PrintIfGratherThanZero(playerData.throttle, "Throttle: ");
            PrintIfGratherThanZero(playerData.brake, "Brake: ");
            PrintIfGratherThanZero(playerData.gear, "Gear: ");
            PrintIfGratherThanZero(playerData.steer, "Steer: ");
            PrintIfGratherThanZero(playerData.engineRPM, "RPM: ");
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
            PrintIfGratherThanZero(sector1Time, "Sector 1: ");
            PrintIfGratherThanZero(sector2Time, "Sector 2: ");
            PrintIfGratherThanZero(sector3Time, "Sector 3: ");
            PrintIfGratherThanZero(lastLapTime, "Last Lap: ");
        }

        private static void Client_OnCarDamageDataReceive(CarDamagePacket packet)
        {
            Console.WriteLine("----- PLAYER DATA -----");
            Console.WriteLine($"PLAYER INDEX: {packet.header.playerCarIndex}");
            Console.WriteLine($"{JsonSerializer.Serialize(packet.carDamageData[packet.header.playerCarIndex])}");
            Console.WriteLine("----");
        }

        /// <summary>
        /// Prints value to console if it's grather than zero
        /// </summary>
        /// <param name="value">Value to be printed</param>
        private static void PrintIfGratherThanZero(TimeSpan value, string prefix = "")
        {
            if (value.TotalMilliseconds > 0)
                Console.WriteLine($"{prefix} {Math.Round(value.TotalMinutes, 2)}:{Math.Round(value.TotalSeconds, 2)}:{Math.Round(value.TotalMilliseconds, 2)}");
        }

        /// <summary>
        /// Prints value to console if it's grather than zero
        /// </summary>
        /// <param name="value">Value to be printed</param>
        private static void PrintIfGratherThanZero(float value, string prefix = "")
        {
            if (value > 0)
                Console.WriteLine($"{prefix} {value}");
        }
    }
}