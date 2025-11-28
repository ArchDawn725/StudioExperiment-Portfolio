using UnityEngine;

public class Building : MonoBehaviour
{
    public string myName;
    public int type;
    private BuildingSO buildingSO;
    public void StartUp(BuildingSO myBuildingSO)
    {
        type = myBuildingSO.type;
        name = myBuildingSO.name;

        GameObject spawnedObject = Instantiate(myBuildingSO.BuildingVisual, transform.position, Quaternion.identity, transform);
        transform.GetComponent<Transpatenter>().StartUp();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>() != null)
        {
            if (other.GetComponent<Unit>().type == type)
            {
                other.GetComponent<Unit>().AtDestination(this);
                other.GetComponent<Unit>().stationed = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Unit>() != null)
        {
            if (other.GetComponent<Unit>().type == type)
            {
                other.GetComponent<Unit>().stationed = false;
            }
        }
    }
}
