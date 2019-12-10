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
            var result = new List<Message>();
            result.Add(new Message()
            {
                Room = message.Room,
                Text = "Хо хо хо! Скоро новый год! Я Распределил для каждого своего серктного санту! Нажми хочу узнать чей я санта и я прошепчу тебе на ушко в личной переписке"
            });
            session.SessionState = new WaitAllResponState();
            return result;
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
            RememberRespondedUser(message.User);
            if (session.Selected.Count < message.UserCount)
            {
                // stay this state
                // send resdonped user directly message
                result.Add(CreateDirectMessage(message.Room, message.User.id,"check"));
            }
            else
            {
                // respond all and go form this state
                var list = Extension.Rand(session.Selected.Count);
                for(int i=0; i<session.Selected.Count;i++)
                {
                    //
                    result.Add(CreateDirectMessage(message.Room, session.Selected[i].id, $"ты даришь подарок {session.Selected[list[i]].nam}"));
                }
                session.SessionState = null;
            }
            return result;
        }

        private void RememberRespondedUser(User user)
        {
            throw new NotImplementedException();
        }

        private bool isAllUsersRespond()
        {
            throw new NotImplementedException();
        }

        private Message CreateDirectMessage(int chat, int user, string text)
        {
            throw new NotImplementedException();
        }
    }



}
