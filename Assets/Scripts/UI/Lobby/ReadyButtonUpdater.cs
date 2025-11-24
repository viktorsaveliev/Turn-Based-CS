using Echobay.NetworkSystem.Lobby;
using Echobay.NetworkSystem.Lobby.Rooms;
using Fusion;
using System.Collections;
using UnityEngine;

namespace Echobay.Lobby
{
    public class ReadyButtonUpdater : MonoBehaviour
    {
        [SerializeField] private ReadyButton _readyButton;
        [SerializeField] private NetworkRoomMediator _networkRoomMediator;

        private NetworkRoomPlayer _player;

        private void OnEnable()
        {
            StartCoroutine(WaitForNetworkInitialized());
        }

        private void OnDisable()
        {
            if (_player != null)
            {
                _player.OnPlayerReadyChanged -= UpdateButton;
            }
        }

        private IEnumerator WaitForNetworkInitialized()
        {
            while (_player == null)
            {
                NetworkRunner networkRunner = _networkRoomMediator.Runner;
                NetworkObject networkObject = networkRunner.GetPlayerObject(networkRunner.LocalPlayer);

                if (networkObject != null)
                {
                    _player = networkObject.GetComponent<NetworkRoomPlayer>();

                    if (_player != null)
                    {
                        _player.OnPlayerReadyChanged += UpdateButton;
                        print("Finded");
                        break;
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void UpdateButton(NetworkRoomPlayer roomPlayer)
        {
            _readyButton.SetState(roomPlayer.IsReady);
        }
    }
}
