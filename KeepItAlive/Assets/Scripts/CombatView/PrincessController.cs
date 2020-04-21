using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessController : MonoBehaviour{
    public GameObject PrincessPrefab;
    public Princess Princess;

    public void Load(Princess princess) {
        this.Princess = princess;
    }

    // Start is called before the first frame update
    void Start(){
        var loadPosition = transform.position;
        loadPosition.y = -11.78f;

        var loadRotation = Quaternion.Euler(new Vector3(20f,-36f,-7.5f));

        Instantiate(PrincessPrefab, loadPosition, loadRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
