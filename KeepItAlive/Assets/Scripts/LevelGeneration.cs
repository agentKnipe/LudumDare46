using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneration : MonoBehaviour{
    private bool _stopRoadBuild = false;
    private bool _stopBuildField = false;
    private bool _playerLoaded = false;

    private List<Vector3> _placedRoads = new List<Vector3>();
    private List<GameObject> _fieldTiles = new List<GameObject>();

    private GameObject[] _possibleLeftRight;
    private GameObject[] _possiblePreviousRightAfterDown;
    private GameObject[] _possiblePreviousLeftAfterDown;

    private GameObject[] _possiblePreviousDown;
    private GameObject[] _possibleDown;
    private GameObject[] _possiblePreviousDownAfterDown;

    private int _downCount = 0;

    public Transform LevelState;
    public Transform[] StartingPositions;
    public LayerMask Road;
    public GameObject ExitPoint;

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
    void Start() {
        SetupRoadsArrays();

        int randStart = Random.Range(0, StartingPositions.Length);
        Direction = Random.Range(1, 6);

        for (float x = MinX; x <= MaxX; x += MoveAmount) {
            for (float y = 15; y >= MinY; y -= MoveAmount) {
                var pos = new Vector3(x, y, 0);
                var newField = Instantiate(Field, pos, Quaternion.identity);

                _fieldTiles.Add(newField);
            }
        }

        transform.position = StartingPositions[randStart].position;

        LevelState.GetComponent<LevelState>().StartPosition = StartingPositions[randStart];
        CreateRoad(_possibleLeftRight);

        LevelState.GetComponent<LevelState>().LoadPlayer();

        if(GameState.Level == 0) {
            GameState.Level++;
        }

        if(GameState.Hero == null) {
            var knight = new Hero();
            knight.GenerateKnight();

            GameState.Hero = knight;
        }

        if(GameState.Princess == null) {
            GameState.Princess = new Princess();
        }
    }

    private void SetupRoadsArrays() {
        var leftRight = new List<GameObject>();
        var previousRightAfterDown = new List<GameObject>();
        var previousLeftAfterDown = new List<GameObject>();

        var previousDown = new List<GameObject>();
        var down = new List<GameObject>();
        var previousDownAfterDown = new List<GameObject>();

        for (int i = 0; i < Roads.Length; i++) {
            //possible left/right
            if (i == 0 || i == 1 || i == 2 || i == 3) {
                leftRight.Add(Roads[i]);
            }

            //possible right After down
            if(i == 3 || i == 2 || i == 5) {
                previousRightAfterDown.Add(Roads[i]);
            }

            //possible left after down
            if (i == 3 || i == 2 || i == 4) {
                previousLeftAfterDown.Add(Roads[i]);
            }

            //possible previous down
            if (i == 1 || i == 3 || i == 6 || i == 7) {
                previousDown.Add(Roads[i]);
            }

            //possible down
            if (i == 2 || i == 3 || i == 4 || i == 5 || i == 8) {
                down.Add(Roads[i]);
            }

            //possible down after down
            if(i == 3 || i == 8) {
                previousDownAfterDown.Add(Roads[i]);
            }
        }

        _possibleLeftRight = leftRight.ToArray();
        _possiblePreviousRightAfterDown = previousRightAfterDown.ToArray();
        _possiblePreviousLeftAfterDown = previousLeftAfterDown.ToArray();

        _possiblePreviousDown = previousDown.ToArray();
        _possibleDown = down.ToArray();
        _possiblePreviousDownAfterDown = previousDownAfterDown.ToArray();
    }

    private void Update() {
        if (!_stopRoadBuild) {
            Move();
        }
    }

    private void Move() {
        if(Direction == 1 || Direction == 2) { //move Right
            if(transform.position.x < MaxX) {
                if(_downCount > 0) { //ensure the previous down allows a right path also.
                    CreateRoad(_possiblePreviousRightAfterDown);
                }

                var newPos = new Vector2(transform.position.x + MoveAmount, transform.position.y);
                transform.position = newPos;

                CreateRoad(_possibleLeftRight);

                Direction = Random.Range(1, 6);

                if(Direction == 3) {
                    Direction = 2;
                }
                else if(Direction == 4) {
                    Direction = 5;
                }

                _downCount = 0;
            }
            else {
                Direction = 5;
            }
        }
        else if (Direction == 3 || Direction == 4) { //move Left
            if(transform.position.x > MinX) {
                if (_downCount > 0) { //ensure the previous down allows a right path also.
                    CreateRoad(_possiblePreviousLeftAfterDown);
                }

                var newPos = new Vector2(transform.position.x - MoveAmount, transform.position.y);
                transform.position = newPos;

                CreateRoad(_possibleLeftRight);

                Direction = Random.Range(3,6);

                _downCount = 0;
            }
            else {
                Direction = 5;
            }
        }
        else if (Direction == 5) { //move Down
            if(transform.position.y > MinY) {
                if(_downCount < 1) {
                    CreateRoad(_possiblePreviousDown);
                }
                else { // we have gone down more than once
                    CreateRoad(_possiblePreviousDownAfterDown);
                }

                var newPos = new Vector2(transform.position.x, transform.position.y - MoveAmount);
                transform.position = newPos;

                CreateRoad(_possibleDown);

                Direction = Random.Range(1, 6);
                _downCount++;
            }
            else {
                //Stop Generation
                _stopRoadBuild = true;
                PlaceExit();
            }
        }
    }

    private void CreateRoad(GameObject[] roads) {
        var fieldTile = _fieldTiles.Where(w => w.transform.position == transform.position).SingleOrDefault();

        if(fieldTile != null) {
            fieldTile.GetComponent<RoadType>().RoadDestroy();
            _fieldTiles.Remove(fieldTile);
        }

        var collision = Physics2D.OverlapCircle(transform.position, 1, Road);
        if (collision != null) {
            collision.GetComponent<RoadType>().RoadDestroy();

            _placedRoads.Remove(transform.position);
        }

        if (!_placedRoads.Contains(transform.position)) {
            var road = roads[Random.Range(0, roads.Length)];

            Instantiate(road, transform.position, Quaternion.identity);
            _placedRoads.Add(transform.position);
        }
    }

    private void PlaceExit() {
        var collision = Physics2D.OverlapCircle(transform.position, 1, Road);
        if(collision != null) {
            var roadType = collision.GetComponent<RoadType>();

            var spawnPoint = roadType.PossibleExits[Random.Range(0, roadType.PossibleExits.Length)].transform.position;
            spawnPoint.z = -.5f;

            Instantiate(ExitPoint, spawnPoint, Quaternion.identity);
        }
    }
}
