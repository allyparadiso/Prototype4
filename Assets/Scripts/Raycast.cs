using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float distance = 8f;
    private Ray ray;
    private void Start()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }
    private void Update()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, distance)) //add a CompareTag("Player") 
        {
            //trigger attack
        }
    }
}
