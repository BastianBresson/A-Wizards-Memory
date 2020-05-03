using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private float followSpeed = .25f;
    [SerializeField] private float zOffset = -10f;
    [SerializeField] private float cameraHeight = 20f;

    private Vector3 cameraOffset;

    void Start()
    {
        player = GameObject.FindWithTag("Player");


        Vector3 startPosition = player.transform.position + cameraOffset;
        startPosition.y = cameraHeight + player.transform.position.y;
        this.transform.position = startPosition;

        cameraOffset = new Vector3(0, cameraHeight, zOffset);
    }


    void Update()
    {
        if (player != null) // Player died
        {
            Vector3 playerPosition = player.transform.position;
            
            Vector3 newPosition = playerPosition + cameraOffset;

            this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeed * Time.deltaTime);
        }

    }
}
