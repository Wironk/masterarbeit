using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public class DropdownHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject modelParameterParent;
        private GameObject _currentModelParams;
        private Generate _generate;
        private TMP_Dropdown _dropdown;
        
        private void Start()
        {
            _dropdown = transform.GetChild(0).GetComponent<TMP_Dropdown>();
            
            for (int i = 0; i < modelParameterParent.transform.childCount; i++)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData {text=modelParameterParent.transform.GetChild(i).gameObject.name});
            }
            _dropdown.RefreshShownValue();
            
            OnDropdownChanged();
        }

        public void OnDropdownChanged()
        {
            for (int i = 0; i < modelParameterParent.transform.childCount; i++)
            {
                modelParameterParent.transform.GetChild(i).gameObject.SetActive(i == _dropdown.value);
                
                if (i == _dropdown.value)
                {
                    _currentModelParams = modelParameterParent.transform.GetChild(i).gameObject;
                }
            }
        }

        private void FindModelScript()
        {
            _generate = _currentModelParams.GetComponent<Generate>();
            _generate.StartGenerate();
        }

        public GameObject GetModelParameterParent()
        {
            return modelParameterParent;
        }
    }
}
