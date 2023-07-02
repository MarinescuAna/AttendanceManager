namespace AttendanceManagerGenerator.Utils
{
    public static class RandomGenerator
    {
        public static int Next(int min, int max)
            => new Random().Next(min, max);
    }
}
