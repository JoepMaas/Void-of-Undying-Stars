using System;
using System.Collections;
using System.Reflection.Emit;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform Enemy;
    public Rigidbody2D rb;
    EnemyScript enemyScript;
    [SerializeField] GameObject hitbox;
    [SerializeField] Transform hitboxPosition;
    [SerializeField] Animator animator;
    [SerializeField] GameObject nail;
    [SerializeField] AudioClip clip;
    AudioSource audioSource;
    
    bool canAttack = true;

    Transform player;
    int attack = 0;
    int attackCooldown = 2;

    void Start()
    {
       //    rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canAttack)
        {
            if (attack == 1)
            {
                AttackOne(player);
            }
            else if (attack == 2)
            {
                AttackTwo();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject.transform;

            if (canAttack)
            {
                attack = RandomAttack();
            }
            else
            {
                attack = 0;
            }
        }
    }


    void AttackOne(Transform playerPosition)
    {
        StartCoroutine(StunCD(1.2f));
        StartCoroutine(attackCD(attackCooldown));

        GameObject hitboxthing = Instantiate(hitbox,hitboxPosition);
        hitboxthing.transform.Rotate(0,0,90);
        hitboxthing.transform.position = playerPosition.position;
        hitboxthing.transform.position = new Vector2(hitboxthing.transform.position.x, -15);
        
        GameObject body = Instantiate(nail, hitboxPosition);
        body.transform.position = playerPosition.position;
        body.transform.position = new Vector2(body.transform.position.x, -9);
        StartCoroutine(attackOnNail(body.transform, hitboxthing.transform));

        Destroy(body, 1.2f);
        Destroy(hitboxthing, 1.2f);
    }

    void AttackTwo()
    {
        enemyScript.StunCD(.8f);
        StartCoroutine(attackCD(attackCooldown));
        GameObject hitboxthing = Instantiate(hitbox, hitboxPosition);
        hitboxthing.transform.position = new Vector2(hitboxthing.transform.position.x - (enemyScript.direction * 10f), hitbox.transform.position.y - 3);  

        GameObject body = Instantiate(nail, hitboxPosition);
        body.transform.position = new Vector2(body.transform.position.x - (enemyScript.direction * 7), body.transform.position.y + 5);
        body.transform.Rotate(0,0,90);
        StartCoroutine(TrowNail(body.transform, hitboxthing.transform));

        Destroy(body, .8f);
        Destroy(hitboxthing, .8f);
    }

    IEnumerator attackCD(int time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);   
        canAttack = true;
    }

    IEnumerator attackOnNail(Transform nail, Transform hitbox)
    {
        animator.SetBool("Walking", false);
        animator.SetBool("NailAttack", true);
        yield return new WaitForSeconds(.3f);
        animator.SetBool("Walking", false);
        animator.SetBool("NailAttack", true);
        enemyScript.stunned = true;
        yield return new WaitForSeconds(.4f);
        audioSource.clip = clip;
        audioSource.Play();
        nail.position = new Vector2(nail.transform.position.x, nail.transform.position.y + 6f);
        hitbox.position = new Vector2(hitbox.position.x, hitbox.position.y + 6f);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("NailAttack", false);
    }

    IEnumerator TrowNail(Transform nail, Transform hitbox)
    {
        yield return new WaitForSeconds(.6f);
        audioSource.clip = clip;
        audioSource.Play();
        nail.position = new Vector2(nail.transform.position.x + (enemyScript.direction * 7), nail.transform.position.y);
        hitbox.position = new Vector2(hitbox.position.x + (enemyScript.direction * 7), hitbox.position.y);
    }

    IEnumerator StunCD(float time)
    {
        enemyScript.stunned = true;
        yield return new WaitForSeconds(0.3f);
        enemyScript.stunned = true;
        yield return new WaitForSeconds(time - 0.3f);
        enemyScript.stunned = false;   
    }

    int RandomAttack()
    {
        return UnityEngine.Random.Range(1, 3);
    }
   

    
    
}
