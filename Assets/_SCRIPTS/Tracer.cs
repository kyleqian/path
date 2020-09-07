using PathCreation;
using UnityEngine;

public class Tracer : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Settings")]
    [SerializeField] float BASE_SPEED = 8;

    [Header("Runtime References")]
    [SerializeField] private PathCreator PATH_CREATOR;
    [SerializeField] private TrailRenderer TRAIL_RENDERER;
#pragma warning restore 0649

    private float distanceTraveled_ = 0;

    private void Update()
    {
        float pathTime = PATH_CREATOR.path.GetClosestTimeOnPath(transform.position);
        distanceTraveled_ += BASE_SPEED * Time.deltaTime;
        transform.position = PATH_CREATOR.path.GetPointAtDistance(distanceTraveled_, EndOfPathInstruction.Stop);
        transform.rotation = PATH_CREATOR.path.GetRotationAtDistance(distanceTraveled_, EndOfPathInstruction.Stop);
    }
}
