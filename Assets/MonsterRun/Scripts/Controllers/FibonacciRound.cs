namespace Controller
{
    public static class FibonacciRound
    {
        internal static int GetMonstersByRound(int currentRound)
        {
            return CalcFibonacci(currentRound);
        }

        internal static int CalcFibonacci(int number)
        {
            if (number < 3)
                return 1;

            return CalcFibonacci(number - 1) + CalcFibonacci(number - 2);
        }

    }
}
