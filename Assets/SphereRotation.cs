using UnityEngine;

public class SphereRotation : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 10f;
    private Vector3 lastMousePosition;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {

            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            transform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.left, mouseDelta.y * rotationSpeed * Time.deltaTime);

            lastMousePosition = Input.mousePosition;
        }
    }
}
