using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPaddleController : MonoBehaviour
{
    public float speed = 3f;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private GameObject ball;


    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //ball = GameObject.Find("Ball");
        spriteRenderer.color = SaveController.Instance.GetColor(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (ball != null)
        //{
        //    float targetY = Mathf.Clamp(ball.transform.position.y, -4.5f, 4.5f);
        //    Vector2 targetPosition = new Vector2(transform.position.x, targetY);
        //    transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        //}
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= speed * Time.deltaTime;
        }
        newPosition.y = Mathf.Clamp(newPosition.y, -4.5f, 4.5f);
        transform.position = newPosition;

    }
}
