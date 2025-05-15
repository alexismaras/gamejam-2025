using UnityEngine;

public class WallDetector : MonoBehaviour
{
    // private float _maxDetectionHeight = 10f;
    // private float _detectionOffsetForward = 0.8f;
    private float _wallDetectionRangeForward = 1.0f;
    private float _detectWallEndOffsetY = 1.7f;
    private float _detectWallHeightOffsetY = 10f;
    private float _detectWallHeightOffsetX = 0.7f;
    

    [SerializeField] private LayerMask _climbableLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RaycastHit DetectWall()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y + _detectWallEndOffsetY, transform.position.z);
        if (Physics.Raycast(raycastStart, transform.forward, out hit, _wallDetectionRangeForward, _climbableLayer))
        {
            Debug.DrawRay(raycastStart, transform.forward * _wallDetectionRangeForward, Color.blue, 0.02f, false);
            return hit;
        }
        return default(RaycastHit);
    }

    public float DetectWallHeight()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y + _detectWallHeightOffsetY, transform.position.z) + transform.forward * _detectWallHeightOffsetX;
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, 100f, _climbableLayer))
        {
            Debug.DrawRay(raycastStart, Vector3.down * 100f, Color.blue, 0.02f, false);
            return _detectWallHeightOffsetY - hit.distance;
        }
        return 0f;
    }
}
