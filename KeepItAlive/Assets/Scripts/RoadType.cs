using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadType : MonoBehaviour
{
    public int Type;

   public void RoadDestroy() {
        Destroy(gameObject);
    }
}
