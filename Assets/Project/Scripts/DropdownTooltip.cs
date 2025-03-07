using System;
using TMPro;
using UnityEngine;

namespace Project.Scripts
{
    public class DropdownTooltip : MonoBehaviour
    {
        public TextMeshProUGUI _tooltipText;
        public RectTransform _tooltipWindow;

        public static Action<string, Vector3> OnHover;
        public static Action OnLoseFocus;

        private void OnEnable()
        {
            OnHover += ShowTooltip;
            OnLoseFocus += HideTooltip;
        }

        private void OnDisable()
        {
            OnHover -= ShowTooltip;
            OnLoseFocus -= HideTooltip;
        }

        void Start()
        {
            HideTooltip();
        }

        private void ShowTooltip(string text, Vector3 pos)
        {
            _tooltipText.text = text;
            _tooltipWindow.sizeDelta = new Vector2(200, _tooltipText.preferredHeight < 25 ? 25 : _tooltipText.preferredHeight);
        
            _tooltipWindow.gameObject.SetActive(true);
            _tooltipWindow.transform.position = new Vector3(pos.x, pos.y, pos.z + 0.7f);
        }

        private void HideTooltip()
        {
            _tooltipText.text = default;
            _tooltipWindow.gameObject.SetActive(false);
        }
    }
}
