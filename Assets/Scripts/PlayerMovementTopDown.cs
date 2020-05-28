using UnityEngine;

public class PlayerMovementTopDown : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    private void Update()
    {
        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x+rb.velocity.y);
        animator.SetFloat("Speed", characterVelocity);
    }

    private void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime;
        verticalMovement = Input.GetAxis("Vertical") * Time.deltaTime;

        MovePlayer(horizontalMovement, verticalMovement);
    }

    private void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, _verticalMovement) * moveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }

    private void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        } else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

}
