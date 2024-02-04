using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MoveCharacter : MonoBehaviour
{
    public Animator animator;
    public GameObject target;
    public float horizontal;
    private float speed = 8f;
    public bool isFacingRight = true;
    public bool isPhone;
    public bool home;
    public bool gameOver;
    public bool flipstart;
    public float velocity;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        if (home)
        {
            animator.SetBool("home", true);
        }

        if(DialogueManager.GetInstance().gameOver)
        {
            gameOver = true;
        }
        
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            horizontal = 0;
            animator.SetFloat("speed_i", Mathf.Abs(horizontal));
            return;
        }

        if (PhoneManager.GetInstance().phoneIsActive)
        {
            horizontal = 0;
            animator.SetFloat("speed_i", Mathf.Abs(horizontal));
            animator.SetBool("phone", PhoneManager.GetInstance().phoneIsActive);
            return;
        }

        velocity = rb.velocity.x;
        if (velocity == 0)
        {
            animator.SetFloat("speed_i", 0);
        } 
        else
        {
            animator.SetFloat("speed_i", Mathf.Abs(horizontal));
        }

        animator.SetBool("phone", PhoneManager.GetInstance().phoneIsActive);
        horizontal = Input.GetAxisRaw("Horizontal");
         
        Flip();
        animator.SetFloat("velocity", horizontal);
        if (!isFacingRight || horizontal < 0f)
        {
            animator.SetBool("faceright", false); 
        }
        else
        {
            animator.SetBool("faceright", true); 
        }
    }

    
    

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }
    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || PhoneManager.GetInstance().phoneIsActive || DialogueManager.GetInstance().gameOver)
        {
            horizontal = 0;
            animator.SetFloat("speed_i", Mathf.Abs(horizontal));
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);            
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }


    private void Flip()
    {
        if (((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) && !isPhone && !gameOver))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;                       
        }
    }
}