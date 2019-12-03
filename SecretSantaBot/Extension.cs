using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public static class Extension
    {

        public static Random Random {
            get
            {
                if (_random == null) {
                    _random = new Random(); }
                return _random;
            }
        }
        private static Random _random;

    }
}
