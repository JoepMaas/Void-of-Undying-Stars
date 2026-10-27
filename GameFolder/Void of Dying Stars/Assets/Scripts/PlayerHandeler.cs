using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandeler : MonoBehaviour
{
    [SerializeField] UIHandeler UIHandeler;
    [SerializeField] GameObject doubleJumpInteraction;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AudioClip clip;
    [SerializeField] Animator animator;
    AudioSource audioSource; 

    public float restingPlaceSavedX;
    public float restingPlaceSavedY;

    int maxMoney = 20000;
    int money = 0;

    int maxHealth = 5; // can change with upgrades
    public int health;

    // need to unlock
    public bool unlockedDoubleJump = false;

    private void Start()
    {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void AddMoney(int Amount)
    {
        money += Amount;

        if (money > maxMoney)
        {
            money = maxMoney;   
        }
        UIHandeler.UpdateUI(money);
    }

    public void TakeDamage(int Damage)
    {
        health -= Damage;

        if (Damage > 0)
        {
            audioSource.PlayOneShot(clip);
            animator.SetBool("Hit", true);
            StartCoroutine(HitCD());
        }

        if (health <= 0)
        {
            StartCoroutine(ResetChar());
            health = maxHealth;
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }

        UIHandeler.UpdateHealthUI(health);
    }

    public void Bypass()
    {
        StartCoroutine(ResetChar());
    }

    IEnumerator HitCD()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Hit", false);
    }

    IEnumerator ResetChar()
    {
        gameObject.transform.position = new Vector2(restingPlaceSavedX,restingPlaceSavedY);
        //StartCoroutine(playerMovement.StunCD(.1f, 2f));
        yield return null;
    }

}
