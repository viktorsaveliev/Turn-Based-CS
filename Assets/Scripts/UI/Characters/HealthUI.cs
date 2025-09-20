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

            _damageable.Health.OnTakedDamage += UpdateHealth;
            _damageable.Health.OnRecoveryHealth += UpdateHealth;

            _damageable.Health.OnDead += DisableHealth;
        }

        private void OnDestroy()
        {
            _damageable.Health.OnTakedDamage -= UpdateHealth;
            _damageable.Health.OnRecoveryHealth -= UpdateHealth;

            _damageable.Health.OnDead -= DisableHealth;
        }

        private void UpdateHealth(int value)
        {
            float progress = (float) _damageable.Health.Value / _damageable.Health.MaxHealth;
            _progressBar.fillAmount = progress;

            _valueText.text = $"{_damageable.Health.Value}";
        }

        private void DisableHealth()
        {
            Hide();
        }
    }
}
