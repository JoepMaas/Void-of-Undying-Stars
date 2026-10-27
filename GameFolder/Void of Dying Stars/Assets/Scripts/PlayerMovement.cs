using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask mask;
    [SerializeField] Attack playerAttack;
    [SerializeField] PlayerHandeler playerHandeler;
    [SerializeField] GameObject player;
    [SerializeField] RawImage slider;
    [SerializeField] Animator animator;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] ParticleSystem ps;
    AudioSource audioSource;
    
    //Locked Abilities.
    public bool unlockedDoubleJump;

    //Variables
    public Rigidbody2D rb;
    private string state;
    public string biome;

    public int focus = 100;
    int moveSpeed = 5;
    int jumpPower = 15;
    int dashPower = 15;

    float horizontal;
    float vertical;
    float attackCD = .4f;

    float[] gravityScales = { 1.2f, 0.2f, 5, 0.5f, 0.9f }; // 1=normal, 2=Noctivane, 3=Lumenfall, 4=Duskmarrow, 5=Umbracline

    public bool isFacingRight = false;
    public bool stunned = false;
    bool healing = false;
    bool dashedCD = false;
    bool doubleJumped = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        playerHandeler = GetComponent<PlayerHandeler>();
        slider.GetComponent<UnityEngine.UI.Slider>().maxValue = focus;
        animator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        slider.GetComponent<UnityEngine.UI.Slider>().value = focus;  
        unlockedDoubleJump = playerHandeler.unlockedDoubleJump;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", horizontal);

        CheckState();
        if (!stunned)
        {
            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            }
            else if (Input.GetButtonDown("Jump") && !isGrounded() && unlockedDoubleJump && !doubleJumped)
            {
                doubleJumped = true;
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 10);
            }

            if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY / 3);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dash();
            }
            if (Input.GetKey(KeyCode.LeftControl) && horizontal == 0 && vertical == 0 && !stunned && focus >= 20 && isGrounded())
            {
                StartCoroutine(Heal());
            }
            else
            {
                healing = false;
                animator.SetBool("Heal", false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                playerAttack.attack(attackCD);
            }

            if (isGrounded())
            {
                animator.SetBool("Jumping", false);
                doubleJumped = false;
                
                
                ps.Play();
            }
            else
            {
                animator.SetBool("Jumping", true);
                ps.Stop();
            }

            Flip();
        }

        if (focus > 100)
        {
            focus = 100;
        }

        BiomeSwitcher();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (!stunned)
        {
            rb.AddForce(Vector2.down * 10);
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }  
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, mask); 
    }

    void dash()
    {
        if (!dashedCD)
        { 
            audioSource.PlayOneShot(sounds[2]);
            dashedCD = true;

            rb.AddForceX(horizontal * (dashPower * 10000), ForceMode2D.Force);
            StartCoroutine(DashCooldown());
        }
    }

    void BiomeSwitcher()
    {
        switch (biome)
        {
            case "StarlessReef":
                rb.gravityScale = gravityScales[0];
                break;
            case "Noctivane":
                rb.gravityScale = gravityScales[1];
                break;
            case "Lumenfall":
                rb.gravityScale = gravityScales[2];
                break;
            case "Duskmarrow":
                rb.gravityScale = gravityScales[3];
                break;
            case "Umbracline":
                rb.gravityScale = gravityScales[4];
                break;
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(2);
        dashedCD = false;
    }

    IEnumerator Heal()
    {
        if (!healing)
        {
            animator.SetBool("Heal", true);
            healing = true; 
            yield return new WaitForSeconds(1.5f);

            if (healing && playerHandeler.health < 5)
            {
                playerHandeler.TakeDamage(-1);
                focus -= 20;
                audioSource.PlayOneShot(sounds[1]);
                animator.SetBool("Heal", false);
            }

            healing = false;
        }
    }

    void Flip()
    {
        if (isFacingRight == true && horizontal < 0f) // left
        {
            isFacingRight = false;
            player.transform.Rotate(0, -180, 0);
        }
        else if (isFacingRight == false && horizontal > 0f)
        {
            isFacingRight = true;
            player.transform.Rotate(0, 180, 0);
        }
    }

    bool playingSound = false;

    void CheckState()
    {
        if (horizontal == 0)
        {
            if (audioSource.clip = sounds[0])
            {
                if (playingSound)
                {
                playingSound = false;
                audioSource.Stop();   
                    
                }
            }
            animator.SetBool("Resting", true);
        }
        else
        {
            if (!playingSound)
            {
                playingSound = true;
                audioSource.clip = sounds[0];
                audioSource.Play();        
            }
            animator.SetBool("Resting", false);
        }

        if (horizontal != 0)
        {
            state = "Walking";
        }

        if (vertical == 1)
        {
            state = "up";
        }
        else if (vertical == -1)
        {
            state = "down";
        }
       
        if (stunned == true)
        {
            StartCoroutine(StunCD(.1f, .1f));
        }

        playerAttack.state = state;
    }

    public IEnumerator StunCD(float cd, float lcd)
    {
        stunned = true;
        yield return new WaitForSeconds(cd);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(lcd);
        stunned = false;
    }
}
