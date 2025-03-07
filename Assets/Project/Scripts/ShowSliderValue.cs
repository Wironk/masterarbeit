using TMPro;
using UnityEngine;

namespace Project.Scripts
{
    public class ShowSliderValue : MonoBehaviour
    {
        public float valueMult;
        public string format;
        private float _currValue;
    
        public void OnValueChanged(float newValue)
        {
            _currValue = newValue * valueMult;
            GetComponent<TextMeshProUGUI>().text = _currValue.ToString(format);
        }

        public float GetValue()
        {
            return _currValue;
        }
    }
}
