namespace BlazorAppRazeni
{
    public static class ReservoarSampling
    {
        public static int[] Generate(int n, int max)
        {
            if(n > max)
            {
                throw new ArgumentException("n must be less than or equal to max");
            }

            int[] pole = new int[n];

            for (int i = 0; i < max; ++i)
            {
                if (i < n)
                {
                    pole[i] = i + 1;
                }
                else
                {
                    int randomIndex = Random.Shared.Next(0, i + 1);

                    if (randomIndex < n)
                    {
                        pole[randomIndex] = i + 1;
                    }
                }
            }

            Random.Shared.Shuffle(pole);

            return pole;
        }
    }
}
