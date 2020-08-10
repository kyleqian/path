using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 道Manager : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Settings")]
    [SerializeField] bool DEBUG_MODE;
    [SerializeField] float MAX_SECONDS_ALLOWED_OFF_PATH = 0.5f;
    [SerializeField] float MAX_DISTANCE_ALLOWED_FROM_PATH = 0.5f;

    [Header("Runtime References")]
    [SerializeField] private Camera CAMERA;
    [SerializeField] private PathCreator PATH_CREATOR;
    [SerializeField] private GameObject TOUCH_DEBUG;
#pragma warning restore 0649

#pragma warning disable 0414, IDE0052
    private bool touchBegan_ = false;
#pragma warning restore 0414, IDE0052
    private Color succeessBackGroundColor_ = new Color(0.416f, 0.921f, 0.506f);
    private Color failureBackgroundColor_ = new Color(0.921f, 0.506f, 0.416f);

    private bool canTouchDown_ = false;
    private bool isTouchDown_ = false;
    private float secondsSpentOffPath_ = 0;
    private int sceneFrameCount_ = 0;

    private void Update()
    {
        ++sceneFrameCount_;
        Vector3? touchPointOpt = GetTouchPoint();
        bool currTouchDown = touchPointOpt != null;

        if (!currTouchDown)
        {
            if (isTouchDown_)
            {
                ResetGame();
            }

            // Need one frame of not touching so that resets don't trigger more resets
            canTouchDown_ = true;

            TOUCH_DEBUG.SetActive(false);
            return;
        }

        if (!canTouchDown_)
        {
            TOUCH_DEBUG.SetActive(false);
            return;
        }

        isTouchDown_ = true;
        Vector3 touchPoint = (Vector3)touchPointOpt;
        TOUCH_DEBUG.SetActive(DEBUG_MODE);
        TOUCH_DEBUG.transform.position = touchPoint;
        Vector3 pathclosestPoint = PATH_CREATOR.path.GetClosestPointOnPath(touchPoint);

        if (Vector3.Distance(touchPoint, pathclosestPoint) < MAX_DISTANCE_ALLOWED_FROM_PATH)
        {
            CAMERA.backgroundColor = succeessBackGroundColor_;
        }
        else
        {
            CAMERA.backgroundColor = failureBackgroundColor_;
            secondsSpentOffPath_ += Time.deltaTime;
            if (secondsSpentOffPath_ >= MAX_SECONDS_ALLOWED_OFF_PATH)
            {
                ResetGame();
            }
        }
    }

    private void OnGUI()
    {
        if (DEBUG_MODE)
        {
            GUI.Label(new Rect(10, 10, 100, 20), sceneFrameCount_.ToString());
        }
    }

    private Vector3? GetTouchPoint()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (Input.touchCount != 1)
        {
            return null;
        }

        var touch = Input.GetTouch(0);

        // Weird edge case in iOS which is probably a bug tbh
        if (!touchBegan_ && touch.phase != TouchPhase.Began)
        {
            return null;
        }

        Vector2 touchScreenPosition = Input.GetTouch(0).position;
#else
        if (!Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            return null;
        }
        Vector2 touchScreenPosition = Input.mousePosition;
#endif

        touchBegan_ = true;
        Vector3 touchPoint = CAMERA.ScreenToWorldPoint(touchScreenPosition);
        touchPoint.z = 0;
        return touchPoint;
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
