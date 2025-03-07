using System;
using UnityEngine;

namespace Project.Scripts
{
    public class DropdownTooltipText : MonoBehaviour
    {
        [SerializeField]
        public string tooltipText = "";

        public string GetTooltipText()
        {
            return tooltipText;
        }
    }
}
