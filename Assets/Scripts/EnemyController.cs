using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private bool isMovingRight = true;
    [SerializeField] private bool isFacingRigth = true;
    public float moveRange = 4.0f;
    private float startPositionX;
    private Animator animator;
    private Vector3 theScale;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if(transform.position.x < startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                Flip();
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
                Flip();
                MoveRight();
                isMovingRight = true;
            }
        }
    }
    private void Awake()
    {       
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
        theScale = transform.localScale;
        if (isMovingRight)
        {
            Flip();
        }
    }
    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void Flip()
    {
        isFacingRigth = !isFacingRigth;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position.y < other.transform.position.y)
            {
                animator.SetBool("isDead",true);
                StartCoroutine(KillOnAnimationEnd());                               
            }
        }
    }
    private IEnumerator KillOnAnimationEnd()
    {
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(2);
        
        gameObject.SetActive(false);
    }
}
