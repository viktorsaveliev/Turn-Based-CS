using Echobay.UISystem;
using UnityEngine;

namespace Echobay.NetworkSystem.Lobby
{
    public class MatchStarter : MonoBehaviour
    {
        [SerializeField] private NetworkRoomMediator _roomMediator;
        [SerializeField] private StartMatchCountdownPanel _startMatchCountdownPanel;

        private void OnEnable()
        {
            _roomMediator.OnRoomJoined += OnRoomJoined;
        }

        private void OnDisable()
        {
            _roomMediator.OnRoomJoined -= OnRoomJoined;
            _roomMediator.RoomController.OnAllPlayersReady -= StartMatch;
            _startMatchCountdownPanel.OnCountdownEnded -= LoadLevel;
        }

        private void OnRoomJoined()
        {
            print("OnRoomJoined...");
            _roomMediator.RoomController.OnAllPlayersReady += StartMatch;
        }

        private void StartMatch()
        {
            print("StartMatch...");
            _startMatchCountdownPanel.Show();
            _startMatchCountdownPanel.OnCountdownEnded += LoadLevel;
        }

        private void LoadLevel()
        {
            _startMatchCountdownPanel.OnCountdownEnded -= LoadLevel;

            if (!_roomMediator.Runner.IsServer) return;

            _roomMediator.Runner.LoadScene("Level", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
