using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject Player;

    [SerializeField] private float followSpeed = .5f;
    [SerializeField] private const float zOffset = -10f;

    private Vector3 cameraOffset = new Vector3(0f, 0f, zOffset);


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = Player.transform.position;
        float currentHeight = this.transform.position.y;
        Vector3 newPosition = playerPosition + cameraOffset;
        newPosition.y = currentHeight;

        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, followSpeed * Time.deltaTime);
    }
}
