using Echobay.ActionContext;
using Echobay.NetworkSystem.Match;
using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    [RequireComponent(typeof(TMP_Text))]
    public class RejectText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, Range(1, 10)] private int _showTimeInSeconds = 5;

        private ActionController _actionController;
        private Coroutine _coroutine;

        [Inject]
        public void Construct(ActionController actionController)
        {
            _actionController = actionController;
        }

        private void OnValidate()
        {
            if (_text == null)
            {
                _text = GetComponent<TMP_Text>();
            }
        }

        private void Awake()
        {
            _actionController.OnActionRejected += OnActionRejected;
            Hide();
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _text.text = string.Empty;
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _actionController.OnActionRejected -= OnActionRejected;
        }

        private void OnActionRejected(NetworkRejectContext context)
        {
            Show(context.ReasonText.ToString());
        }

        private void Show(string text)
        {
            _text.text = text;
            _text.gameObject.SetActive(true);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(HideWithDelay());
        }

        private void Hide()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _text.gameObject.SetActive(false);
        }

        private IEnumerator HideWithDelay()
        {
            yield return new WaitForSeconds(_showTimeInSeconds);
            Hide();
        }
    }
}
