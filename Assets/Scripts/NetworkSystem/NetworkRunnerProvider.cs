using Echobay.NetworkSystem.Lobby.Rooms;
using Fusion;
using UnityEngine;

namespace Echobay.NetworkSystem
{
    public class NetworkRunnerProvider
    {
        public NetworkRunner Runner { get; private set; }
        public RoomController RoomController { get; private set; }

        public void Set(NetworkRunner runner, RoomController controller)
        {
            Runner = runner;
            RoomController = controller;
        }
    }
}
