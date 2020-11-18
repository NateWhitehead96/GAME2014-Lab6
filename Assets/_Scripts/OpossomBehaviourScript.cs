using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RampDirection
{
    NONE,
    UP,
    DOWN
}


public class OpossomBehaviourScript : MonoBehaviour
{
    public float runForce;
    public Rigidbody2D rb2d;
    public bool isGrounded;
    public Transform lookahead;
    public Transform lookInfront;
    public LayerMask collisionGroundLayer;
    public LayerMask collisionWallLayer;

    public bool onRamp;
    public RampDirection rampDirection;

    public LOS opossumLOS;

    [Header("Bullet Firing")]
    public float fireDelay;
    public Transform bulletSpawn;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rampDirection = RampDirection.NONE;
        //player = GameObject.FindObjectOfType<PlayerBehaviour>();
        //direction = Vector2.left;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_HasLOS())
        {
            Debug.Log("Opossum sees player");
            // fire from here
            _FireBullet();
        }
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private void _FireBullet()
    {
        if (Time.frameCount % fireDelay == 0 && BulletManager.Instance().HasBullets())
        {
            var playerPosition = player.transform.position;
            var firingDirection = Vector3.Normalize(playerPosition - bulletSpawn.position);

            BulletManager.Instance().GetBullet(bulletSpawn.position, firingDirection);
        }
    }

    private bool _HasLOS()
    {
        if (opossumLOS.colliders.Count > 0)
        {
            if (opossumLOS.collidesWith.gameObject.name == "Player" && opossumLOS.colliders[0].gameObject.name == "Player")
            {
                return true;
            }
        }

        return false;
    }
    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInfront.position, collisionWallLayer);
        if (wallHit)
        {
            if(!wallHit.collider.CompareTag("Ramps"))
            {
                if(!onRamp && transform.rotation.z == 0.0f)
                {
                    _Flip();
                }
                rampDirection = RampDirection.DOWN;
            }
            else
            {
                rampDirection = RampDirection.UP;
            }
        }
        Debug.DrawLine(transform.position, lookInfront.position, Color.red);
    }
    private void _LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookahead.position, collisionGroundLayer);
        if (groundHit)
        {
            if(groundHit.collider.CompareTag("Ramps"))
            {
                // hitting the ramp
                //direction = new Vector2(-1f, 1f);
                onRamp = true;
            }

            if(groundHit.collider.CompareTag("Platforms"))
            {
                // hitting platforms
                onRamp = false;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawLine(transform.position, lookahead.position, Color.green);
    }

    private void _Move()
    {
        if(isGrounded)
        {
            rb2d.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);
            if (onRamp)
            {
                if(rampDirection == RampDirection.UP)
                {
                    rb2d.AddForce(Vector2.up * runForce * Time.deltaTime);
                }
                else
                {
                    rb2d.AddForce(Vector2.down * runForce * Time.deltaTime);
                }
                StartCoroutine(Rotate());
            }
            else
            {
                StartCoroutine(Normalize());
            }
            rb2d.velocity *= 0.90f;
        }
        else
        {
            _Flip();
        }

    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0f, 0f, -25f);
    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void _Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }
   
}
