using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform xrCamera; // Assign your AR Camera here
    [SerializeField] private Transform hostelRoot; // Assign your aligned hostel prefab root

    void Update()
    {
        if (xrCamera == null || hostelRoot == null) return;

        // Convert AR Camera position to local space of the hostel
        Vector3 localPos = hostelRoot.InverseTransformPoint(xrCamera.position);
        transform.localRotation = Quaternion.Euler(0, xrCamera.eulerAngles.y, 0);
        // Keep the Y low (like on the ground)
        localPos.y = 0.05f;

        transform.localPosition = localPos;
    }
}
