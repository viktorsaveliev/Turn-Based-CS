using Cysharp.Threading.Tasks;
using Echobay.PlayerSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem.TurnSystem
{
    public class TurnController : ITurnMaster, ITurnInfo, IInitializable, IDisposable, IMatchObserver
    {
        public event Action<Player> OnTurnGained;
        public event Action<Player> OnTurnLost;

        public int CurrentRound { get; private set; }
        public int TimeRemaining { get; private set; }

        private readonly HashSet<ITurnObserver> _observers = new();
        private readonly List<IPlayerTurn> _turnOrder = new();

        private int _currentPlayerIndex;
        private CancellationTokenSource _turnCts;

        private readonly IMatchMaster _matchMaster;
        private readonly GameplayData _settings;
        private readonly CancellationTokenObject _tokenObject;

        [Inject]
        public TurnController(IMatchMaster matchMaster, GameplayData settings, CancellationTokenObject tokenObject)
        {
            _matchMaster = matchMaster;
            _settings = settings;
            _tokenObject = tokenObject;
        }

        public IPlayerTurn CurrentPlayer =>
            _turnOrder.Count > _currentPlayerIndex ? _turnOrder[_currentPlayerIndex] : null;

        public void Initialize()
        {
            _matchMaster.OnPlayerCreated += AddPlayer;
            _matchMaster.Register(this);
        }

        public void Dispose()
        {
            _matchMaster.OnPlayerCreated -= AddPlayer;
            _matchMaster.Unregister(this);
            _observers.Clear();
        }

        public void OnMatchStarted()
        {
            CurrentRound = 0;
            StartNewRound();
        }

        public void OnMatchEnded()
        {
            EndTurn();
        }

        public void Register(ITurnObserver observer) => _observers.Add(observer);
        public void Unregister(ITurnObserver observer) => _observers.Remove(observer);

        public void AddPlayer(IPlayerTurn player)
        {
            if (!_turnOrder.Contains(player))
                _turnOrder.Add(player);
        }

        public void RemovePlayer(IPlayerTurn player)
        {
            if (_turnOrder.Contains(player))
                _turnOrder.Remove(player);
        }

        private void StartNewRound()
        {
            CurrentRound++;
            _currentPlayerIndex = 0;

            StartTurnForCurrentPlayer();

            foreach (var observer in _observers)
            {
                observer.OnRoundStarted();
            }
        }

        private void StartTurnForCurrentPlayer()
        {
            var player = CurrentPlayer as Player;
            player?.SetActionPoints(_settings.StandardTurnPointsPerRound);

            TimeRemaining = _settings.TimePerTurnInSeconds;

            player.PassTurn();
            OnTurnGained?.Invoke(player);

            _turnCts?.Cancel();
            _turnCts?.Dispose();
            _turnCts = CancellationTokenSource.CreateLinkedTokenSource(_tokenObject.Token);

            StartTimer(_turnCts.Token).Forget();
        }

        private void EndTurn()
        {
            _turnCts?.Cancel();
            _turnCts?.Dispose();
            _turnCts = null;

            foreach (var observer in _observers)
            {
                observer.OnTurnEnded();
            }

            CurrentPlayer?.EndTurn();
            OnTurnLost?.Invoke(CurrentPlayer as Player);

            _currentPlayerIndex++;

            if (_currentPlayerIndex >= _turnOrder.Count)
            {
                StartNewRound();
            }
            else
            {
                StartTurnForCurrentPlayer();
            }
        }

        private async UniTaskVoid StartTimer(CancellationToken token)
        {
            try
            {
                while (TimeRemaining > 0)
                {
                    await UniTask.WaitForSeconds(1, cancellationToken: token);
                    TimeRemaining--;

                    if (TimeRemaining <= 0)
                    {
                        EndTurn();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Timer stopped");
            }
        }
    }
}
