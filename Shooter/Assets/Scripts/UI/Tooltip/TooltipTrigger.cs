using UnityEngine;
using UnityEngine.EventSystems;

namespace Shooter.UI
{
    public sealed class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField] private TooltipPresenter _presenter;
        [SerializeField, TextArea] private string _text;
        private bool _isPointerInside;

        public void Initialize(TooltipPresenter presenter, string text)
        {
            _presenter = presenter;
            _text = text;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerInside = true;
            _presenter?.Show(_text, eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerInside = false;
            _presenter?.Hide();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_isPointerInside)
            {
                _presenter?.Show(_text, eventData.position);
            }
        }
    }
}
