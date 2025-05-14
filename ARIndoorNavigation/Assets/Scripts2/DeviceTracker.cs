

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DeviceTracker : MonoBehaviour
{
    public GameObject deviceIndicatorPrefab;
    public Camera arCamera;

    private GameObject indicatorInstance;

    void Start()
    {
        if (deviceIndicatorPrefab != null && arCamera != null)
        {
            indicatorInstance = Instantiate(deviceIndicatorPrefab);
        }
    }

    void Update()
    {
        if (indicatorInstance != null && arCamera != null)
        {
            Vector3 cameraPosition = arCamera.transform.position;
            indicatorInstance.transform.position = cameraPosition;

            // Optional: rotate to match camera facing direction
            Quaternion cameraRotation = arCamera.transform.rotation;
            indicatorInstance.transform.rotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        }
    }
}

    
