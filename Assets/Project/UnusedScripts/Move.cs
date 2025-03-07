using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Move : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
   
        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.isValid)
            {
                var currentRaycastPosition = eventData.pointerCurrentRaycast.worldPosition;
                transform.position = currentRaycastPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
