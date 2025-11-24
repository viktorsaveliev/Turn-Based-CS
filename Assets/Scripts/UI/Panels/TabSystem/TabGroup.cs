using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay.UISystem.TabSystem
{
    public class TabGroup : MonoBehaviour
    {
        public bool IsActive { get; private set; } = true;

        [SerializeField] private TabButton[] _tabs;

        private TabButton _selectedTab;

        private void OnEnable()
        {
            foreach (TabButton tab in _tabs)
            {
                tab.OnClicked += OnTabClicked;
            }
        }

        private void OnDisable()
        {
            foreach (TabButton tab in _tabs)
            {
                tab.OnClicked -= OnTabClicked;
            }
        }

        public void SetActive(bool active)
        {
            IsActive = active;

            if (!IsActive && _selectedTab != null)
            {
                _selectedTab.DeselectTab();
                _selectedTab = null;
            }
        }

        private void OnTabClicked(TabButton tab)
        {
            //if (_selectedTab == tab) return;

            if (_selectedTab != null)
            {
                _selectedTab.DeselectTab();
            }

            tab.SelectTab();
            _selectedTab = tab;
        }

        [Button]
        private void GetAllTabsInChildren()
        {
            _tabs = GetComponentsInChildren<TabButton>();
        }
    }
}
