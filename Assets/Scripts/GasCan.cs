using Echobay.FightSystem;
using Echobay.GridSystem;
using Echobay.InteractSystem;
using UnityEngine;

namespace Echobay
{
    public class GasCan : OutlineObject, IDamageable, ICellOccupant
    {
        public HealthSystem Health { get; private set; } = new HealthSystem(100);
        public Vector3 Position => transform.position;

        public GridCell CurrentCell { get; set; }

        private void OnEnable()
        {
            Health.OnTakedDamage += HandleTakedDamage;
            Health.OnDead += HandleDead;
        }

        private void OnDisable()
        {
            Health.OnTakedDamage -= HandleTakedDamage;
            Health.OnDead -= HandleDead;
        }

        private void HandleTakedDamage(int damage)
        {
            Debug.Log($"GasCan took {damage} damage. Remaining health: {Health.Value}");
        }

        private void HandleDead()
        {
            Destroy(gameObject);
            Debug.Log("GasCan is dead.");
        }

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}
