using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillController : MonoBehaviour
{

    [SerializeField, ReadOnly] private List<RuntimeCircleClipper> clipper;
    [SerializeField, ReadOnly] private List<DestructibleTerrain> terrains;
    [SerializeField, ReadOnly] private List<GameObject> woodList;
    [SerializeField, ReadOnly] private int woodOrder = 0;
    private List<GameObject> circleParent;
    private GameObject woodParent;
    private GameObject firstDrill;
    private int thisChild;
    private float changeTime = 0;

    public int WoodOrder { get => woodOrder; set => woodOrder = value; }

    void Start()
    {
        GameManager.Instance.GameStart += GameStart;
    }

    private void GameStart()
    {
        clipper = GetComponentsInChildren<RuntimeCircleClipper>().ToList();
        terrains = FindObjectsOfType<DestructibleTerrain>().ToList();
        woodParent = ObjectManager.Instance.WoodParent;
        woodList.AddRange(woodParent.transform.GetChildren().ToList());
        firstDrill = ObjectManager.Instance.SpearParent.GetChild(1).gameObject;
        thisChild = transform.childCount;

        for (int i = 0; i < thisChild; i++)
        {
            if (woodList.Count > WoodOrder + i)
            {
                clipper[i].terrain = woodList[WoodOrder + i].GetChild(0).GetComponent<DestructibleTerrain>();

            }
        }
    }

    public void ChangeTerrain()
    {
        Run.After(0.5f, () =>
        {
            WoodOrder++;
        });
    }


    public void ExitObstacleTransScript()
    {
        firstDrill = ObjectManager.Instance.SpearParent.GetLastChild().gameObject;
        Run.After(15f, () =>
        {
            if (woodList.Count > WoodOrder)
            {
                clipper[0].terrain = woodList[WoodOrder].GetChild(0).GetComponent<DestructibleTerrain>();
            }
            
        });

        Run.After(1, () =>
        {
            for (int i = 1; i < thisChild; i++)
            {

                if (woodList.Count - WoodOrder < 7)
                {
                    clipper[i].terrain = woodList[woodList.Count - i].GetChild(0).GetComponent<DestructibleTerrain>();
                }
                else if (woodList.Count > WoodOrder + i)
                {
                    clipper[i].terrain = woodList[WoodOrder + i - 1].GetChild(0).GetComponent<DestructibleTerrain>();
                }
            }
            
        });

        
    }

    public void SetNewDrill()
    {
        for (int i = 0; i < clipper.Count-1; i++)
        {
            clipper[i].TerrainChangeParent();
        }
    }

    public void StackNewDrill()
    {
        for (int i = 0; i < clipper.Count - 1; i++)
        {
            clipper[i].StackTerrainChangeParent();
        }
    }
}




