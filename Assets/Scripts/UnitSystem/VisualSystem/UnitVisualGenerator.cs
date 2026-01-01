using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Echobay.UnitSystem.VisualSystem
{
    public class UnitVisualGenerator : MonoBehaviour
    {
        [SerializeField] private VisualDatabase _database;
        [SerializeField] private Unit _unit;

        [Button]
        public void Generate()
        {
            ClearVisualElements();

            var profile = _unit.GetData<UnitData>().VisualProfile;

            foreach (var rule in profile.Slots)
            {
                if (!rule.Enabled)
                    continue;

                VisualElementData element = rule.Randomize
                    ? PickRandom(rule.Slot, profile.Gender)
                    : rule.FixedElement;

                if (element == null)
                    continue;

                Object.Instantiate(element.Prefab, _unit.VisualRoot);
            }
        }

        private VisualElementData PickRandom(
            VisualSlot slot,
            Gender gender)
        {
            var pool = _database.Get(slot, gender).ToList();
            return pool.Count == 0
                ? null
                : pool[Random.Range(0, pool.Count)];
        }

        private void ClearVisualElements()
        {
            Transform[] elements = _unit.VisualRoot.GetComponentsInChildren<Transform>();

            foreach (var element in elements)
            {
                if (element == _unit.VisualRoot)
                    continue;
                Destroy(element.gameObject);
            }
        }
    }
}
