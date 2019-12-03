using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot
{
    public class RoomSession
    {
        public int _room { get; set; }
        private int _userCount { get; set; }
        public List<int> _selectedUsers { get; set; }
        public int CurrentUser { get; set; }
        public SessionState _sessionState { get; set; }


        public RoomSession(int room, int count)
        {
            _room = room;
            _userCount = count;
        }

        public IEnumerable<Message> Next(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
