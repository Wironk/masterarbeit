using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts
{
    public class DropdownTooltipEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private GameObject _modelParameterParent;
        private DropdownHandler _dropdownHandler;
        private string _tipToShow = "";
        private const float TimeToWait = 0.5f;

        private void Start()
        {
            _dropdownHandler = GameObject.Find("ChooseModel Dropdown").GetComponent<DropdownHandler>();
            _modelParameterParent = _dropdownHandler.GetModelParameterParent();
            string modelParamsName = transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
            Transform modelParams = _modelParameterParent.transform.Find(modelParamsName);
            if (modelParams is not null) 
            {
                _tipToShow = modelParams.GetComponent<DropdownTooltipText>().GetTooltipText();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            DropdownTooltip.OnLoseFocus();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StopAllCoroutines();
            DropdownTooltip.OnLoseFocus();
        }

        private void ShowMessage()
        {
            DropdownTooltip.OnHover(_tipToShow, transform.position);
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(TimeToWait);
            ShowMessage();
        }
    }
}
