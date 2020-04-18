using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public GameObject[] Tiles;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, Tiles.Length);
        Instantiate(Tiles[rand], transform.position, Quaternion.identity, transform);
    }
}
