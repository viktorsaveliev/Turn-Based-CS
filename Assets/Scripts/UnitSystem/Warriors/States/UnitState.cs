
namespace Echobay.UnitSystem
{
    public abstract class UnitState : State
    {
        protected readonly IUnit Unit;

        public UnitState(IUnit unit)
        {
            Unit = unit;
        }
    }
}