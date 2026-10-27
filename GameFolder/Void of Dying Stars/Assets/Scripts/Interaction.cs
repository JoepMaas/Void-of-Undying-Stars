using System.Collections;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] string input = "";
    [SerializeField] PlayerHandeler playerHandeler;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lever;
    [SerializeField] AudioClip save;
    public GameObject text;

    // output when clicked
    public string output = "";

    // extras
    bool clicked = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.name != "DoubleJumpUnlock")
        {
            if (collision.tag == "Hitbox")
            {
                audioSource.clip = lever;
                audioSource.Play();    
            }
            if (collision.tag == "Hitbox" && !clicked)
            {
                gameObject.transform.Rotate(0,180,0);
                clicked = true;
                output = input;
            }
            else if (collision.tag == "Hitbox" && clicked)
            {
                gameObject.transform.Rotate(0,0,0);
                clicked = false;
                output = "";
            }
        }    
    }

    bool unlocked = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && gameObject.name == "DoubleJumpUnlock" && !unlocked)
        {
            unlocked = true;
            audioSource.clip = save;
            audioSource.Play();
            playerHandeler = collision.gameObject.GetComponentInParent<PlayerHandeler>();
            output = input;
            
            StartCoroutine(hideText());

            playerHandeler.unlockedDoubleJump = true;

        }

        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && gameObject.name == "RestingPlace")
        {
            audioSource.clip = save;
            audioSource.Play();

            playerHandeler = collision.gameObject.GetComponentInParent<PlayerHandeler>();
            playerHandeler.restingPlaceSavedX = gameObject.transform.position.x -.5f;
            playerHandeler.restingPlaceSavedY = gameObject.transform.position.y;
        }
    }

    IEnumerator hideText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2);
        text.SetActive(false);
        Debug.Log("sd");
        Destroy(gameObject);
    }

}
