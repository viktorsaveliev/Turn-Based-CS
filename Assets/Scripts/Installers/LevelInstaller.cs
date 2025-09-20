using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.FightSystem.StatusEffects;
using Echobay.GridSystem;
using Echobay.InputSystem;
using Echobay.PlayerSystem;
using Echobay.MatchSystem.TurnSystem;
using UnityEngine;
using Zenject;
using Echobay.MatchSystem;

namespace Echobay
{
    public class LevelInstaller : MonoInstaller
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

        private InputData _inputData;
        private ActionController _actionController;

        private MatchController _matchController;

        private void Awake()
        {
            Resolve();

            _inputData.Init();
            _actionController.Initialize();

            
        }

        private void OnDestroy()
        {
            _actionController.Dispose();
        }

        public override void InstallBindings()
        {
            Container.Bind<GameplayData>().FromInstance(_gameplayData).AsSingle();
            Container.Bind<InputData>().FromNew().AsSingle();

            Container.Bind<PlayerFactory>().FromNew().AsSingle();
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

            Container.Bind<GridPathView>().FromInstance(_gridPathView).AsSingle();
        }

        private void Resolve()
        {
            _matchController = Container.Resolve<MatchController>();
            _inputData = Container.Resolve<InputData>();
            _actionController = Container.Resolve<ActionController>();
        }
    }
}
