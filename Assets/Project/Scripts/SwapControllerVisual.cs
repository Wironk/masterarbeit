using System;
using UnityEngine;
using UnityEngine.XR;

namespace Project.Scripts
{
    public class SwapControllerVisual : MonoBehaviour
    {
        [SerializeField]
        private Transform controllerRight;
        [SerializeField]
        private Transform controllerLeft;
        
        [SerializeField]
        private GameObject markerRightVisual;
        [SerializeField]
        private GameObject markerLeftVisual;

        private GameObject _controllerRightVisual;
        private GameObject _controllerLeftVisual;

        private ControllerPhysics _markerScriptRight;
        private ControllerPhysics _markerScriptLeft;
        
        private void Start()
        {
            _markerScriptRight =
                markerRightVisual.transform.parent.GetComponent<ControllerPhysics>();
            _markerScriptLeft =
                markerLeftVisual.transform.parent.GetComponent<ControllerPhysics>();

            _controllerRightVisual = controllerRight.GetChild(3)
                .GetChild(0).gameObject;
            _controllerLeftVisual = controllerLeft.GetChild(3)
                .GetChild(0).gameObject;
        }

        private void Update()
        {
            CheckRotation(controllerRight, _controllerRightVisual,
                markerRightVisual, _markerScriptRight);
            CheckRotation(controllerLeft, _controllerLeftVisual,
                markerLeftVisual, _markerScriptLeft);
        }

        private void CheckRotation(Transform controller,
            GameObject controllerVisual, GameObject markerVisual,
            ControllerPhysics markerScript)
        {
            double absoluteY = Math.Abs(controller.rotation.eulerAngles.y -
                                        transform.rotation.eulerAngles.y);
            double absoluteX = Math.Abs(controller.rotation.eulerAngles.y -
                                        transform.rotation.eulerAngles.y);

            if ((absoluteY < 270 && absoluteY > 90) ||
                (absoluteX < 270 && absoluteX > 90) ||
                markerScript.rotationLocked)
            {
                controllerVisual.SetActive(false);
                markerVisual.SetActive(true);
            }
            else
            {
                controllerVisual.SetActive(true);
                markerVisual.SetActive(false);
            }
        }
    }
}