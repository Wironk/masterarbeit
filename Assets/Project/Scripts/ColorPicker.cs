using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class ColorPicker : MonoBehaviour
    {
        public Texture2D _colorwheelTexture;
        private RectTransform _colorwheel;
        private Image blackwheel;
        private RectTransform _handle;
        private Image _handleColor;
        private Renderer _preview;
        
        private GameObject _targetColorObj;

        private Color _currValue, _currColor;
    
        void Start()
        {
            _colorwheel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            blackwheel = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            _handle = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
            _handleColor = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            _preview = transform.GetChild(0).GetChild(2).GetComponent<Renderer>();
            
            _currValue = blackwheel.color;
            OnValueChanged(0);
        }

        public void OnValueChanged(float newValue)
        {
            _currValue.a = newValue;
            blackwheel.color = _currValue;
        
            UpdateColor();
        }
        
        public void PickColor(BaseEventData data)
        {
            PointerEventData pointer = data as PointerEventData;
        
            Vector3 extractedPoint = Vector3.zero;
            extractedPoint = pointer.pointerPressRaycast.worldPosition;
        
            _handle.position = extractedPoint;

            if (!(Math.Pow(_handle.localPosition.x, 2) + Math.Pow(_handle.localPosition.y, 2) < Math.Pow(48, 2)))
            {
                Vector3 vector = _handle.localPosition;
                vector -= Vector3.zero;
                vector.Normalize();
                _handle.localPosition = vector * 48;
            }
        
            UpdateColor();
        }

        public void UpdateColor()
        {
            int realXPos = (int) (_handle.localPosition.x + _colorwheel.rect.width / 2);
            int realYPos = (int) (_handle.localPosition.y + _colorwheel.rect.height / 2);
            
            Color pickedColor = _colorwheelTexture.GetPixel(
                (int)(realXPos * (_colorwheelTexture.width / _colorwheel.rect.width)),
                (int)(realYPos * (_colorwheelTexture.height / _colorwheel.rect.height)));
        
            _currColor = pickedColor;
        
            float h, s;
            Color.RGBToHSV(_currColor, out h, out s, out _);
            _currColor = Color.HSVToRGB(h, s, 1 - _currValue.a);
        
            _preview.material.color = _currColor;
            _handleColor.color = pickedColor;
        }
        
        public void Call(GameObject caller)
        {
            _targetColorObj = caller;
        }
        
        public void SetColor()
        {
            _targetColorObj.GetComponent<Renderer>().material.color = _currColor;
            gameObject.SetActive(false);
        }
    }
}
