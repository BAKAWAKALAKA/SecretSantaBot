using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSantaBot
{
    public interface SessionState
    {
        SessionState NextState(RoomSession session);
    }

    public class StartSessionState: SessionState
    {
       
        public StartSessionState()
        {

        }


        public SessionState NextState(RoomSession session)
        {
            return this;
        }

     
    }


    public class LastSessionState : SessionState
    {
        public SessionState NextState(RoomSession session)
        {





            return null;
        }

        public List<int> Rand(int lenght)
        {
            int n = lenght;
            int[] perm = Enumerable.Range(0, n).ToArray();
            for (int i = n - 1; i >= 1; i--)
            {
                int j = Extension.Random.Next(i + 1);
                // exchange perm[j] and perm[i]
                int temp = perm[j];
                perm[j] = perm[i];
                perm[i] = temp;
            }
            return perm.ToList();
        }
    }

}
