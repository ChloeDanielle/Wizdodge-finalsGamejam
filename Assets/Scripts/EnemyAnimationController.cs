using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("Die"); // Trigger the death animation
        }
    }
}
