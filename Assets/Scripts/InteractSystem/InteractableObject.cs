using System;
using UnityEngine;

namespace Echobay.InteractSystem
{
    public abstract class InteractableObject : MonoBehaviour, IInteractable
    {
        public event Action<InteractableObject> OnInteracted;

        [SerializeField] private ObjectData _data;

        public bool IsInteractable { get; set; }

        private void OnValidate()
        {
            LayerMask interactableLayer = LayerMask.NameToLayer("Interactable");
            if (gameObject.layer != interactableLayer)
            {
                gameObject.layer = interactableLayer;
            }
        }

        protected virtual void Awake()
        {
            IsInteractable = true;
        }

        //public abstract void OnSelected();
        //public abstract void OnUnselected();
        public abstract void OnPointEnter();

        public abstract void OnPointExit();

        public virtual void Interact()
        {
            OnInteracted?.Invoke(this);
        }

        public T GetData<T>() where T : ObjectData
        {
            return _data as T;
        }

        protected void SetData(ObjectData data)
        {
            _data = data;
        }
    }
}