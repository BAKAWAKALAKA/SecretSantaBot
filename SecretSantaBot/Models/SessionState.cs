using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretSantaBot.Interfaces;

namespace SecretSantaBot
{
    

    public class StartSessionState: ISessionState
    {
       
        public StartSessionState()
        {

        }


        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            //inicial session
            // create message for start
            session.SessionState = new WaitAllResponState();
            throw new NotImplementedException();
        }
    }


    public class WaitAllResponState : ISessionState
    {
        List<int> selectedUsers;

        public WaitAllResponState()
        {
            selectedUsers = new List<int>();
        }

        public IEnumerable<Message> NextState(Message message, RoomSession session)
        {
            var result = new List<Message>();
            RememberRespondedUser();
            if (!isAllUsersRespond())
            {
                // stay this state
                // send resdonped user directly message
                throw new NotImplementedException();
            }
            else
            {
                // respond all and go form this state
                foreach(var user in selectedUsers)
                {
                    result.Add(CreateDirectMessage(user, ""));
                }
                session.SessionState = null;
            }
            return result;
        }

        private void RememberRespondedUser()
        {
            throw new NotImplementedException();
        }

        private bool isAllUsersRespond()
        {
            throw new NotImplementedException();
        }

        private Message CreateDirectMessage(int user, string text)
        {
            throw new NotImplementedException();
        }
    }



}
