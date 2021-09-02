using System;
using System.Collections.Generic;
using Photon.Realtime;

namespace Game.Events
{
    public static class MessageBus
    {
        public static Action<List<RoomInfo>> RoomListUpdateEvent;
        public static Action JoinedLobbyEvent;
        public static Action<string> JoinedRoomEvent;
        public static Action<string> CreateRoomFailedEvent;
        public static Action<Player> PlayerEnteredRoomEvent;
        public static Action<Player> OtherLeftRoomEvent;
        public static Action MeLeftRoomEvent;
        public static Action<string> CreateRoomEvent;
        public static Action LeaveRoomEvent;
        public static Action<RoomInfo> JoinRoomEvent;
    }
}