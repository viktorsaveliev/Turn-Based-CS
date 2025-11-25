using Echobay.ActionContext;
using Echobay.MatchSystem.TurnSystem;
using Echobay.PlayerSystem;
using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class NetworkTurnController : NetworkBehaviour
    {
        [Networked] public int CurrentRound { get; private set; }
        [Networked] public PlayerRef CurrentPlayer { get; private set; }
        [Networked] public int TimeRemaining { get; private set; }

        public event Action<int> OnRoundChanged;
        public event Action<PlayerRef> OnCurrentPlayerChanged;
        public event Action<int> OnTimeChanged;

        private TurnController _turnController;
        private ChangeDetector _changeDetector;
        private NetworkMatchController _networkMatchController;
        private ActionContextLinks _contextLinks;

        [Inject]
        public void Construct(TurnController turnController, NetworkMatchController networkMatchController, ActionContextLinks actionContextLinks)
        {
            _turnController = turnController;
            _networkMatchController = networkMatchController;
            _contextLinks = actionContextLinks;
        }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                _turnController.OnActionPointsSpended += OnActionPointsSpended;
                _turnController.OnRoundStarted += HandleRoundStarted;
                _turnController.OnTurnGained += HandleTurnGained;
            }

            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

            InitClientState();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (Object.HasStateAuthority)
            {
                _turnController.OnActionPointsSpended -= OnActionPointsSpended;
                _turnController.OnRoundStarted -= HandleRoundStarted;
                _turnController.OnTurnGained -= HandleTurnGained;
            }
        }

        public override void Render()
        {
            if (_changeDetector == null) return;

            var changes = _changeDetector.DetectChanges(this, out var prev, out var curr);

            foreach (var name in changes)
            {
                switch (name)
                {
                    case nameof(CurrentRound):
                        {
                            var reader = GetPropertyReader<int>(nameof(CurrentRound));
                            var (_, newVal) = reader.Read(prev, curr);
                            OnRoundChanged?.Invoke(newVal);
                            break;
                        }

                    case nameof(CurrentPlayer):
                        {
                            var reader = GetPropertyReader<PlayerRef>(nameof(CurrentPlayer));
                            var (_, newVal) = reader.Read(prev, curr);

                            if (_networkMatchController.TryGetMatchPlayerByRef(newVal, out MatchPlayer matchPlayer))
                            {
                                _contextLinks.SetCurrentPlayer(matchPlayer);
                            }

                            OnCurrentPlayerChanged?.Invoke(newVal);
                            break;
                        }

                    case nameof(TimeRemaining):
                        {
                            var reader = GetPropertyReader<int>(nameof(TimeRemaining));
                            var (_, newVal) = reader.Read(prev, curr);
                            OnTimeChanged?.Invoke(newVal);
                            break;
                        }
                }
            }
        }

        private void OnActionPointsSpended(MatchPlayer player, int spendedPoints)
        {
            if (!Object.HasStateAuthority) return;

            RPC_UpdatePlayerPoints(player.Data.PlayerRef, player.ActionPoints);
        }

        private void HandleRoundStarted(int round)
        {
            CurrentRound = round;
        }

        private void HandleTurnGained(MatchPlayer player)
        {
            CurrentPlayer = player.Data.PlayerRef;
        }

        private void InitClientState()
        {
            OnRoundChanged?.Invoke(CurrentRound);
            OnCurrentPlayerChanged?.Invoke(CurrentPlayer);
            OnTimeChanged?.Invoke(TimeRemaining);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_UpdatePlayerPoints(PlayerRef playerRef, int actionPointsValue)
        {
            if (_networkMatchController.TryGetMatchPlayerByRef(playerRef, out MatchPlayer matchPlayer))
            {
                matchPlayer.SetActionPoints(actionPointsValue);
            }
        }
    }
}
