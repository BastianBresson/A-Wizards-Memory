using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool gameHasStarted = false;

    [SerializeField] private GameObject uiCanvas = default;
    [SerializeField] private GameObject mainMenuPlatform = default;
    [SerializeField] private GameObject mainMenuClouds = default;


    private void Awake()
    {
        if (!gameHasStarted)
        {
            uiCanvas.SetActive(true);
            mainMenuPlatform.SetActive(true);
            mainMenuClouds.SetActive(true);
        }
    }


    public void OnGameStarted()
    {
        gameHasStarted = true;
        uiCanvas.SetActive(false);

        Destroy(mainMenuPlatform);
        StartCoroutine(DisableMenuClouds());
    }


    public void OnQuit()
    {
        Application.Quit();
    }


    IEnumerator DisableMenuClouds()
    {
        yield return new WaitForSecondsRealtime(5f);
        mainMenuClouds.SetActive(false);
    }
}
