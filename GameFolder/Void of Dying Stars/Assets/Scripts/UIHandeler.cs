using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class UIHandeler : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text biomeText;
    [SerializeField] GameObject biomeContainer;
    [SerializeField] GameObject[] healthIcons;
    [SerializeField] GameObject container;
    private void Start()
    {
        UpdateUI(0);
    }
    public void UpdateUI(int AmountMoney)
    {
        moneyText.text = AmountMoney.ToString();
    }

    public void Biome(string Biome)
    {
        StartCoroutine(UpdateBiome(Biome)); 
    }

    public void Continue()
    {
        container.SetActive(false);
    }

    public void MainMenu()
    {
        container.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
        }
    }

    public IEnumerator UpdateBiome(string Biome)
    {
        biomeContainer.SetActive(true);
        biomeText.text = Biome;

        yield return new WaitForSeconds(3);

        biomeContainer.SetActive(false);
    }

    public void UpdateHealthUI(int Health)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < Health)
            {
                healthIcons[i].GetComponent<RawImage>().color = Color.white;
            }
            else
            {
                healthIcons[i].GetComponent<RawImage>().color = Color.black;
            }
        }
    }
}
