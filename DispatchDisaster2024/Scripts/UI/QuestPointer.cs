using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    [SerializeField] private Camera ui_Camera;
    private Vector3 tartgetPos;
    private RectTransform rectTransform;

    [SerializeField] private Transform rotateFrom;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float borderSize = -100;

    private void Awake()
    {
        tartgetPos = targetTransform.transform.position;
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 targetToScreenPoint = Camera.main.WorldToScreenPoint(tartgetPos);
        if (!isOffScreen(targetToScreenPoint)) { transform.GetChild(0).gameObject.SetActive(false); return; }
        else 
        {
            transform.GetChild(0).gameObject.SetActive(true);
            RotatePointer();

            Vector3 cappedTargetScreenPos = targetToScreenPoint;
            if (cappedTargetScreenPos.x <= 0) cappedTargetScreenPos.x = borderSize;
            if (cappedTargetScreenPos.x >= Screen.width - 0) cappedTargetScreenPos.x = Screen.width - borderSize;
            if (cappedTargetScreenPos.y <= 0) cappedTargetScreenPos.y = borderSize;
            if (cappedTargetScreenPos.y >= Screen.height - 0) cappedTargetScreenPos.y = Screen.height - borderSize;

            Vector3 pointerWorldPos = ui_Camera.ScreenToWorldPoint(cappedTargetScreenPos);
            rectTransform.position = pointerWorldPos;
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0);
        }
    }
    private void RotatePointer()
    {
        Vector3 toPos = tartgetPos;
        Vector3 fromPos = rotateFrom.position;
        fromPos.z = 0f;
        Vector3 dir = (toPos - fromPos).normalized;

        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) { n += 360; }

        rectTransform.localEulerAngles = new Vector3(0, 0, n);
    }

    private bool isOffScreen(Vector3 targetToScreenPoint)
    {

        return
            targetToScreenPoint.x <= 0 ||
            targetToScreenPoint.y <= 0 ||
            targetToScreenPoint.x >= Screen.width - 0 ||
            targetToScreenPoint.y >= Screen.height - 0;
    }
}
