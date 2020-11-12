using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool grounded;
    public bool crouching = false;
    public bool jumping = false;
    public GameObject spawnPoint;

    private Rigidbody2D m_rb2d;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        if (grounded)
        {


            if (!jumping && !crouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    m_rb2d.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", 1);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rb2d.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", 1);
                }
                else
                {
                    //idle
                    m_animator.SetInteger("AnimState", 0);
                }

            }
            if (joystick.Vertical > joystickVerticalSensitivity && !jumping)
            {
                // jump
                m_rb2d.AddForce(Vector2.up * verticalForce * Time.deltaTime);
                m_animator.SetInteger("AnimState", 2);
                jumping = true;
            }
            else
            {
                jumping = false;
            }

            if (joystick.Vertical < -joystickVerticalSensitivity && (!crouching))
            {
                m_animator.SetInteger("AnimState", 3);
                crouching = true;
            }
            else
            {
                crouching = false;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // respawn
        if(collision.gameObject.CompareTag("Fire"))
        {
            transform.position = spawnPoint.transform.position;

        }
    }
}
