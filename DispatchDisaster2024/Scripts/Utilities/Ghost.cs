using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private SpriteRenderer sprite;
    private void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Controller.instance.placingPrefab != null)
        {
            FollowMouseWithSnap();
            RayCaster();
            sprite.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                if (BuildRaycast()) { Controller.instance.Build(GetBuildPos()); }
            }
            return;
        }
        else 
        { 
            sprite.enabled = false;
            if (Input.GetMouseButtonDown(0)) { AttemptRaycast(); }
        }
    }

    private void FollowMouseWithSnap()
    {
        transform.position = PlacingRaycast();
    }

    private Vector3 SnapToGrid(Vector3 originalPosition)
    {
        // Round the position to the nearest multiple of 10
        float x = Mathf.Round(originalPosition.x / 100) * 10;
        float z = Mathf.Round(originalPosition.z / 100) * 10;
        float y = 0;

        return new Vector3(x, y, z);
    }
    private void RayCaster()
    {
        if (SentRaycast())
        {
            sprite.color = colors[0];
        }
        else
        {
            sprite.color = colors[1];
        }
    }
    private bool SentRaycast()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Buildable")) 
            {
                if (hit.collider.transform.parent.GetComponent<CityBlock>().emergencyNode.claimed) { return false; }
                return true; 
            }
            else { return false; }
        }
        else
        {
            return false;
        }

    }
    private Vector3 PlacingRaycast()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 placingPos = hit.collider.transform.position;
            placingPos.y += 1.1f;
            return placingPos;
        }
        else
        {
            return transform.position;
        }

    }

    private bool BuildRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Buildable")) { hit.collider.tag = "Untagged"; return true; }
            else { return false; }
        }
        else
        {
            return false;
        }
    }
    private Vector3 GetBuildPos()
    {
        return PlacingRaycast();
    }
    private void AttemptRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Controller.instance.selectedUnit == null)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    Controller.instance.NewSelectUnit(hit.collider.gameObject);
                    UIController.instance.SelectUnit(hit.collider.gameObject.GetComponent<Unit>());
                }
                else if (hit.collider.CompareTag("Building"))
                {
                    Controller.instance.selectedBuilding = hit.collider.gameObject;
                    UIController.instance.SelectedBuilding(hit.collider.gameObject.GetComponent<Building>());
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift)) { Controller.instance.MoveUnitTONewPos(hit.collider.transform.position, true); }
                else { Controller.instance.MoveUnitTONewPos(hit.collider.transform.position, false); }
            }

        }
    }
}
