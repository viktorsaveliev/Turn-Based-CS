using Echobay.ActionContext;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.CardSystem
{
    [RequireComponent(typeof(Button))]
    public abstract class Card : MonoBehaviour, ICard
    {
        public event Action<Card> OnClicked;

        [SerializeField] private CardData _data;
        [SerializeField] private Button _button;

        [Title("Base Info")]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _bg;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;

        [Header("Turn cost UI")]
        [SerializeField] private Image _turnBG;
        [SerializeField] private TMP_Text _turnCost;

        [Header("Energy cost UI")]
        [SerializeField] private Image _energyBG;
        [SerializeField] private TMP_Text _energyCost;

        [Header("Energy gain UI")]
        [SerializeField] private Image _energyGainBG;
        [SerializeField] private TMP_Text _energyGain;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public virtual void Init(CardData cardData)
        {
            _data = cardData;

            _icon.sprite = _data.Icon;
            _bg.sprite = _data.BG;
            _name.text = _data.Name;
            _description.text = _data.Description;

            if (_data.TurnCost > 0)
            {
                _turnCost.text = _data.TurnCost.ToString();
            }
            else
            {
                _turnBG.gameObject.SetActive(false);
                _turnCost.gameObject.SetActive(false);
            }

            if (_data.EnergyCost > 0)
            {
                _energyCost.text = _data.EnergyCost.ToString();
            }
            else
            {
                _energyBG.gameObject.SetActive(false);
                _energyCost.gameObject.SetActive(false);
            }

            if (_data.EnergyGain > 0)
            {
                _energyGain.text = _data.EnergyGain.ToString();
            }
            else
            {
                _energyGainBG.gameObject.SetActive(false);
                _energyGain.gameObject.SetActive(false);
            }
        }

        public T GetData<T>() where T : CardData => _data as T;

        public void OnClick()
        {
            OnClicked?.Invoke(this);
        }
    }
}
