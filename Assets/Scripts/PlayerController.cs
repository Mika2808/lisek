using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f; // moving speed of the player
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f; // jumping force of the player       

    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    private float rayLength = 1.5f;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRigth = true;
    private Vector3 theScale;
    private int lives = 3;
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState==GameState.GS_GAME && !animator.GetBool("isDead"))
        {
            isWalking = false;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
                if (!isFacingRigth)
                {
                    Flip();
                }

            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
                if (isFacingRigth)
                {
                    Flip();
                }
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);

            ////rysowanie kreski do po�owy platformy
            //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        }
    }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        theScale = transform.localScale;
        startPosition = transform.position;
    }
    private void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            ////komunikat  konsoli
            //Debug.Log("Jumping!!!");            
        }
    }
    private void Flip()
    {
        isFacingRigth = !isFacingRigth;     
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }
    private void Death()
    {
        if (lives == 0)
        {
            Debug.Log("Przegra�e�! Koniec gry!");
            gameObject.SetActive(false);
        }
        else
        {
            lives--;
            Debug.Log("Zgin��e�. Liczba dodatkowych �y�: " + lives);
            animator.SetBool("isDead", true);
            
            StartCoroutine(KillOnAnimationEnd());                        
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.AddPoints(50);
            //Debug.Log("Score: " + score);            
        }
        else if (other.CompareTag("Enemy"))
        {
            if(transform.position.y > other.transform.position.y)
            {                
                GameManager.instance.AddPoints(100);
                //Debug.Log("Zabito or�a!! Score: " + score);                
            }
            else
            {
                Death();
            }                       
        }
        else if (other.CompareTag("Key"))
        {
            ////robocze
            //Debug.Log(other.ToString());            
            GameManager.instance.AddKeys(other.ToString());            
            other.gameObject.SetActive(false);            
        }
        else if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("Znalaz�e� dodatkowe �ycie! Liczba �y�: " + lives);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Meta"))
        {
            GameManager.instance.Meta();            
        }
        else if (other.CompareTag("FallLevel"))
        {
            Death();
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        transform.SetParent(null);
    }

    private IEnumerator KillOnAnimationEnd()
    {
        rigidBody.simulated = false;

        yield return new WaitForSeconds(1);        
        animator.SetBool("isDead", false);
        transform.position = startPosition;
        rigidBody.simulated = true;
    }
}
