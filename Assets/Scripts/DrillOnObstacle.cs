using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeObstacle
{
    Wood,
    Stone,
    Space,
    Finish,
    Stop,
}

public class DrillOnObstacle : MonoBehaviour
{
    [SerializeField] private TypeObstacle typeObstacle;

    private GameObject firstObject;
    private GameObject circleParent;
    private GameObject terrainObject;
    private GameObject spearParent;

    private bool bigCollider = true;

    private bool inObject = true;

    private SpearControl spearControlScript;

    private PlayerMovementController playerMovementController;

    public TypeObstacle TypeObstacle { get => typeObstacle; set => typeObstacle = value; }

    private void Start()
    {
        firstObject = ObjectManager.Instance.SpearParent.GetChild(0).gameObject;
        circleParent = ObjectManager.Instance.CircleParent;
        spearParent = ObjectManager.Instance.SpearParent;
        terrainObject = transform.parent.GetChild(0).gameObject;
        spearControlScript = spearParent.GetComponent<SpearControl>();
        playerMovementController = firstObject.GetComponent<PlayerMovementController>();
    }



    private void OnTriggerEnter(Collider other)
    {

        playerMovementController.GetTypeObstacle(gameObject);
        spearControlScript.GetTypeObstacle(gameObject);

        if (other.HasComponent<ObstacleCountControl>() && bigCollider)
        {

            Physics.IgnoreCollision(other, GetComponent<Collider>());
            other.gameObject.GetComponent<ObstacleCountControl>()?.ObstacleControl();
            bigCollider = false;
        }
        else
        {
            Run.After(0.3f, () =>
            {
                spearParent.GetComponent<SpearControl>().Control = true;
            });
            if (!firstObject.GetComponent<PlayerMovementController>().InObstacle)
            {

                playerMovementController.DrillInObstacle();
                playerMovementController.GetTypeObstacle(gameObject);
                spearControlScript.GetTypeObstacle(gameObject);
                StartCoroutine(spearControlScript.Vibration());
                StartCoroutine(spearControlScript.ExplodeDrill());
                circleParent.GetComponent<DrillController>()?.StackNewDrill();
            }
        }

    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, firstObject.transform.position);
        if(dist < 25 && !terrainObject.activeInHierarchy)
        {
            terrainObject.SetActive(true);
        }
        else if(dist > 25 && terrainObject.activeInHierarchy)
        {
            terrainObject.SetActive(false);
        }

    }
    private void OnTriggerExit(Collider other)
    {

        //Destroy(GetComponent<BoxCollider>());
        Run.After(0.1f, () =>
        {
            spearControlScript.Control = false;
        });

        Run.After(0.3f, () =>
        {
            if (!spearControlScript.Control)
            {
                if (typeObstacle == TypeObstacle.Stone)
                {
                    playerMovementController.ExitObstacle(TypeObstacle.Wood);
                    spearControlScript.ExitObstacle(TypeObstacle.Wood);
                }
                else if (typeObstacle == TypeObstacle.Wood)
                {
                    playerMovementController.ExitObstacle(TypeObstacle.Space);
                    spearControlScript.ExitObstacle(TypeObstacle.Space);
                    playerMovementController.InObstacle = false;
                    circleParent.GetComponent<DrillController>().ExitObstacleTransScript();
                }
            }
        });


    }
}
