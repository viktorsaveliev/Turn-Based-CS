using Echobay.FightSystem;
using Echobay.FightSystem.DamageType;
using Echobay.GridSystem;
using Echobay.InteractSystem;
using UnityEngine;

namespace Echobay
{
    public class GasCan : DamageableObject, ICellOccupant
    {
        [field: SerializeField] public DamageModifiers DamageModifiers { get; private set; }

        public Vector3 Position => transform.position;
        public GridCell CurrentCell { get; set; }

        private void OnEnable()
        {
            OnTakedDamage += HandleTakedDamage;
            OnDead += HandleDead;
        }

        private void OnDisable()
        {
            OnTakedDamage -= HandleTakedDamage;
            OnDead -= HandleDead;
        }

        private void HandleTakedDamage(int damage)
        {
            Debug.Log($"GasCan took {damage} damage. Remaining health: {CurrentHealth}");
        }

        private void HandleDead()
        {
            Destroy(gameObject);
            Debug.Log("GasCan is dead.");
        }
    }
}
