using UnityEngine;

public class Biome : MonoBehaviour
{
    [SerializeField] UIHandeler handeler;
    [SerializeField] PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Body")
        {
            handeler.Biome(gameObject.tag);
            playerMovement.biome = gameObject.tag;  
        }
    }
}
