using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static bool gameIsStarted = false;

    private GameObject player;

    [SerializeField] private float gameStartedDelay = 0.5f;
    [SerializeField] private float gameLookAtPlayerDelay = 0.5f;

    [SerializeField] private float followSpeed = .25f;
    [SerializeField] private float zOffsetPlayer = -10f;
    [SerializeField] private float cameraHeight = 20f;

    [SerializeField] private float mapHeight = 100f;
    [SerializeField] private float zOffsetMap = -40f;

    private bool isDisplayingMap;

    private Vector3 followPlayerOffSet;
    private Vector3 mapOverviewOffset;
    private Vector3 cameraOffset;



    public void OnGameStarted()
    {
        StartCoroutine(OnGameHasStartedCoroutine());

    }


    public void OnDisplayMap()
    {
        if (isDisplayingMap)
        {
            cameraOffset = followPlayerOffSet;
            isDisplayingMap = false;
        }
        else
        {
            cameraOffset = mapOverviewOffset;
            isDisplayingMap = true;
        }
    }

    


    void Start()
    {
        player = GameObject.FindWithTag("Player");

        SetStartPositionIfGameStarted();

        followPlayerOffSet = new Vector3(0, cameraHeight, zOffsetPlayer);
        mapOverviewOffset = new Vector3(0, mapHeight, zOffsetMap);
        cameraOffset = followPlayerOffSet;
    }


    void LateUpdate()
    {
        if (player != null && gameIsStarted) // Player died
        {
            Vector3 playerPosition = player.transform.position;
            
            Vector3 newPosition = playerPosition + cameraOffset;

            this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeed * Time.deltaTime);
        }

    }


    private void SetStartPositionIfGameStarted()
    {
        if (MainMenu.gameHasStarted)
        {
            SetLookAtAngles();
            Vector3 startPosition = player.transform.position + followPlayerOffSet;
            startPosition.y = cameraHeight + player.transform.position.y;
            this.transform.position = startPosition;
        }
    }


    private void SetLookAtAngles()
    {
        this.transform.eulerAngles = new Vector3(60, 0, 0);
    }


    private IEnumerator OnGameHasStartedCoroutine()
    {
        yield return new WaitForSecondsRealtime(gameStartedDelay);


        float timeToLook = gameLookAtPlayerDelay;
        float passedTime = 0;
        Vector3 targetAngles = new Vector3(60, 0, 0);
        Vector3 targetLookRIght = new Vector3(0, 10, 0);
        Vector3 startAngles = this.transform.eulerAngles;

        while (passedTime < timeToLook * 2)
        {
            this.transform.eulerAngles = Vector3.Lerp(startAngles, targetLookRIght, passedTime / timeToLook);

            passedTime += Time.deltaTime;
            yield return null;
        }

        passedTime = 0;

        while (passedTime < timeToLook * 2)
        {
            this.transform.eulerAngles = Vector3.Lerp(targetLookRIght, startAngles, passedTime / timeToLook);

            passedTime += Time.deltaTime;
            yield return null;
        }

        passedTime = 0;

        while (passedTime < timeToLook)
        {
            this.transform.eulerAngles = Vector3.Lerp(startAngles, targetAngles, passedTime/timeToLook);
            passedTime += Time.deltaTime;

            if (passedTime/timeToLook > 0.7f)
            {
                gameIsStarted = true;
            }

            yield return null;
        }
    }
}
