using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int baseRoomSpawns = 0;

    private int roomsSpawned;
    private int roomsCleared;

    private int levelsCompleted;

    [SerializeField] private GameObject startPlatformEnd = default;
    [SerializeField] private GameObject endPlatform = default;
    [SerializeField] private GameObject endPlatformStart = default;

    [SerializeField] private GameObject[] smallRooms = default;
    [SerializeField] private GameObject[] mediumRooms = default;
    [SerializeField] private GameObject[] largeRooms = default;


    private void Start()
    {
        levelsCompleted = GameManager.Instance.LevelsCompleted;

        StartCoroutine(SpawnRoomsCoroutine());
    }

    public void RoomCleared()
    {
        roomsCleared++;

        if (roomsCleared == roomsSpawned)
        {
            LevelCompleted();
        }
    }


    private void LevelCompleted()
    {
        
    }


    private IEnumerator SpawnRoomsCoroutine()
    {
        int roomLvlLower = RoomLevelLowerBound();
        int roomLvlUpper = RoomLevelUpperBound();
        
        int roomsToSpawns = NumberOfRoomsToSpawn();

        GameObject end = startPlatformEnd;

        for (int i = 0; i <= roomsToSpawns; i++)
        {
            yield return null;

            int roomLevel = Random.Range(roomLvlLower, (roomLvlUpper + 1));    
            GameObject roomToSpawn = RandomRoom(roomLevel);
            GameObject room = SpawnRoom(roomToSpawn);

            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();

            PlaceRoom(room, roomBehaviour.RoomStart, end);

            end = roomBehaviour.RoomEnd;
        }

        PlaceRoom(endPlatform, endPlatformStart, end);
    }

    private int RoomLevelLowerBound()
    {
        int lowerbound = levelsCompleted / 10;
        lowerbound = Mathf.Clamp(lowerbound, 1, 2);
        return lowerbound;
    }


    private int RoomLevelUpperBound()
    {
        int upperbound = levelsCompleted / 5;
        upperbound = Mathf.Clamp(upperbound, 1, 3);
        return upperbound;
    }


    private int NumberOfRoomsToSpawn()
    {
        int additionalRoomSpawns = GameManager.Instance.LevelsCompleted / 2;
        int roomsToSpawns = baseRoomSpawns + additionalRoomSpawns;
        return roomsToSpawns;
    } 


    private GameObject RandomRoom(int roomLevel)
    {
        int r = 0;
        switch (roomLevel)
        {
            case 1:
                r = Random.Range(0, smallRooms.Length);
                return smallRooms[r];
            case 2:
                r = Random.Range(0, mediumRooms.Length);
                return mediumRooms[r];
            case 3:
                r = Random.Range(0, largeRooms.Length);
                return largeRooms[r];
            default:
                return null;
        }
    }


    private GameObject SpawnRoom(GameObject roomToSpawn)
    {
        GameObject spawnedRoom = Instantiate(roomToSpawn);
        roomsSpawned++;
        return spawnedRoom;
    }


    private void PlaceRoom(GameObject room, GameObject roomStart, GameObject previouslyRoomEnd)
    {
        Vector3 distance = DistanceFromEndToStart(previouslyRoomEnd, roomStart);
        room.transform.position += distance;
    }


    private Vector3 DistanceFromEndToStart(GameObject roomEnd, GameObject roomStart)
    {
        Vector3 distance = roomEnd.transform.position - roomStart.transform.position;
        return distance;
    }
}
