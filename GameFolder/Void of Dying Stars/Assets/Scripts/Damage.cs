using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    bool hit = false;
    bool parried = false;
    Rigidbody2D rbPlayer;

    private ParticleSystem particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            //particle.
            particle = collision.GetComponentInParent<ParticleSystem>();
            particle.Emit(20);
            parried = true;

        }
        else
        {
            if (collision.tag == "Enemy" && !parried && gameObject.name != "Enemy")
            {
                bool stunned = collision.gameObject.GetComponentInChildren<EnemyScript>().stunned;
                Rigidbody2D rb = collision.gameObject.GetComponentInChildren<Rigidbody2D>();

                if (!stunned)
                {
                    stunned = true;

                    particle = collision.GetComponentInParent<ParticleSystem>();
                    particle.Emit(20);

                    if (gameObject.name == "HitboxDown(Clone)")
                    {
                        hit = true;
                        rbPlayer = GetComponentInParent<PlayerMovement>().rb;
                        rbPlayer.AddForce(Vector2.up * 250, ForceMode2D.Impulse);
                    }
                    if (collision.gameObject.GetComponentInChildren<EnemyScript>().rotationY == 1 && !hit) // sideways knockback
                    {
                        hit = true;
                        rb.AddForce(Vector2.left * 25000, ForceMode2D.Impulse);
                    }
                    else
                    {
                        if (!hit)
                        {
                            hit = true;
                            rb.AddForce(Vector2.right * 25000, ForceMode2D.Impulse);
                        }
                    }
                    if (hit)
                    {
                        collision.gameObject.GetComponentInChildren<EnemyScript>().TakeDamage(25);
                        GetComponentInParent<PlayerMovement>().focus += Random.Range(5, 10);
                    }
                }

            }
            else if (collision.tag == "Player" && !parried)      
            {
                PlayerMovement pM = collision.gameObject.GetComponentInParent<PlayerMovement>();
                rbPlayer = pM.rb;
                pM.stunned = true;

                Transform Enemy = GetComponentInParent<EnemyScript>().Enemy;

                if (pM.gameObject.transform.position.y >= Enemy.position.y + 0.10f)
                {
                    rbPlayer.AddForce(Vector2.up * 250, ForceMode2D.Impulse);
                }
                else if (Enemy.transform.rotation.y == 1) // sideways knockback
                {
                    rbPlayer.AddForce(Vector2.right * 400, ForceMode2D.Impulse);
                }
                else
                {
                    rbPlayer.AddForce(Vector2.left * 400, ForceMode2D.Impulse);
                }
                collision.gameObject.GetComponentInParent<PlayerHandeler>().TakeDamage(1);
            }
        }
        
    }
}
