using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject hitboxDown;
    [SerializeField] Transform hitboxPosition; 
    [SerializeField] Transform hitboxPositionUp;
    [SerializeField] Transform hitboxPositionDown;
    [SerializeField] AudioClip clip;
    [SerializeField] Animator animator;

    AudioSource audioSource;

    bool canAttack = true;
    public string state;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void attack(float attackCD)
    {
        if (canAttack)
        {
            audioSource.PlayOneShot(clip);

            canAttack = false;
            if (state == "up")
            {
                GameObject hitboxthing = Instantiate(hitbox, hitboxPositionUp);
                Destroy(hitboxthing, .2f);
            }
            else if (state == "down" && canAttack == false)
            {
                GameObject hitboxthing = Instantiate(hitboxDown, hitboxPositionDown);
                Destroy(hitboxthing, .2f);
            }
            else
            {
                animator.SetBool("Attack", true);
                GameObject hitboxthing = Instantiate(hitbox, hitboxPosition);
                Destroy(hitboxthing, .2f);
            }
         
            StartCoroutine(AttackCooldown(attackCD));
        }
    }

    IEnumerator AttackCooldown(float attackCD)
    {
        yield return new WaitForSeconds(.1f);
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

}
