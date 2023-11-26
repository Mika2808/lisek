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
    [SerializeField] private AudioClip bonusSound;
    [SerializeField] private AudioClip gemSound;
    [SerializeField] private AudioClip eagleSound;
    [SerializeField] private AudioClip metaSound;
    [SerializeField] private AudioClip deathSound;

    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    private float rayLength = 1.5f;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRigth = true;
    private Vector3 theScale;
    private int lives = 3;
    private Vector2 startPosition;
    private AudioSource source;

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

            ////rysowanie kreski do po³owy platformy
            //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        }
    }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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
            Debug.Log("Przegra³eœ! Koniec gry!");
            gameObject.SetActive(false);
        }
        else
        {
            lives--;
            Debug.Log("Zgin¹³eœ. Liczba dodatkowych ¿yæ: " + lives);
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
            source.PlayOneShot(bonusSound, AudioListener.volume);
            //Debug.Log("Score: " + score);            
        }
        else if (other.CompareTag("Enemy"))
        {
            if(transform.position.y > other.transform.position.y)
            {                
                GameManager.instance.AddPoints(100);
                source.PlayOneShot(eagleSound, AudioListener.volume); // roboczo
                //Debug.Log("Zabito or³a!! Score: ");                
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
            source.PlayOneShot(gemSound, AudioListener.volume);
        }
        else if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("Znalaz³eœ dodatkowe ¿ycie! Liczba ¿yæ: " + lives);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Exit"))
        {
            GameManager.instance.Meta(lives);
            source.PlayOneShot(metaSound, AudioListener.volume);
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
        source.PlayOneShot(deathSound, AudioListener.volume);
        yield return new WaitForSeconds(1);        
        animator.SetBool("isDead", false);
        transform.position = startPosition;
        rigidBody.simulated = true;
    }
}
