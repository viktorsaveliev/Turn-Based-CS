using Echobay.Lobby;
using Echobay.NetworkSystem.Lobby.Ui;
using Echobay.UISystem;
using Fusion;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.NetworkSystem.Lobby
{
    public class RoomControlUI : MonoBehaviour
    {
        [SerializeField] private CreateOrJoinRoomController _createOrJoinRoomController;

        [SerializeField] private GameObject _inRoomObject;
        [SerializeField] private GameObject _outRoomObject;

        [Header("Buttons")]
        [SerializeField] private Button _fastPlayButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Button _leaveRoomButton;
        [SerializeField] private ReadyButton _readyButton;

        [Header("UI")]
        [SerializeField] private PanelUI _roomInputFieldUI;
        [SerializeField] private TMP_Text _roomNameText;

        private void OnValidate()
        {
            if (_createOrJoinRoomController == null)
            {
                _createOrJoinRoomController = GetComponent<CreateOrJoinRoomController>();
            }
        }

        private void OnEnable()
        {
            _fastPlayButton.onClick.AddListener(OnFastPlayButtonClicked);
            _createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
            _joinRoomButton.onClick.AddListener(OnJoinRoomButtonClicked);
            _leaveRoomButton.onClick.AddListener(OnLeaveRoomButtonClicked);

            _createOrJoinRoomController.OnCreateOrJoinRoom += OnConnectedToRoom;
            _createOrJoinRoomController.OnExitRoom += OnDisconnectedFromRoom;
        }

        private void OnDisable()
        {
            _fastPlayButton.onClick.RemoveListener(OnFastPlayButtonClicked);
            _createRoomButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
            _joinRoomButton.onClick.RemoveListener(OnJoinRoomButtonClicked);
            _leaveRoomButton.onClick.RemoveListener(OnLeaveRoomButtonClicked);

            _createOrJoinRoomController.OnCreateOrJoinRoom -= OnConnectedToRoom;
            _createOrJoinRoomController.OnExitRoom -= OnDisconnectedFromRoom;
        }

        private void OnFastPlayButtonClicked()
        {
            _createOrJoinRoomController.CreateOrJoin();
        }

        private void OnCreateRoomButtonClicked()
        {
            _createOrJoinRoomController.CreateRoom();
            //_readyButton.gameObject.SetActive(true);
        }

        private void OnJoinRoomButtonClicked()
        {
            _roomInputFieldUI.Show();
        }

        private void OnLeaveRoomButtonClicked()
        {
            //_readyButton.gameObject.SetActive(false);
            _createOrJoinRoomController.LeaveRoom();
        }

        private void OnConnectedToRoom(PlayerRef playerRef, string roomName, GameMode gameMode)
        {
            _outRoomObject.SetActive(false);
            _inRoomObject.SetActive(true);

            _roomNameText.text = $"{roomName}";
        }

        private void OnDisconnectedFromRoom(PlayerRef playerRef)
        {
            _outRoomObject.SetActive(true);
            _inRoomObject.SetActive(false);
        }
    }
}
