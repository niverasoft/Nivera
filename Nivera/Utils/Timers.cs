using System.Diagnostics;
using System.Collections.Generic;

namespace Nivera.Utils
{
    public static class Timers
    {
        private static Dictionary<string, Stopwatch> timers = new Dictionary<string, Stopwatch>();

        public static void StartTimer(string timerName)
        {
            if (!timers.ContainsKey(timerName))
                timers[timerName] = new Stopwatch();

            timers[timerName].Start();
        }

        public static void PauseTimer(string timerName)
        {
            timers[timerName].Stop();
        }

        public static void ResetTimer(string timerName)
        {
            timers[timerName].Reset();
        }

        public static void RestartTimer(string timerName)
        {
            timers[timerName].Restart();
        }

        public static long Milliseconds(string timerName)
        {
            return timers[timerName].ElapsedMilliseconds;
        }

        public static long Ticks(string timerName)
        {
            return timers[timerName].ElapsedTicks;
        }

        public static string MillisecondsToString(string timerName)
        {
            return $"{timerName}: {timers[timerName].ElapsedMilliseconds} ms";
        }

        public static string TicksToString(string timerName)
        {
            return $"{timerName}: {timers[timerName].ElapsedTicks} ticks";
        }

        public static void MillisecondsToLogger(string timerName)
        {
            NiveraLog.Info(MillisecondsToString(timerName));
        }

        public static void TicksToLogger(string timerName)
        {
            NiveraLog.Info(TicksToString(timerName));
        }
    }
}