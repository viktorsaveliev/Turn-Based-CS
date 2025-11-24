using Cysharp.Threading.Tasks;
using Echobay.NetworkSystem.Lobby.Rooms;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem
{
    public class MatchController : IMatchMaster, IMatchInfo
    {
        public event Action<MatchPlayer> OnPlayerCreated;

        public IReadOnlyList<MatchPlayer> Players => _players;

        private readonly MatchPlayerFactory _playerFactory;
        private readonly UnitSpawner _spawner;
        private readonly CancellationTokenObject _tokenObject;
        private readonly GameplayData _settings;
        private readonly UnitsDatabase _unitsDatabase;

        private readonly HashSet<IMatchObserver> _observers = new();
        private readonly List<MatchPlayer> _players = new();

        private IMatchMode _matchMode;

        [Inject]
        public MatchController(
            MatchPlayerFactory playerFactory, 
            UnitSpawner spawner,
            CancellationTokenObject cancellationTokenObject,
            GameplayData gameplayData,
            UnitsDatabase unitsDatabase)
        {
            _playerFactory = playerFactory;
            _spawner = spawner;
            _tokenObject = cancellationTokenObject;
            _settings = gameplayData;
            _unitsDatabase = unitsDatabase;
        }

        public void Init()
        {
        }

        public void StartMultiplayerMatch(IReadOnlyDictionary<PlayerRef, NetworkRoomPlayer> players)
        {
            List<PlayerConfig> playerConfigs = new();

            foreach (var player in players)
            {
                PlayerConfig config = new()
                {
                    PlayerRef = player.Key,
                    Name = player.Value.PlayerName,
                    TeamID = player.Value.TeamID,
                };

                config.UnitsDataID.CopyFrom(player.Value.UnitsDataID, 0, player.Value.UnitsDataID.Length);
                playerConfigs.Add(config);
            }

            _matchMode = new MultiplayerMatchMode(playerConfigs);
            _matchMode.SetupPlayers(this);
        }

        public MatchPlayer CreatePlayer(PlayerConfig config)
        {
            MatchPlayer player = _playerFactory.CreatePlayer(config);
            _players.Add(player);

            OnPlayerCreated?.Invoke(player);

            Debug.Log($"MatchController | Player created: {player.Data.Name}");
            return player;
        }

        public void SpawnUnits(MatchPlayer player, PlayerConfig config)
        {
            List<UnitData> units = new();

            for (int i = 0; i < config.UnitsDataID.Length; i++)
            {
                if (_unitsDatabase.TryGetUnitDataByID(config.UnitsDataID[i], out UnitData unitData)) 
                {
                    units.Add(unitData);
                }
            }

            _spawner.SpawnPlayerUnits(player, units);
        }

        public async void StartTurns()
        {
            await UniTask.WaitForSeconds(_settings.MatchStartDelayInSeconds, cancellationToken: _tokenObject.Token);

            foreach (IMatchObserver observer in _observers)
            {
                observer.OnMatchStarted();
            }
        }

        public void EndMatch()
        {
            foreach (IMatchObserver observer in _observers)
            {
                observer.OnMatchEnded();
            }
        }

        public void Register(IMatchObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unregister(IMatchObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
