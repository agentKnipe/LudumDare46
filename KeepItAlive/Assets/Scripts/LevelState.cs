using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour{
    private bool _paused = false;

    public Transform StartPosition;
    public GameObject Player;
    public GameObject vCam;

    public GameObject PausePanel;
    public GameObject YouWinPanel;
    public GameObject DetailsPanel;

    public AudioSource AudioSource;
    public AudioClip EncounterStart;

    [Range(0, 0.01f)]
    public float DangerousTileSpawnRate;

    [Range(0, 0.001f)]
    public float RoadTileSpawnRate;

    public LevelState() {
        CharacterFightTrigger.HitDangerousTile += OnTriggerDangeriousSpawn;
        CharacterFightTrigger.HitRoadTile += OnTriggerRoadSpawn;
    }

    public void LoadPlayer() {
        var loadPosition = StartPosition.position;
        loadPosition.z = -.5f;

        Player = Instantiate(Player, loadPosition, Quaternion.identity);
        Player.GetComponent<CharacterController>().enabled = true;

        var cameraPosition = StartPosition.position;
        cameraPosition.z = -5f;

        var vcam = vCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        vcam.Follow = Player.transform;
    }

    private void Start() {
        Time.timeScale = 0f;
    }

    private void Update() {
        
    }

    public void HideDetails() {
        Time.timeScale = 1f;
        DetailsPanel.SetActive(false);
    }

    private void OnTriggerDangeriousSpawn() {
        var rand = Random.Range(0f, 1f);

        if(rand < DangerousTileSpawnRate) {
            SceneLoader.Instance.LoadScene("CombatView");
        }
    }

    private void OnTriggerRoadSpawn() {
        var rand = Random.Range(0f, 1f);

        if (rand < RoadTileSpawnRate) {
            Debug.Log("Road Tile");
        }
    }

}
