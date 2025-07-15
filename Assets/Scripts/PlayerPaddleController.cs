using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    public float speed = 5f;

    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.color = SaveController.Instance.GetColor(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            newPosition.y += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newPosition.y -= speed * Time.deltaTime;
        }
        newPosition.y = Mathf.Clamp(newPosition.y, -4.5f, 4.5f);
        transform.position = newPosition;


    }
}
