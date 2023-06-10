using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCountControl : MonoBehaviour
{
    private GameObject circleControl;

    private void Start()
    {
        circleControl = ObjectManager.Instance.CircleParent;
    }
    public void ObstacleControl()
    {
        circleControl.GetComponent<DrillController>().ChangeTerrain();
    }
}
