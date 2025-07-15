using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BallController : MonoBehaviour
{
    public Vector2 startVelocity = new Vector2(5f, 5f);
    public GameManager gameManager;
    public GameObject collisionVFX;
    public float speedUp = 1.1f;

    private Rigidbody2D rb;
    private string lastContact;

    public void ResetBall()
    {
        transform.position = Vector2.zero;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = startVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                break;
            case "Player":
            case "Enemy":
                if (string.IsNullOrEmpty(lastContact) || !collision.gameObject.CompareTag(lastContact))
                {
                    lastContact = collision.gameObject.tag;
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    rb.velocity *= speedUp;
                    var quart = lastContact == "Enemy" ? Quaternion.Euler(0, 0, 90f) : Quaternion.Euler(0, 0, 270f);
                    GameObject vfx = Instantiate(collisionVFX, collision.contacts[0].point, quart);
                    Destroy(vfx, 1f);
                }
                break;
            case "WallEnemy":
                gameManager.ScoreEnemy();
                ResetBall();
                break;
            case "WallPlayer":
                gameManager.ScorePlayer();
                ResetBall();
                break;
            default:
                break;
        }

    }

    public void StopBall()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
