using Cysharp.Threading.Tasks;
using Echobay.FightSystem.StatusEffects;
using Echobay.NetworkSystem.Match;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using Fusion;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using static Echobay.Contexts;

namespace Echobay.MatchSystem.TurnSystem
{
    public class TurnController : ITurnMaster, ITurnInfo, IDisposable, IMatchObserver
    {
        public event Action<MatchPlayer> OnTurnGained;
        public event Action<MatchPlayer> OnTurnLost;

        public event Action<MatchPlayer> OnActivateStartTurnEffects;
        public event Action<MatchPlayer> OnActivateEndTurnEffects;

        public event Action<MatchPlayer, int> OnActionPointsSpended;

        public event Action<int> OnRoundStarted;
        public event Action OnTurnEnded;
        public event Action OnTick;

        public int CurrentRound { get; private set; }
        public int TimeRemaining { get; private set; }
        public MatchPlayer CurrentPlayer => _matchMaster.Players[_currentPlayerIndex];


        private readonly HashSet<ITurnObserver> _observers = new();

        private int _currentPlayerIndex;
        private CancellationTokenSource _turnCts;

        private readonly IMatchMaster _matchMaster;
        private readonly GameplayData _settings;

        [Inject]
        public TurnController(IMatchMaster matchMaster, GameplayData settings)
        {
            _matchMaster = matchMaster;
            _settings = settings;
        }

        public void Init()
        {
            _matchMaster.Register(this);
        }

        public void Dispose()
        {
            _turnCts?.Cancel();

            _matchMaster.Unregister(this);
            _observers.Clear();
        }

        public void Register(ITurnObserver observer) => _observers.Add(observer);
        public void Unregister(ITurnObserver observer) => _observers.Remove(observer);

        public void OnMatchStarted()
        {
            CurrentRound = 0;
            StartNewRound();
        }

        public void OnMatchEnded()
        {

        }

        public bool TrySpendPoints(MatchPlayer player, int requiredPoints)
        {
            if (player.ActionPoints < requiredPoints)
            {
                return false;
            }
            
            bool endTurn = player.SpendPoints(requiredPoints);

            if (endTurn)
            {
                EndTurn();
            }

            OnActionPointsSpended?.Invoke(player, requiredPoints);
            return true;
        }

        public void EndTurn()
        {
            EndTurnTask().Forget();
        }

        public async UniTask ProcessEndTurnEffects(MatchPlayer player)
        {
            foreach (Unit unit in player.Units)
            {
                ExecuteStatusEffectContext context = new()
                {
                    Executer = unit
                };

                await unit.OnTurnEnded(context);
            }
        }

        public async UniTask ProcessStartTurnEffects(MatchPlayer player)
        {
            foreach (Unit unit in player.Units)
            {
                ExecuteStatusEffectContext context = new()
                {
                    Executer = unit
                };

                await unit.OnTurnStarted(context);
            }
        }

        private async UniTaskVoid EndTurnTask()
        {
            _turnCts?.Cancel();

            var player = CurrentPlayer;
            OnTurnLost?.Invoke(player);

            OnActivateEndTurnEffects?.Invoke(player);
            await ProcessEndTurnEffects(player);

            OnTurnEnded?.Invoke();

            _currentPlayerIndex++;

            bool startNewRound = false;
            if (_currentPlayerIndex >= _matchMaster.Players.Count)
            {
                _currentPlayerIndex = 0;
                startNewRound = true;
            }

            await UniTask.Delay(1000);

            OnActivateStartTurnEffects?.Invoke(CurrentPlayer);
            await ProcessStartTurnEffects(CurrentPlayer);

            await UniTask.Delay(1000);

            if (startNewRound)
            {
                StartNewRound();
            }
            else
            {
                StartTurn();
            }
        }

        private void StartNewRound()
        {
            CurrentRound++;
            _currentPlayerIndex = 0;
            OnRoundStarted?.Invoke(CurrentRound);

            StartTurn();

            foreach (ITurnObserver observer in _observers)
            {
                observer.OnTurnStarted();
            }
        }

        private void StartTurn()
        {
            TimeRemaining = _settings.TimePerTurnInSeconds;
            CurrentPlayer.SetActionPoints(_settings.StandardTurnPointsPerRound);

            OnTurnGained?.Invoke(CurrentPlayer);

            _turnCts?.Cancel();
            _turnCts = new CancellationTokenSource();

            RunTimer(_turnCts.Token).Forget();
        }

        private async UniTaskVoid RunTimer(CancellationToken token)
        {
            try
            {
                while (TimeRemaining > 0)
                {
                    await UniTask.Delay(1000, cancellationToken: token);
                    TimeRemaining--;

                    OnTick?.Invoke();

                    if (TimeRemaining <= 0)
                    {
                        EndTurn();
                    }
                }
            }
            catch
            {
                Debug.Log("Timer stopped");
            }
        }
    }
}