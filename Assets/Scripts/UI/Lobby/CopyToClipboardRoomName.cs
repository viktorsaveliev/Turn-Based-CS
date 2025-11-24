using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.NetworkSystem.Lobby
{
    [RequireComponent(typeof(Button))]
    public class CopyToClipboardRoomName : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private Button _copyButton;

        private void OnValidate()
        {
            if (_copyButton == null)
            {
                _copyButton = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _copyButton.onClick.AddListener(OnCopyButtonClicked);
        }

        private void OnDisable()
        {
            _copyButton.onClick.RemoveListener(OnCopyButtonClicked);
        }

        private void OnCopyButtonClicked()
        {
            string roomName = _roomNameText != null ? _roomNameText.text : string.Empty;

            if (!string.IsNullOrEmpty(roomName))
            {
                GUIUtility.systemCopyBuffer = roomName;
                Debug.Log($"Room name '{roomName}' copied to clipboard.");
            }
            else
            {
                Debug.LogWarning("No room name found to copy.");
            }
        }
    }
}
