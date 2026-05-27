using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
    public Rigidbody agentRb;
    public float rotationSpeed = 10f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agentRb == null || animator == null) return;

        Vector3 horizontalVelocity = new Vector3(agentRb.linearVelocity.x, 0f, agentRb.linearVelocity.z);
        float speed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", speed);

        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}