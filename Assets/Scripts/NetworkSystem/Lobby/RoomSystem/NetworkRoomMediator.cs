using Echobay.Lobby;
using Echobay.NetworkSystem.Lobby.Rooms;
using Fusion;
using Networking;
using System;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Lobby
{
    public class NetworkRoomMediator : MonoBehaviour
    {
        public event Action OnRoomJoined;

        public NetworkRunner Runner => _runner;
        public RoomController RoomController => _networkBridge.RoomController;

        [SerializeField] private CreateOrJoinRoomController _controller;
        [SerializeField] private ReadyButton _readyButton;

        private LocalRoomPlayer _localRoomPlayer;
        private NetworkRunner _runner;
        private LobbyNetworkBridge _networkBridge;
        private NetworkRunnerProvider _networkRunnerProvider;

        [Inject]
        public void Construct(LocalRoomPlayer localPlayer, NetworkRunnerProvider networkRunnerProvider)
        {
            _localRoomPlayer = localPlayer;
            _networkRunnerProvider = networkRunnerProvider;
        }

        private void OnEnable()
        {
            _controller.OnNetworkRunnerCreated += OnNetworkRunnerCreated;
            _controller.OnNetworkRunnerDestroyed += OnNetworkRunnerDestroyed;
            _readyButton.OnReadyStateChanged += OnReadyClicked;

            _localRoomPlayer.OnDataChanged += OnLocalDataChanged;
            
        }

        private void OnDisable()
        {
            _controller.OnNetworkRunnerCreated -= OnNetworkRunnerCreated;
            _controller.OnNetworkRunnerDestroyed -= OnNetworkRunnerDestroyed;
            _readyButton.OnReadyStateChanged -= OnReadyClicked;

            _localRoomPlayer.OnDataChanged -= OnLocalDataChanged;
        }

        private void OnLocalDataChanged()
        {
            if (_runner == null) return;

            NetworkRoomPlayer roomPlayer = _networkBridge.RoomController.GetLocalRoomPlayer();
            roomPlayer.RPC_SetNickname(_localRoomPlayer.PlayerName);

            roomPlayer.RPC_SetUnits(_localRoomPlayer.UnitsDataID);
        }

        private void OnNetworkRunnerCreated(NetworkRunner networkRunner)
        {
            _runner = networkRunner;
            _networkBridge = networkRunner.GetComponent<LobbyNetworkBridge>();
            _networkRunnerProvider.Set(_runner, _networkBridge.RoomController);

            OnRoomJoined?.Invoke();
        }

        private void OnNetworkRunnerDestroyed(NetworkRunner networkRunner)
        {
            _runner = null;
        }

        private void OnReadyClicked(bool isReady)
        {
            if (!_controller.IsConnected) return;

            RoomController roomController = _networkBridge.RoomController;

            if (roomController == null)
            {
                Debug.LogWarning("RoomController is null in NetworkRoomMediator.OnReadyClicked");
                return;
            }

            RoomController.RPC_OnPlayerPressReady(_runner, _runner.LocalPlayer, isReady);
        }
    }

}
