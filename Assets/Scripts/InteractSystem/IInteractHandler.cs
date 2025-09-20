using System;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public interface IInteractHandler
    {
        public event Action<IInteractable> OnPointEnter;
        public event Action<IInteractable> OnPointExit;
        public event Action<IInteractable> OnInteract;

        public bool IsPointerOverUI { get; }
    }
}