using Echobay.UISystem;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem.TurnSystem
{
    public class TurnInfoUI : PanelUI, ITurnObserver
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _turnText;

        private ITurnMaster _turnMaster;
        private GameplayData _gameplayData;

        private Coroutine _coroutine;

        private bool _isActive;

        [Inject]
        public void Construct(ITurnMaster turnMaster, GameplayData gameplayData)
        {
            _turnMaster = turnMaster;
            _gameplayData = gameplayData;
        }

        private void Awake()
        {
            StartCoroutine(StartMatchDelay());
        }

        private void OnEnable()
        {
            _turnMaster.Register(this);
        }

        private void OnDisable()
        {
            _turnMaster.Unregister(this);
        }

        public void OnTurnStarted()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _isActive = true;
            _coroutine = StartCoroutine(Tick());

            _turnText.text = $"Round {_turnMaster.CurrentRound}";
            UpdateTimer();
        }

        public void OnTurnEnded()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _isActive = false;

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            _timeText.text = $"{FormatTime(_turnMaster.TimeRemaining)}";
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

        private IEnumerator Tick()
        {
            WaitForSeconds waitForSeconds = new(1);

            while (_isActive)
            {
                UpdateTimer();

                yield return waitForSeconds;
            }
        }

        private string FormatTime(int totalSeconds)
        {
            var time = System.TimeSpan.FromSeconds(totalSeconds);
            return $"{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}
