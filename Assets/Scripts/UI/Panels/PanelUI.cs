using UnityEngine;

namespace Echobay.UISystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelUI : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup Group;

        private void OnValidate()
        {
            if (Group == null)
            {
                Group = GetComponent<CanvasGroup>();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            Group.interactable = true;
            Group.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            Group.interactable = false;
            Group.blocksRaycasts = false;

            gameObject.SetActive(false);
        }
    }
}