using System;
using TMPro;
using UnityEngine;

namespace Echobay.UISystem
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NicknameField : MonoBehaviour
    {
        public event Action<string> OnNicknameChanged;

        [SerializeField] private TMP_InputField _inputField;

        private void OnValidate()
        {
            if (_inputField == null)
            {
                _inputField = GetComponent<TMP_InputField>();
            }
        }

        private void OnEnable()
        {
            _inputField.onEndEdit.AddListener(NicknameChanged);
        }

        private void OnDisable()
        {
            _inputField.onEndEdit.RemoveListener(NicknameChanged);
        }

        public void SetNickname(string nickname)
        {
            _inputField.text = nickname;
            NicknameChanged(nickname);
        }

        private void NicknameChanged(string nickname)
        {
            OnNicknameChanged?.Invoke(nickname);
        }
    }
}
