using UnityEngine;

public class MinimapFollower : MonoBehaviour
{
    [SerializeField] private Transform player3D;

    void Update()
    {
        if (player3D == null) return;

        Vector3 newPos = player3D.position;
        newPos.y = transform.position.y; 
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90, 0, player3D.eulerAngles.y);
 
    }
}