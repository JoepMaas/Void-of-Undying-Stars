using UnityEngine;

public class ReturnScript : MonoBehaviour
{
    public PlayerHandeler playerHandeler;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHandeler.Bypass();
        }
    }
}
