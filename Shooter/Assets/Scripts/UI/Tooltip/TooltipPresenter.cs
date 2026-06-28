using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public sealed class TooltipPresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Text _text;

        public void Initialize(RectTransform panel, Text text)
        {
            _panel = panel;
            _text = text;
            Hide();
        }


        public void Show(string text, Vector2 screenPosition)
        {
            if (_panel == null || _text == null)
            {
                return;
            }

            _text.text = text;

            _panel.gameObject.SetActive(true);
            _panel.position = screenPosition + new Vector2(18f, -18f);
        }

        public void Hide()
        {
            if (_panel != null)
            {
                _panel.gameObject.SetActive(false);
            }
        }
    }
}
