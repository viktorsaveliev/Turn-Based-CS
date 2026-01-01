using Echobay.UnitSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.UISystem
{
    public class HealthUI : PanelUI
    {
        [SerializeField] private Unit _damageable;
        [SerializeField] private Image _progressBar;
        [SerializeField] private TMP_Text _valueText;

        private void Start()
        {
            UpdateHealth(0);

            _damageable.OnTakedDamage += UpdateHealth;
            _damageable.OnRecoveryHealth += UpdateHealth;

            _damageable.OnDead += DisableHealth;
        }

        private void OnDestroy()
        {
            _damageable.OnTakedDamage -= UpdateHealth;
            _damageable.OnRecoveryHealth -= UpdateHealth;

            _damageable.OnDead -= DisableHealth;
        }

        private void UpdateHealth(int value)
        {
            float progress = (float) _damageable.CurrentHealth / _damageable.MaxHealth;
            _progressBar.fillAmount = progress;

            _valueText.text = $"{_damageable.CurrentHealth}";
        }

        private void DisableHealth()
        {
            Hide();
        }
    }
}
