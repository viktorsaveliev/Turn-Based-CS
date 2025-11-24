using Echobay.PlayerSystem;
using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Lobby.Rooms
{
    public class RoomController : SimulationBehaviour
    {
        public event Action<NetworkRoomPlayer> OnPlayerJoined;
        public event Action<NetworkRoomPlayer> OnPlayerLeft;

        public event Action OnAllPlayersReady;

        public IReadOnlyDictionary<PlayerRef, NetworkRoomPlayer> Players => _players;

        [SerializeField] private NetworkPrefabRef _roomPlayerPrefab;
        [SerializeField] private NetworkEvents _events;

        private LocalRoomPlayer _localRoomPlayer;
        private readonly Dictionary<PlayerRef, NetworkRoomPlayer> _players = new();

        [Inject]
        public void Construct(LocalRoomPlayer localRoomPlayer)
        {
            _localRoomPlayer = localRoomPlayer;
        }

        private void OnEnable()
        {
            _events.PlayerJoined.AddListener(OnPlayerJoinedRoom);
            _events.PlayerLeft.AddListener(OnPlayerLeftRoom);
        }

        private void OnDisable()
        {
            _events.PlayerJoined.RemoveListener(OnPlayerJoinedRoom);
            _events.PlayerLeft.RemoveListener(OnPlayerLeftRoom);
        }

        public void OnPlayerJoinedRoom(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer != player) return;

            PlayerConfig playerConfig = new()
            {
                PlayerRef = player,
                Name = _localRoomPlayer.PlayerName,
                TeamID = 0
            };

            playerConfig.UnitsDataID.CopyFrom(_localRoomPlayer.UnitsDataID, 0, _localRoomPlayer.UnitsDataID.Length);

            RPC_OnPlayerConnectedRoom(runner, playerConfig);
        }

        public void OnPlayerLeftRoom(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;

            if (_players.TryGetValue(player, out NetworkRoomPlayer roomPlayer))
            {
                roomPlayer.OnPlayerReadyChanged -= OnPlayerReadyChanged;

                RPC_OnPlayerLeft(runner, roomPlayer);

                Runner.Despawn(roomPlayer.Object);
                _players.Remove(player);

                foreach (var p in _players.Values)
                {
                    p.RPC_SetReady(false);
                }
            }
        }

        public void OnPlayerReadyChanged(NetworkRoomPlayer roomPlayer)
        {
            if (!Runner.IsServer) return;

            if (_players.Count > 0 && _players.Values.All(p => p.IsReady))
            {
                StartMatch();
            }
        }

        public NetworkRoomPlayer GetLocalRoomPlayer()
        {
            return _players.Values.FirstOrDefault(p => p.Object.HasInputAuthority);
        }

        public void OnAllReady()
        {
            OnAllPlayersReady?.Invoke();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public static void RPC_OnPlayerPressReady(NetworkRunner runner, PlayerRef playerRef, bool isReady)
        {
            LobbyNetworkBridge lobbyNetworkBridge = runner.GetComponent<LobbyNetworkBridge>();
            RoomController roomController = lobbyNetworkBridge.RoomController;

            if (roomController.Players.TryGetValue(playerRef, out NetworkRoomPlayer roomPlayer))
            {
                roomPlayer.RPC_SetReady(isReady);
            }
        }

        private void StartMatch()
        {
            foreach (var p in _players.Values)
            {
                //p.RPC_SetReady(false);
            }

            RPC_SendPlayersStatusToAll(Runner);
        }

        private void CreatePlayer(PlayerConfig playerConfig)
        {
            if (!Runner.IsServer) return;

            NetworkObject roomPlayerObject = Runner.Spawn(
                _roomPlayerPrefab,
                inputAuthority: playerConfig.PlayerRef,
                flags: NetworkSpawnFlags.DontDestroyOnLoad
            );

            NetworkRoomPlayer roomPlayer = roomPlayerObject.GetComponent<NetworkRoomPlayer>();

            playerConfig.TeamID = _players.Count;
            roomPlayer.RPC_SetupPlayer(playerConfig);
            roomPlayer.OnPlayerReadyChanged += OnPlayerReadyChanged;

            Runner.SetPlayerObject(playerConfig.PlayerRef, roomPlayerObject);

            _players.Add(playerConfig.PlayerRef, roomPlayer);

            RPC_OnPlayerCreated(Runner, roomPlayer);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private static void RPC_SendPlayersStatusToAll(NetworkRunner runner)
        {
            LobbyNetworkBridge lobbyNetworkBridge = runner.GetComponent<LobbyNetworkBridge>();
            lobbyNetworkBridge.RoomController.OnAllReady();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private static void RPC_OnPlayerCreated(NetworkRunner runner, NetworkRoomPlayer roomPlayer)
        {
            LobbyNetworkBridge lobbyNetworkBridge = runner.GetComponent<LobbyNetworkBridge>();
            lobbyNetworkBridge.RoomController.OnPlayerJoined?.Invoke(roomPlayer);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private static void RPC_OnPlayerLeft(NetworkRunner runner, NetworkRoomPlayer roomPlayer)
        {
            LobbyNetworkBridge lobbyNetworkBridge = runner.GetComponent<LobbyNetworkBridge>();
            lobbyNetworkBridge.RoomController.OnPlayerLeft?.Invoke(roomPlayer);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private static void RPC_OnPlayerConnectedRoom(NetworkRunner runner, PlayerConfig playerConfig)
        {
            LobbyNetworkBridge lobbyNetworkBridge = runner.GetComponent<LobbyNetworkBridge>();
            lobbyNetworkBridge.RoomController.CreatePlayer(playerConfig);
        }
    }
}
