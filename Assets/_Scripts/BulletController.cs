using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IApplyDamage
{
    public float verticalSpeed;
    public float verticalBoundary;
    public int damage;

    public ContactFilter2D contactFilter;
    public List<Collider2D> colliderList;
    private BoxCollider2D boxCollider;

    public Vector3 direction;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //direction = Vector3.up;
    }
    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
        _CheckCollision();
    }

    private void _CheckCollision()
    {
        Physics2D.GetContacts(boxCollider, contactFilter, colliderList);
        if (colliderList.Count > 0)
        {
            if (colliderList[0] != null)
            {
                BulletManager.Instance().ReturnBullet(gameObject);
            }
        }
    }

    private void _Move()
    {
        transform.position += direction * verticalSpeed * Time.deltaTime;
    }

    private void _CheckBounds()
    {
        if (transform.position.y > verticalBoundary)
        {
            BulletManager.Instance().ReturnBullet(gameObject);
        }
    }

    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    switch(other.gameObject.tag)
    //    {
    //        case "Enemy":
    //            BulletManager.Instance().ReturnBullet(gameObject);
    //            break;
    //        case "Player":
    //            BulletManager.Instance().ReturnBullet(gameObject);
    //            break;
    //    }
    //    //Debug.Log(other.gameObject.name);
    //    //if(other.gameObject.CompareTag("Enemy"))
    //    //{
    //    //    BulletManager.Instance().ReturnBullet(gameObject);

    //    //}
    //}

    public int ApplyDamage()
    {
        return damage;
    }
}
