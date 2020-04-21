using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFightTrigger : MonoBehaviour{
    public delegate void PossibleSpawn();

    public static event PossibleSpawn HitDangerousTile;
    public static event PossibleSpawn HitRoadTile;

    public 

    // Update is called once per frame
    void Update(){
        var collider = Physics2D.OverlapCircle(transform.position, .5f);
        if (collider != null && (collider.CompareTag("DangerousTile") || collider.GetComponent<RoadType>()?.Type == -1)) {
            if(HitDangerousTile != null) {
                if (gameObject.GetComponent<CharacterWorldController>().IsWalking) {
                    HitDangerousTile();
                }
            }
        }
        else if (collider.gameObject.tag == "Finish") {
            SceneLoader.Instance.NextLevel();
        }
        else {
            if(HitRoadTile != null) {
                if (gameObject.GetComponent<CharacterWorldController>().IsWalking) {
                    HitRoadTile();
                }
            }
        }
    }
}
