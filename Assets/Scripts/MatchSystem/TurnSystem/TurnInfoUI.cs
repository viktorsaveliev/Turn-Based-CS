using Echobay.NetworkSystem.Match;
using Echobay.UISystem;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem.TurnSystem
{
    public class TurnInfoUI : PanelUI
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _turnText;

        private GameplayData _gameplayData;
        private NetworkTurnController _networkTurnController;

        [Inject]
        public void Construct(GameplayData gameplayData, NetworkTurnController networkTurnController)
        {
            _gameplayData = gameplayData;
            _networkTurnController = networkTurnController;
        }

        private void Awake()
        {
            StartCoroutine(StartMatchDelay());
        }

        private void OnEnable()
        {
            _networkTurnController.OnTimeChanged += UpdateTimer;
            _networkTurnController.OnRoundChanged += OnTurnStarted;
        }

        private void OnDisable()
        {
            _networkTurnController.OnTimeChanged += UpdateTimer;
            _networkTurnController.OnRoundChanged -= OnTurnStarted;
        }

        private void OnTurnStarted(int round)
        {
            _turnText.text = $"Round {_networkTurnController.CurrentRound}";
        }

        private void UpdateTimer(int currentTime)
        {
            _timeText.text = $"{FormatTime(currentTime)}";
        }

        private IEnumerator StartMatchDelay()
        {
            int delay = _gameplayData.MatchStartDelayInSeconds;

            _turnText.text = "Get Ready";

            while (delay > 0)
            {
                _timeText.text = FormatTime(delay);

                yield return new WaitForSeconds(1);

                delay--;
            }
        }

        private string FormatTime(int totalSeconds)
        {
            var time = System.TimeSpan.FromSeconds(totalSeconds);
            return $"{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}
