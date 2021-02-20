namespace Core.Utilities
{
    public static class CronMaker
    {

        public static string MinutelyCron(int minute)
        {
            return $"*/{minute} * * * * ";
        }
        public static string HourlyCron(int hour)
        {
            return $"* */{hour} * * * ";
        }
        public static string DailyCron(int minute)
        {
            return $"* * * * * ";
        }
        public static string YearlyCron(int minute)
        {
            return $"* * * * * ";
        }
    }
}
