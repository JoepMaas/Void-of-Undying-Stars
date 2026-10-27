using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] RawImage block;
    [SerializeField] GameObject settingsMenu;

    private void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(FadeIn());
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);   
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitSettings()
    {
        settingsMenu.SetActive(false);
    }

    IEnumerator FadeIn()
    {   
        Color c = block.color;
        c.a = 0;
        block.color = c;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = t / 1;
            block.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainGame");
    }


}
