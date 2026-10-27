using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{

    //puzzle condition.
    // lever 1 and 3 need to be clicked up, 2 cant be clicked.
    // Output = Double Jump.

    [SerializeField] GameObject lever1;
    [SerializeField] GameObject lever2;
    [SerializeField] GameObject lever3;
    [SerializeField] GameObject DoubleJumpThing;
    bool unlocked = false;

    private void Update()
    {
        if (lever1.GetComponent<Interaction>().output == "Up" && lever3.GetComponent<Interaction>().output == "Up" && lever2.GetComponent<Interaction>().output == "" && unlocked == false)
        {
            unlocked = true;
            DoubleJumpThing.SetActive(true);
        }
    }
}
