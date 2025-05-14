using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TapToPlaceOrigin : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject arContentRoot; // Your 3D model or its parent GameObject
    public bool placed = false;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (placed) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // Optionally align rotation with camera forward (Y only)
                Quaternion cameraYaw = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

                arContentRoot.transform.SetPositionAndRotation(hitPose.position, cameraYaw);

                placed = true;
                Debug.Log("AR world aligned at tap position.");
            }
        }
    }
}
