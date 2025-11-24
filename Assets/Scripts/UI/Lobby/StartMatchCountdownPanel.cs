using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Echobay.UISystem
{
    public class StartMatchCountdownPanel : PanelUI
    {
        public event Action OnCountdownEnded;

        [SerializeField, Range(3, 10)] private int _countdown = 5;
        [SerializeField] private TMP_Text _countdownText;

        private int _currentCountdown = 0;

        public override void Show()
        {
            base.Show();
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            _currentCountdown = _countdown;

            WaitForSeconds waitForSeconds = new(1f);

            while (_currentCountdown > 0)
            {
                yield return waitForSeconds;

                _currentCountdown--;
                _countdownText.text = $"{_currentCountdown}";
            }

            OnCountdownEnded?.Invoke();
            Hide();
        }
    }
}
