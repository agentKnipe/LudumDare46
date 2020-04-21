using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public GameObject[] Tiles;

    public GameObject[] Sceenery;

    // Start is called before the first frame update
    void Start()
    {
        var rand = Random.Range(0, Tiles.Length);
        var randTrees = Random.Range(0, Sceenery.Length);

        var position = transform.position;
        position.x -= .5f;
        position.y -= .5f;

        var tile = Tiles[rand];
        tile.tag = "DangerousTile";

        Instantiate(tile, position, Quaternion.identity, transform);

        if(Sceenery.Length > 0) {
            if (Random.value > .95) {
                var treePosition = transform.position;
                treePosition.z = -.1f;
                Instantiate(Sceenery[randTrees], treePosition, Quaternion.identity, transform);
            }
        }
    }
}
