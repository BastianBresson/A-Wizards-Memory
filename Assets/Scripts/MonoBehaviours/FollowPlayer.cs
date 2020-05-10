using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;

    private Vector3 offsetPosition;
    private Vector3 newPosition;

    [SerializeField] float zOffset = default;
    [SerializeField] float yOffset = default;
    [SerializeField] float xOffset = default;
    [SerializeField] bool changeOffsetsOnRuntime = default;
    [SerializeField] float lerpSpeed = default;


    private void Start()
    {
        findPlayer();
        setInitialPositions();
    }


    private void Update()
    {
        updateOffsetPostionIfEnabled();
        followPlayer();
    }


    private void findPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }


    private void setInitialPositions()
    {
        offsetPosition = new Vector3(xOffset, yOffset, zOffset);
        newPosition = player.transform.position + offsetPosition;
    }


    private void updateOffsetPostionIfEnabled()
    {
        if (changeOffsetsOnRuntime)
        {
            offsetPosition.x = xOffset;
            offsetPosition.y = yOffset;
            offsetPosition.z = zOffset;
        }
    }


    private void followPlayer()
    {
        Vector3 lerpFromPosition = this.transform.position;
        Vector3 lerpToPosition = player.transform.position + offsetPosition;

        Vector3 lerpPosition = Vector3.Lerp(lerpFromPosition, lerpToPosition, lerpSpeed * Time.deltaTime);
        this.transform.position = lerpPosition;

    }
}
