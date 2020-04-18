using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour{
    private bool _stopRoadBuild = false;
    private bool _stopBuildField = false;
    private bool _playerLoaded = false;
    private List<Vector3> _placedRoads = new List<Vector3>();


    public Transform LevelState;
    public Transform[] StartingPositions;
    public LayerMask Road;

    /*
     * 0 - left to right
     * 1 - left right bottom
     * 2 - left right top
     * 3 - cross roads
     * 4 - turn up left
     * 5 - turn up right
     * 6 - turn down right
     * 7 - turn down left
     * 8 - vertical road
     */

    public GameObject[] Roads;
    public GameObject Field;

    public int Direction;
    public float MoveAmount;

    public float MinX;
    public float MaxX;
    public float MinY;

    private float timeBetweenRoom;
    public float startTimeBetweenRoom = .25f;

    // Start is called before the first frame update
    void Start()
    {
        int randStart = Random.Range(0, StartingPositions.Length);
        Direction = Random.Range(1, 6);

        transform.position = StartingPositions[randStart].position;

        LevelState.GetComponent<LevelState>().StartPosition = StartingPositions[randStart];
        CreateRoad(Roads[0]);

        LevelState.GetComponent<LevelState>().LoadPlayer();
    }

    private void Update() {
        if (!_stopBuildField) {
            for (float x = MinX; x <= MaxX; x += MoveAmount) {
                for (float y = 15; y >= MinY; y -= MoveAmount) {
                    var pos = new Vector3(x, y, 0);
                    Instantiate(Field, pos, Quaternion.identity);
                }
            }

            _stopBuildField = true;
        }

        if (!_stopRoadBuild) {
            Move();
        }
    }

    private void Move() {
        if(Direction == 1 || Direction == 2) { //move Right
            if(transform.position.x < MaxX) {
                var newPos = new Vector2(transform.position.x + MoveAmount, transform.position.y);
                transform.position = newPos;

                var randRoad = Roads[Random.Range(0, Roads.Length)];
                CreateRoad(randRoad);

                Direction = Random.Range(1, 6);

                if(Direction == 3) {
                    Direction = 2;
                }
                else if(Direction == 4) {
                    Direction = 5;
                }
            }
            else {
                Direction = 5;
            }
        }
        else if (Direction == 3 || Direction == 4) { //move Left
            if(transform.position.x > MinX) {
                var newPos = new Vector2(transform.position.x - MoveAmount, transform.position.y);
                transform.position = newPos;

                var randRoad = Roads[Random.Range(0, Roads.Length)];
                CreateRoad(randRoad);

                Direction = Random.Range(3, 6);
            }
            else {
                Direction = 5;
            }
        }
        else if (Direction == 5) { //move Down
            if(transform.position.y > MinY) {
                var roadDetection = Physics2D.OverlapCircle(transform.position, 1, Road);
                if(roadDetection.GetComponent<RoadType>().Type != 1 && roadDetection.GetComponent<RoadType>().Type != 3) {
                    roadDetection.GetComponent<RoadType>().RoadDestroy();
                    _placedRoads.Remove(transform.position);

                    var randBottomRoad = Random.Range(1, 4);
                    if(randBottomRoad == 2) {
                        randBottomRoad = 1;
                    }
                    var newRoad = Roads[randBottomRoad];
                    CreateRoad(newRoad);
                }

                var newPos = new Vector2(transform.position.x, transform.position.y - MoveAmount);
                transform.position = newPos;

                var randRoad = Roads[Random.Range(2, 4)];
                CreateRoad(randRoad);

                Direction = Random.Range(1, 6);
            }
            else {
                //Stop Generation
                _stopRoadBuild = true;
            }
        }
    }

    private void CreateRoad(GameObject road) {
        if (!_placedRoads.Contains(transform.position)) {
            var collider = Physics2D.OverlapCircle(transform.position, 1, LayerMask.NameToLayer("Terrain"));
            if (collider != null) {
                collider.GetComponent<RoadType>().RoadDestroy();
            }

            Instantiate(road, transform.position, Quaternion.identity);
            _placedRoads.Add(transform.position);
        }
    }
}
