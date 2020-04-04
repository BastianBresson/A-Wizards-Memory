using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int roomsSpawned;
    private int roomsCleared;

    [SerializeField] private int baseRoomSpawns = 2;

    [SerializeField] private GameObject startPlatformEnd;

    [SerializeField] private GameObject[] smallRooms;
    [SerializeField] private GameObject[] mediumRooms;
    [SerializeField] private GameObject[] largeRooms;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnRoomsCoroutine());
    }

    public void RoomCleared()
    {
        roomsCleared++;

        if (roomsCleared == roomsSpawned)
        {
            GameManager.Instance.AllRoomsClear();
        }
    }

    private IEnumerator spawnRoomsCoroutine()
    {
        int levelsCompleted = GameManager.Instance.LevelsCompleted;

        int roomLvlLower = 1;
        int roomLvlUpper = 1;

        if (levelsCompleted >= 5 && levelsCompleted < 10)
        {
            roomLvlUpper = 2;
        }
        else if (levelsCompleted >= 10)
        {
            roomLvlLower = 1;
            roomLvlUpper = 3;
        }
        
        int additionalRoomSpawns = GameManager.Instance.LevelsCompleted / 2;
        int roomSpawns = baseRoomSpawns + additionalRoomSpawns;

        GameObject end = startPlatformEnd;

        for (int i = 0; i <= roomSpawns; i++)
        {
            yield return null;

            int r = Random.Range(roomLvlLower, (roomLvlUpper + 1));

            GameObject roomToSpawn = RandomRoom(r);

            GameObject room = Instantiate(roomToSpawn);

            roomsSpawned++;

            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();

            Vector3 distance = end.transform.position - roomBehaviour.RoomStart.transform.position;

            room.transform.position += distance;

            end = roomBehaviour.RoomEnd;
        }
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
}
