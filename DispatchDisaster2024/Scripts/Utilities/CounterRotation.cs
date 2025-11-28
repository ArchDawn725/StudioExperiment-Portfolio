using UnityEngine;

public class CounterRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 newRotation = new Vector3 (0,0,0);
        if (transform.parent.rotation.z == 1) { newRotation.z = 180; }
        else if (transform.parent.rotation.z == 0) { newRotation.z = 0; }
        else if (transform.parent.rotation.z < -0.5) { newRotation.z = 90; }
        else { newRotation.z = -90; }


        GetComponent<RectTransform>().localEulerAngles = newRotation;
    }
}
