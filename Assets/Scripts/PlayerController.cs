using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int speed = 10;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    private float movementX;
    private float movementY;
    private Vector2 movement_direction;

    public TrailRenderer trail;
    private bool canDash = true;
    private bool isDashing;
    public int dashSpeed = 20;
    public float dashingTime = 0.5f;
    public float dashingCooldown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) { return; }

        Move();
        Shoot();

        //calling the dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash == true)
        {
            StartCoroutine(Dash());
        }
    }


    private void Move()
    {
        //actual movement
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");
        movement_direction = new Vector2(movementX, movementY).normalized;
        rb.velocity = new Vector2(movement_direction.x * speed, movement_direction.y * speed);

        //running animation
        if (movement_direction.magnitude > 0)
        {
            animator.SetBool("isRunning", true);
        }

        else
        {
            animator.SetBool("isRunning", false);
        }

        //facing 
        if (movementX < 0)
        {
            gameObject.transform.localScale= new Vector3(-1,1,1);
        }
        else if (movementX > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }


    }

    private void Shoot()
    {
        //shooting animation
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("isShooting", true);
        }

        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isShooting", false);
        }
    }


    private IEnumerator Dash()
    {
        //dashing
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(movement_direction.x * dashSpeed, movement_direction.y);
        trail.emitting = true;

        
        yield return new WaitForSeconds(dashingTime);

        //cooldown
        trail.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;


    }
}