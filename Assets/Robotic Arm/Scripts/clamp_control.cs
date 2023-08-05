using UnityEngine;

public class ArmatureClampController : MonoBehaviour
{
    // Adjust these parameters to control the movement and interaction behavior
    public float rotationSpeed = 10f;
    public float clampForce = 100f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Example rotation control
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);
    }

    private void OnTriggerStay(Collider other)
    {
        // Example clamp behavior
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (other.attachedRigidbody)
            {
                Vector3 clampForceDirection = transform.forward; // Change the direction as needed
                other.attachedRigidbody.AddForce(clampForceDirection * clampForce, ForceMode.Force);
            }
        }
    }
}