using UnityEngine;
using UnityEngine.AI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Transform arCameraTransform; // Assign this in the Inspector
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private LayerMask wallLayerMask;
    private NavMeshAgent indicatorAgent;

    private void Awake()
    {
        indicatorAgent = indicatorPrefab.GetComponent<NavMeshAgent>();
        
    }

    public void PlaceIndicatorAtMarker(Vector3 markerPosition)
    {
        Vector3 direction = markerPosition - arCameraTransform.position;
        Ray ray = new Ray(arCameraTransform.position, direction.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, direction.magnitude, wallLayerMask))
        {
           
            indicatorPrefab.transform.position = hit.point - direction.normalized * 0.1f;
        }
        else
        {
            
            indicatorPrefab.transform.position = markerPosition;
        }

        indicatorPrefab.SetActive(true);
    }

    public void SetIndicatorPosition(Vector3 targetPosition)
    {
        indicatorAgent.SetDestination(targetPosition);
    }
}
