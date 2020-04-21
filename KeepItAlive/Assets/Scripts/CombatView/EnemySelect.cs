using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour, ISelectable{
    public GameObject ArrowSprite;

    public void Start() {
    }

    public void Selected() {
        ArrowSprite.SetActive(true);
    }

    public void Unselected() {
        ArrowSprite.SetActive(false);
    }
}
