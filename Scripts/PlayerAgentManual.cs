using UnityEngine;

public class PlayerAgentManual : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }
}