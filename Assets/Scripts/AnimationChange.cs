
using UnityEngine;

public class AnimationChange : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (PlayerMovement.rb == null)
        {
            Debug.LogError("Rigidbody reference is missing on PlayerMovement.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing.");
        }
    }

    void Update()
    {
        if (PlayerMovement.rb != null)
        {
            Vector3 accumulatedVelocity = PlayerMovement.rb.velocity;
            if (accumulatedVelocity != Vector3.zero)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
    }
}
