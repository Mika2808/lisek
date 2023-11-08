using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 4.0f;
    private bool isMovingRight = true;
    [SerializeField] private bool horizontal = false;
    public float moveRange = 4.0f;
    private float startPositionX;
    private float startPositionY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal)
        {
            if (isMovingRight)
            {
                if (transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                    isMovingRight = false;
                }
            }
            else
            {
                if (transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                    isMovingRight = true;
                }
            }
        }
        else
        {
            if (isMovingRight)
            {
                if (transform.position.y < startPositionY + moveRange)
                {
                    MoveUp();
                }
                else
                {
                    MoveDown();
                    isMovingRight = false;
                }
            }
            else
            {
                if (transform.position.y > startPositionY - moveRange)
                {
                    MoveDown();
                }
                else
                {
                    MoveUp();
                    isMovingRight = true;
                }
            }
        }

    }
    private void Awake()
    {
        startPositionX = this.transform.position.x;
        startPositionY = this.transform.position.y;
    }
    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void MoveUp()
    {
        transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }
    private void MoveDown()
    {
        transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }
}
