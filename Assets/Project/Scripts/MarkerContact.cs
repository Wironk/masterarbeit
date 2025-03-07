using UnityEngine;

namespace Project.Scripts
{
    public class MarkerContact : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Marker")) return;
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.CompareTag("Color"))
                {
                    var color = contact.thisCollider.gameObject;
                    var marker = collision.gameObject;

                    var material = color.GetComponent<Renderer>().material;
                    marker.GetComponent<Transform>().GetChild(0).GetComponent<WhiteboardMarker>().ChangeMaterial(material);

                    break;
                }
            }
        }
    }
}
