using UnityEngine;

public class Transpatenter : MonoBehaviour
{
    public int entities;

    private Renderer rend;
    private BoxCollider collider;
    public void StartUp()
    {
        rend = transform.GetChild(transform.childCount - 1).GetChild(0).GetComponent<Renderer>();
        collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            entities++;
            rend.enabled = false;
            //collider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            entities--;
            if (entities <= 0)
            {
                rend.enabled = true;
                //collider.enabled = true;
            }
        }
    }
}
