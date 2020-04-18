using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour{

    public Transform StartPosition;
    public GameObject Player;
    public GameObject vCam;

    public void LoadPlayer() {
        var loadPosition = StartPosition.position;
        loadPosition.z = -.5f;

        Player = Instantiate(Player, loadPosition, Quaternion.identity);

        var cameraPosition = StartPosition.position;
        cameraPosition.z = -5f;

        var vcam = vCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        vcam.Follow = Player.transform;
    }
}
