using Echobay.CardSystem;
using Echobay.NetworkSystem;
using Echobay.UnitSystem;
using UnityEngine;
using Zenject;

namespace Echobay.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UnitsDatabase _unitsDatabase;
        [SerializeField] private CardsDatabase _cardsDatabase;

        /*private void Awake()
        {
            
        }*/

        public override void InstallBindings()
        {
            Container.Bind<UnitsDatabase>().FromInstance(_unitsDatabase).AsSingle();
            Container.Bind<CardsDatabase>().FromInstance(_cardsDatabase).AsSingle();
            Container.Bind<NetworkRunnerProvider>().FromNew().AsSingle();
        }

        private void Resolve()
        {

        }
    }
}
