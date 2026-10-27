using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject geo;
    [SerializeField] GameObject body;
    [SerializeField] Attack attack;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioClip clip;
    AudioSource audioSource; 
    public Transform Enemy;

    private int damage = 1;
    public int health = 100;
    public int direction = 1;
    public int moveSpeed = 5;

    public bool canAttack = true;

    private float hitCooldown = 1.4f;
    public float rotationY;

    bool isFacingRight = true;
    bool follow = false;
    public bool stunned = false;

    private void Start()
    {
       Enemy = gameObject.GetComponentInParent<Transform>();
       animator = GetComponent<Animator>();
       audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        rotationY = Enemy.rotation.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {          
            follow = true; 
            Transform player = collision.gameObject.GetComponentInParent<Transform>();
            float distance = Vector2.Distance(player.position, Enemy.position);

            if (!stunned)
            {
                animator.SetBool("Resting", false);
                if (distance > 3.4f)
                {
                    animator.SetBool("Walking", true);
                    if (player.position.x >= Enemy.position.x && isFacingRight)
                    {
                        isFacingRight = false;
                        Enemy.rotation = Quaternion.Euler(0, 180, 0);
                        direction = 1;
                    }
                    else if (player.position.x <= Enemy.position.x && !isFacingRight)
                    {
                        isFacingRight = true;
                        Enemy.rotation = Quaternion.Euler(0, 0, 0);
                        direction = -1;
                    }

                    if (follow)
                    {
                        rb.linearVelocity = new Vector2(direction * moveSpeed, 0);
                    }
                    else
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                
                }
                if (distance <= 4f && canAttack)
                {
                    animator.SetBool("Walking", false);
                    if (player.position.y >= Enemy.position.y + 0.10f)
                    {
                        attack.state = "up";
                    }
                    else
                    {
                        attack.state = "Horizontal";
                    }

                    attack.attack(hitCooldown);
                }
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            follow = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!stunned)
        {
            health -= damage;
            stunned = true;
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(HitPart());

        }

        if (health <= 0) //ded
        {
            Instantiate(geo, transform.position, Quaternion.identity);
            Destroy(body);
        }

        if (stunned)
        {
            StartCoroutine(StunCD(.3f));
        }
    }

    public IEnumerator StunCD(float time)
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Resting", true);
        stunned = true;
        yield return new WaitForSeconds(.1f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    IEnumerator HitPart()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.darkGray;
        yield return new WaitForSeconds(.3f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
}
