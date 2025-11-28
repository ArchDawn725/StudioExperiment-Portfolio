using UnityEngine;

public class EmergencyNode : MonoBehaviour
{
    public int zone;
    public bool claimed;
    private void Start()
    {
        zone = transform.parent.parent.GetComponent<CityBlock>().zone;
        Controller.instance.e_Nodes.Add(this);
    }
}
