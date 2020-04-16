using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private float followSpeed = .5f;
    [SerializeField] private const float zOffset = -10f;
    [SerializeField] private float cameraHeight = 20f;

    private Vector3 cameraOffset = new Vector3(0f, 0f, zOffset);


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");


        Vector3 startPosition = player.transform.position + cameraOffset;
        startPosition.y = cameraHeight + player.transform.position.y;
        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 newPosition = playerPosition + cameraOffset;
        newPosition.y = cameraHeight + player.transform.position.y;

        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeed * Time.deltaTime);
    }
}
