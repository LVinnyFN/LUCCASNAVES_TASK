using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private static int xHash = Animator.StringToHash("x");
    private static int yHash = Animator.StringToHash("y");
    private static int collectStateHash = Animator.StringToHash("UpperBody.Collect");

    public Animator animator;
    public float blendSpeed = 1.0f;

    private Vector2 currentDirection;

    public void UpdateDirectionAnimation(Vector2 direction)
    {
        currentDirection = Vector2.Lerp(currentDirection, direction, blendSpeed * Time.deltaTime);

        animator.SetFloat(xHash, currentDirection.x);
        animator.SetFloat(yHash, currentDirection.y);
    }

    public void PlayCollectAnimation()
    {
        animator.Play(collectStateHash, 1);
    }
}