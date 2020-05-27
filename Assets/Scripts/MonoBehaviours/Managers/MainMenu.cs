using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private static bool gameHasStarted = false;

    [SerializeField] private GameObject uiCanvas = default;
    [SerializeField] private GameObject mainMenuPlatform = default;
    [SerializeField] private GameObject mainMenuGameObjectsParent = default;

    [SerializeField] private Vector3 playerMenuPosition;
    [SerializeField] private Vector3 cameraMenuPostion;

    [SerializeField] private Vector3 playerMenuEulerAngels;
    [SerializeField] private Vector3 cameraMenuEulerAngels;




    private void Awake()
    {
        if (!gameHasStarted)
        {
            EnableMenuGameObjects();
            PlacePlayerAndDisableControls();
            PlaceCameraAndDisableFollow();
        }
    }


    private void PlacePlayerAndDisableControls()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        Player.transform.position = playerMenuPosition;
        Player.transform.eulerAngles = playerMenuEulerAngels;
        Player.GetComponent<PlayerController>().DisableControls();
    }


    private void PlaceCameraAndDisableFollow()
    {
        GameObject mainCamera = Camera.main.gameObject;
        mainCamera.transform.position = cameraMenuPostion;
        mainCamera.transform.eulerAngles = cameraMenuEulerAngels;
        mainCamera.GetComponent<CameraController>().DisableCameraFollow();

    }


    private void EnableMenuGameObjects()
    {
        uiCanvas.SetActive(true);
        mainMenuPlatform.SetActive(true);
        mainMenuGameObjectsParent.SetActive(true);
    }

    public void OnGameStarted()
    {
        gameHasStarted = true;
        uiCanvas.SetActive(false);

        Destroy(mainMenuPlatform);
        StartCoroutine(DisableMenuGameObjects());
    }


    public void OnQuit()
    {
        Application.Quit();
    }


    IEnumerator DisableMenuGameObjects()
    {
        yield return new WaitForSecondsRealtime(5f);
        mainMenuGameObjectsParent.SetActive(false);
    }





}
