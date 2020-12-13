using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBot
{
    public static class Extension
    {

        public static IDriver TelegramDriver;

        public static Random Random {
            get
            {
                if (_random == null) {
                    _random = new Random(); }
                return _random;
            }
        }
        private static Random _random;

        public static List<int> Rand(int lenght)
        {
            if (lenght < 1)
            {
                throw new ArgumentException("lenght can't be less than 1");
            }
            if (lenght == 1)
            {
                return new List<int>() { 0 };
            }
            if (lenght == 2)
            {
                return new List<int>() { 1, 0 };
            }
            int n = lenght;
            int[] perm = Enumerable.Range(0, n).ToArray();
            for (int i = n - 1; i >= 0; i--)
            {
                int j = Extension.Random.Next(lenght);
                while (i == j || i == perm[j] || perm[i] == j) { j = Extension.Random.Next(lenght); }
                // exchange perm[j] and perm[i]
                int temp = perm[j];
                perm[j] = perm[i];
                perm[i] = temp;
            }
            return perm.ToList();
        }

    }
}
