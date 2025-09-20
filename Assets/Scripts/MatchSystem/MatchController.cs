using Cysharp.Threading.Tasks;
using Echobay.PlayerSystem;
using System;
using System.Collections.Generic;
using Zenject;

namespace Echobay.MatchSystem
{
    public class MatchController : IInitializable, IMatchMaster, IMatchInfo
    {
        public event Action<Player> OnPlayerCreated;
        public event Action<Player> OnTurnGained;
        public event Action<Player> OnTurnLost;

        public Player LocalPlayer { get; private set; }

        private readonly PlayerFactory _playerFactory;
        private readonly UnitSpawner _spawner;
        private readonly CancellationTokenObject _tokenObject;
        private readonly GameplayData _settings;

        private readonly HashSet<IMatchObserver> _observers = new();
        private readonly List<Player> _players = new();

        private IMatchMode _matchMode;

        [Inject]
        public MatchController(
            PlayerFactory playerFactory, 
            UnitSpawner spawner,
            CancellationTokenObject cancellationTokenObject,
            GameplayData gameplayData)
        {
            _playerFactory = playerFactory;
            _spawner = spawner;
            _tokenObject = cancellationTokenObject;
            _settings = gameplayData;
        }

        public void Initialize()
        {
            PlayerConfig playerConfig = new()
            {
                Name = "Player",
                TeamID = 0,
                UnitsData = new()
            };

            PlayerConfig botConfig = new()
            {
                Name = "Bot",
                TeamID = 1,
                UnitsData = new()
            };

            IMatchMode mode = new SingleplayerMatchMode(playerConfig, botConfig);

            _matchMode = mode;
            _matchMode.SetupPlayers(this);
        }

        public Player CreatePlayer(PlayerConfig config)
        {
            Player player = _playerFactory.CreatePlayer(config);
            _players.Add(player);

            OnPlayerCreated?.Invoke(player);
            return player;
        }

        public void SetLocalPlayer(Player player)
        {
            LocalPlayer = player;
        }

        public void SpawnUnits(Player player, PlayerConfig config)
        {
            _spawner.SpawnPlayerUnits(player, config.UnitsData);
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
