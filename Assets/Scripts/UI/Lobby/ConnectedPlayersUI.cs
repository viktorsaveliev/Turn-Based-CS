using Echobay.NetworkSystem.Lobby.Rooms;
using Echobay.UISystem;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.NetworkSystem.Lobby.Ui
{
    public class ConnectedPlayersUI : PanelUI
    {
        [SerializeField] private NetworkRoomMediator _networkRoomMediator;
        [SerializeField] private PlayerInfoElement _playerInfoPrefab;
        [SerializeField] private Transform _playersContainer;

        private readonly Dictionary<NetworkRoomPlayer, PlayerInfoElement> _players = new();

        private RoomController RoomController => _networkRoomMediator.RoomController;

        private void OnEnable()
        {
            if (RoomController != null)
            {
                RoomController.OnPlayerJoined += CreatePlayer;
                RoomController.OnPlayerLeft += OnPlayerLeft;
            }

            UpdateList();
        }

        private void OnDisable()
        {
            if (RoomController != null)
            {
                RoomController.OnPlayerJoined -= CreatePlayer;
                RoomController.OnPlayerLeft -= OnPlayerLeft;
            }

            ClearList();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            UpdateList();
        }

        private void UpdateList()
        {
            ClearList();

            NetworkRunner runner = _networkRoomMediator.Runner;

            if (runner == null) return;

            List<NetworkRoomPlayer> players = runner.GetAllBehaviours<NetworkRoomPlayer>();
            print("ConnectedPlayersUI UpdateList found players: " + players.Count);

            foreach (var netPlayer in players)
            {
                if (netPlayer != null)
                    CreatePlayer(netPlayer);
            }
        }

        private void ClearList()
        {
            foreach (var kv in _players)
            {
                var player = kv.Key;
                var element = kv.Value;

                if (player != null)
                    player.OnPlayerReadyChanged -= OnPlayerReadyChanged;

                if (element != null)
                    Destroy(element.gameObject);
            }

            _players.Clear();
        }

        private void OnPlayerLeft(NetworkRoomPlayer player)
        {
            DestroyPlayerInfo(player);
        }

        private void CreatePlayer(NetworkRoomPlayer player)
        {
            if (player == null) return;
            if (_players.ContainsKey(player)) return;

            PlayerInfoElement playerInfo = Instantiate(_playerInfoPrefab, _playersContainer);
            playerInfo.Init(player);

            _players.Add(player, playerInfo);

            player.OnPlayerReadyChanged += OnPlayerReadyChanged;
        }

        private void DestroyPlayerInfo(NetworkRoomPlayer player)
        {
            if (player == null) return;

            if (_players.TryGetValue(player, out PlayerInfoElement playerInfo))
            {
                player.OnPlayerReadyChanged -= OnPlayerReadyChanged;
                Destroy(playerInfo.gameObject);
                _players.Remove(player);
            }
        }

        private void OnPlayerReadyChanged(NetworkRoomPlayer player)
        {
            if (player == null) return;

            if (_players.TryGetValue(player, out PlayerInfoElement playerInfo))
            {
                playerInfo.SetStatus(player.IsReady);
            }
            else
            {
                CreatePlayer(player);
            }
        }
    }
}
