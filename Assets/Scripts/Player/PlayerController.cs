using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public int maxJumps;
    public LayerMask whatIsGround;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    public bool isFracingRight = true;
    private int jumpsLeft;

    [Header("Jump on Enemy")]
    [SerializeField] private float reboundSpeed;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, whatIsGround);
        return raycastHit.collider != null;
    }

    void ProcesarSalto()
    {
        if (EstaEnSuelo())
        {
            jumpsLeft = maxJumps;
            animator.SetBool("isGrounded", false);
        }
        else
        {
            animator.SetBool("isGrounded",true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            jumpsLeft--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Rebound()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, reboundSpeed);
    }

    void ProcesarMovimiento()
    {
        float xMovement = Input.GetAxis("Horizontal");
        if(xMovement != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        rigidBody.velocity = new Vector2(xMovement * speed, rigidBody.velocity.y);

        GestionarOrientacion(xMovement);
    }

    void GestionarOrientacion(float xMovement)
    {
        if ((isFracingRight == true && xMovement < 0) || (isFracingRight == false && xMovement > 0))
        {
            isFracingRight = !isFracingRight;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180);
        }
    }
}
