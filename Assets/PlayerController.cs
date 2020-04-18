using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float move_speed = 100f;
    public Vector2 direction;
    private bool facing_right = true;

    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public BoxCollider collider;

    public float max_speed = 7f;
    public float linear_drag = 7f;
    public float gravity = 1f;
    public float fall_multiplier = 5f;

    public bool on_ground = false;
    public float ground_length = 1f;

    public float jump_force = 25f;

    public float previous_x_velocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        previous_x_velocity = rb.velocity.x;
    }

    // Update is called once per frame
    void Update()
    {
        on_ground = Physics2D.Raycast(transform.position + Vector3.left * 0.31f, Vector2.down, ground_length, groundLayer) ||
            Physics2D.Raycast(transform.position + Vector3.left * -0.31f, Vector2.down, ground_length, groundLayer);
        if (Input.GetButtonDown("Jump") && on_ground)
        {
            Jump();
        }
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        MoveCharacter(direction.x);
        ModifyPhysics();
        previous_x_velocity = rb.velocity.x;
    }

    void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * move_speed);

        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical", rb.velocity.y);
        animator.SetBool("grounded", on_ground);
        

        if ((horizontal > 0 && !facing_right) || horizontal < 0 && facing_right)
        {
            Flip();
        }
        
        if(Mathf.Abs(rb.velocity.x) > max_speed)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -max_speed, max_speed), rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
    }

    void ModifyPhysics()
    {
        bool changing_direction = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (on_ground)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changing_direction)
            {
                rb.drag = linear_drag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linear_drag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fall_multiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fall_multiplier / 2);
            }
        }
    }

    void Flip()
    {
        facing_right = !facing_right;
        transform.rotation = Quaternion.Euler(0, facing_right ? 0 : 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * 0.31f, transform.position + Vector3.left * 0.31f + Vector3.down * ground_length);
        Gizmos.DrawLine(transform.position + Vector3.left * -0.31f, transform.position + Vector3.left * -0.31f + Vector3.down * ground_length);
        // Gizmos.DrawLine(transform.position , transform.position + Vector3.down * ground_length);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(rb.velocity.x, rb.velocity.y));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, rb.velocity.y));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(rb.velocity.x, 0));
    }
}
