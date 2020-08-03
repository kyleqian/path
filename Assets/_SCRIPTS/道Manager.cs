using PathCreation;
using UnityEngine;

public class 道Manager : MonoBehaviour
{
    public bool debugMode;

#pragma warning disable 0649
    [SerializeField] private Camera camera_;
    [SerializeField] private PathCreator pathCreator_;
    [SerializeField] private GameObject touchDebug_;
#pragma warning restore 0649

    private Color initialBackgroundColor;
    private Color failureBackgroundColor = new Color(0.2f, 0.2f, 0.2f);

    private void Start()
    {
        initialBackgroundColor = camera_.backgroundColor;
    }

    private void Update()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (Input.touchCount < 1)
        {
            touchDebug_.SetActive(false);
            return;
        }
        Vector2 touchScreenPosition = Input.GetTouch(0).position;
#else
        if (!Input.GetMouseButton(0))
        {
            touchDebug_.SetActive(false);
            return;
        }
        Vector2 touchScreenPosition = Input.mousePosition;
#endif

        Vector3 touchPoint = camera_.ScreenToWorldPoint(touchScreenPosition);
        touchPoint.z = 0;
        touchDebug_.SetActive(debugMode);
        touchDebug_.transform.position = touchPoint;
        Vector3 pathclosestPoint = pathCreator_.path.GetClosestPointOnPath(touchPoint);

        if (Vector3.Distance(touchPoint, pathclosestPoint) > 0.5f)
        {
            camera_.backgroundColor = failureBackgroundColor;
        }
        else
        {
            camera_.backgroundColor = initialBackgroundColor;
        }
    }
}
