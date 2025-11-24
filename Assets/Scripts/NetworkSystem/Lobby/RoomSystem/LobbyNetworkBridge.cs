using Echobay.NetworkSystem.Lobby.Rooms;
using Fusion;
using UnityEngine;

namespace Echobay.NetworkSystem.Lobby
{
    public class LobbyNetworkBridge : MonoBehaviour
    {
        [field: SerializeField] public NetworkRunner Runner { get; private set; }
        [field: SerializeField] public RoomController RoomController { get; private set; }
    }

}
