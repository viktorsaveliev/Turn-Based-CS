using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.FightSystem.StatusEffects;
using Echobay.GridSystem;
using Echobay.InputSystem;
using Echobay.MatchSystem;
using Echobay.MatchSystem.TurnSystem;
using Echobay.NetworkSystem;
using Echobay.NetworkSystem.Match;
using Echobay.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Echobay.Installers
{
    public class MatchInstaller : MonoInstaller
    {
        [SerializeField] private GameplayData _gameplayData;
        [SerializeField] private CardController _cardController;
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private GridPathView _gridPathView;
        [SerializeField] private MouseRaycaster _interactHandler;
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private ObjectInteractionViewData _objectInteractionView;
        [SerializeField] private CancellationTokenObject _cancellationTokenObject;
        [SerializeField] private UnitSpawner _unitSpawner;

        [Header("Networking")]
        [SerializeField] private NetworkMatchMediator _networkMatchMediator;
        [SerializeField] private NetworkMatchController _networkMatchController;
        [SerializeField] private NetworkTurnController _networkTurnController;

        private InputData _inputData;

        private ActionController _actionController;
        private ActionContextLinks _actionContext;

        private MatchController _matchController;
        private TurnController _turnController;

        private NetworkRunnerProvider _networkProvider;
        private GridTurnActivator _gridTurnActivator;

        private void Awake()
        {
            Resolve();

            _inputData.Init();
            _actionController.Init();
            _gridTurnActivator.Init();

            if (_networkProvider.Runner != null && _networkProvider.Runner.IsServer)
            {
                _matchController.Init();
                _turnController.Init();
            }
        }

        private void OnDestroy()
        {
            _gridTurnActivator.Dispose();
            _actionController.Dispose();
            _turnController.Dispose();
            _actionContext.Dispose();
        }

        public override void InstallBindings()
        {
            Container.Bind<GameplayData>().FromInstance(_gameplayData).AsSingle();
            Container.Bind<InputData>().FromNew().AsSingle();

            Container.Bind<MatchPlayerFactory>().FromNew().AsSingle();
            Container.Bind<UnitFactory>().FromNew().AsSingle();

            Container.Bind<IInteractHandler>().FromInstance(_interactHandler).AsSingle();
            Container.Bind<CameraShake>().FromInstance(_cameraShake).AsSingle();
            Container.Bind<ObjectInteractionViewData>().FromInstance(_objectInteractionView).AsSingle();
            Container.Bind<CardController>().FromInstance(_cardController).AsSingle();
            Container.Bind<CancellationTokenObject>().FromInstance(_cancellationTokenObject).AsSingle();

            Container.Bind<ActionContextLinks>().FromNew().AsSingle();

            Container.Bind<ActionController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<TurnController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<StatusEffectController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GridManager>().FromInstance(_gridManager).AsSingle();
            Container.BindInterfacesAndSelfTo<UnitSpawner>().FromInstance(_unitSpawner).AsSingle();

            // === Network ===
            Container.Bind<ServerActionHandler>().FromNew().AsSingle();
            Container.Bind<ClientActionExecutor>().FromNew().AsSingle();
            Container.Bind<GridTurnActivator>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<NetworkMatchMediator>().FromInstance(_networkMatchMediator).AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkMatchController>().FromInstance(_networkMatchController).AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkTurnController>().FromInstance(_networkTurnController).AsSingle();

            Container.Bind<GridPathView>().FromInstance(_gridPathView).AsSingle();
        }

        private void Resolve()
        {
            _turnController = Container.Resolve<TurnController>();
            _matchController = Container.Resolve<MatchController>();
            _inputData = Container.Resolve<InputData>();

            _actionController = Container.Resolve<ActionController>();
            _actionContext = Container.Resolve<ActionContextLinks>();

            _networkProvider = Container.Resolve<NetworkRunnerProvider>();

            _gridTurnActivator = Container.Resolve<GridTurnActivator>();
        }
    }
}
