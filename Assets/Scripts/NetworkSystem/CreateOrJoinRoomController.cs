using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Zenject;
using System.Collections.Generic;
using Echobay;

namespace Networking
{
    public class CreateOrJoinRoomController : MonoBehaviour
    {
        public event Action<NetworkRunner> OnNetworkRunnerCreated;
        public event Action<NetworkRunner> OnNetworkRunnerDestroyed;

        public event Action<PlayerRef, string, GameMode> OnCreateOrJoinRoom;
        public event Action<PlayerRef> OnExitRoom;

        [SerializeField] private NetworkRunner _runnerPrefab;

        private NetworkSceneManagerDefault _sceneManager;
        private NetworkRunner _runner;
        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        public void CreateRoom()
        {
            string sessionName = GenerateRandomSessionName(4);
            StartSimulation(GameMode.Host, sessionName);
        }

        public void JoinRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("Room name cannot be empty!");
                return;
            }

            StartSimulation(GameMode.AutoHostOrClient, roomName);
        }

        public void LeaveRoom()
        {
            if (_runner != null && _runner.IsRunning)
            {
                OnNetworkRunnerDestroyed?.Invoke(_runner);
                Debug.Log("Leaving the room...");
                _runner.Shutdown();

                OnExitRoom?.Invoke(_runner.LocalPlayer);
            }
            else
            {
                Debug.LogWarning("No active session to leave.");
            }
        }

        public async void CreateOrJoin()
        {
            EnsureNetworkRunner();

            _runner.ProvideInput = true;

            StartGameArgs startGameArgs = GetPublicSettings();
            var result = await _runner.StartGame(startGameArgs);

            if (result.Ok)
            {
                Debug.Log($"Simulation started as {GameMode.Shared} in public room");
                OnCreateOrJoinRoom?.Invoke(_runner.LocalPlayer, "", GameMode.Shared);
            }
            else
            {
                Debug.LogError($"Failed to start simulation. Reason: {result.ShutdownReason}");
            }
        }

        private async void StartSimulation(GameMode gameMode, string sessionName)
        {
            EnsureNetworkRunner();

            _runner.ProvideInput = true;

            StartGameArgs startGameArgs = gameMode == GameMode.Host ? GetHostSettings(sessionName) : GetClientSettings(sessionName);
            var result = await _runner.StartGame(startGameArgs);

            if (result.Ok)
            {
                Debug.Log($"Simulation started as {gameMode} in room: {sessionName}");
                OnCreateOrJoinRoom?.Invoke(_runner.LocalPlayer, sessionName, gameMode);
            }
            else
            {
                Debug.LogError($"Failed to start simulation. Reason: {result.ShutdownReason}");
            }
        }

        private void EnsureNetworkRunner()
        {
            if (_runner == null)
            {
                _runner = _container.InstantiatePrefabForComponent<NetworkRunner>(_runnerPrefab);
                _sceneManager = _runner.GetComponent<NetworkSceneManagerDefault>();

                OnNetworkRunnerCreated?.Invoke(_runner);
            }
        }

        private SceneRef GetScene()
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            return SceneRef.FromIndex(buildIndex);
        }

        private StartGameArgs GetPublicSettings()
        {
            Dictionary<string, SessionProperty> properties = new()
            {
                [StringBus.SETTING_ROOM_BOTS] = false,
                [StringBus.SETTING_ROOM_MATCH_LENGTH] = 3,
                [StringBus.SETTING_ROOM_PRIVATE_ROOM] = false,
                [StringBus.SETTING_ROOM_LOCKED] = false
            };

            StartGameArgs gameArgs = new()
            {
                GameMode = GameMode.Client,
                SceneManager = _sceneManager,
                Scene = GetScene(),
                PlayerCount = 6,
                SessionProperties = properties
            };

            return gameArgs;
        }

        private StartGameArgs GetHostSettings(string sessionName)
        {
            Dictionary<string, SessionProperty> properties = new()
            {
                [StringBus.SETTING_ROOM_BOTS] = false,
                [StringBus.SETTING_ROOM_MATCH_LENGTH] = 3,
                [StringBus.SETTING_ROOM_PRIVATE_ROOM] = true,
                [StringBus.SETTING_ROOM_LOCKED] = false
            };

            StartGameArgs gameArgs = new()
            {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
                SceneManager = _sceneManager,
                Scene = GetScene(),
                PlayerCount = 6,
                SessionProperties = properties
            };

            return gameArgs;
        }

        private StartGameArgs GetClientSettings(string sessionName)
        {
            Dictionary<string, SessionProperty> properties = new()
            {
                [StringBus.SETTING_ROOM_LOCKED] = false
            };

            StartGameArgs gameArgs = new()
            {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
                SceneManager = _sceneManager,
                Scene = GetScene(),
                PlayerCount = 2,
                SessionProperties = properties
            };

            return gameArgs;
        }

        private string GenerateRandomSessionName(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(0, length).Select(_ => chars[UnityEngine.Random.Range(0, chars.Length)]).ToArray());
        }
    }
}