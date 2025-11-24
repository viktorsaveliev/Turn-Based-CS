using Echobay.UISystem;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.NetworkSystem.Lobby.Ui
{
    public class RoomInputFieldUI : PanelUI
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _pasteButton;

        [SerializeField] private CreateOrJoinRoomController _createOrJoinRoomController;

        private void OnEnable()
        {
            _inputField.onValueChanged.AddListener(OnRoomNameChanged);
            _connectButton.onClick.AddListener(OnConnectButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _pasteButton.onClick.AddListener(OnPasteButtonClicked);

            _inputField.text = string.Empty;
            _connectButton.interactable = false;
        }

        private void OnDisable()
        {
            _inputField.onValueChanged.RemoveListener(OnRoomNameChanged);
            _connectButton.onClick.RemoveListener(OnConnectButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _pasteButton.onClick.RemoveListener(OnPasteButtonClicked);
        }

        private void OnRoomNameChanged(string value)
        {
            string upper = value.ToUpper();

            if (upper != value)
            {
                int caretPos = _inputField.caretPosition;
                _inputField.text = upper;
                _inputField.caretPosition = Mathf.Min(caretPos, upper.Length);
            }

            bool isValid = IsValidRoomName(upper);
            _connectButton.interactable = isValid;
        }

        private void OnConnectButtonClicked()
        {
            string roomName = _inputField.text.Trim();

            if (!IsValidRoomName(roomName))
            {
                Debug.LogWarning("Invalid room name. Must be 4 uppercase letters (A–Z).");
                return;
            }

            ConnectToRoom(roomName);
        }

        private void OnBackButtonClicked()
        {
            Hide();
        }

        private void OnPasteButtonClicked()
        {
            string clipboard = GUIUtility.systemCopyBuffer.Trim().ToUpper();

            _inputField.text = clipboard;
            _inputField.caretPosition = clipboard.Length;

            _connectButton.interactable = IsValidRoomName(clipboard);
        }

        private void ConnectToRoom(string roomName)
        {
            _createOrJoinRoomController.JoinRoom(roomName);
            Hide();
        }

        private bool IsValidRoomName(string roomName)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(roomName, @"^[A-Z0-9]{4}$");
        }
    }
}
