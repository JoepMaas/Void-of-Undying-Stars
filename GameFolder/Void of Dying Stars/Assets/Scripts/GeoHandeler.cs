using UnityEngine;

public class GeoHandeler : MonoBehaviour
{
    [SerializeField] int lowestNumber;
    [SerializeField] int highestNumber;

    int Amount;

    private void Start()
    {
        Randomize();
    }

    void Randomize()
    {
        Amount = Random.Range(lowestNumber, highestNumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Player")
        { 
            collision.gameObject.GetComponentInParent<PlayerHandeler>().AddMoney(Amount);
            Destroy(gameObject);
        }
    }
}
