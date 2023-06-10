using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private GameStatus gameStatus;
    private GameObject spearParent;
    private GameObject followObject;
    private Transform circleParent;
    private Camera mainCamera;

    private ObjectManager objectManager;

    [SerializeField] private bool stepControl = true;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        objectManager = ObjectManager.Instance;

        gameStatus = gameManager.GameStatus;
        spearParent = objectManager.SpearParent;
        followObject = objectManager.SpearParent.GetChild(0).gameObject;
        mainCamera = objectManager.MainCamera;
        circleParent = objectManager.CircleParent.transform;
        objectManager.FinishObject = gameObject;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stepControl)
        {
            mainCamera.GetComponent<CameraMovement>().StepControlEnter();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(stepControl)
        {
            GetComponent<BoxCollider>().enabled = false;

            gameManager.GameStatus = GameStatus.Finish1;

            spearParent.GetComponent<SpearControl>().FinishIsOpen1(gameObject);
            mainCamera.GetComponent<CameraMovement>().FinishPos();
            for (int i = 0; i < circleParent.childCount; i++)
            {
                circleParent.GetChild(i).GetComponent<RuntimeCircleClipper>().FinishControl();
            }
            Run.After(5, () => {
                GetComponent<BoxCollider>().enabled = true;
            });
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;

            mainCamera.GetComponent<CameraMovement>().StepControl(gameObject);

            spearParent.GetComponent<SpearControl>().StepControl(gameObject);
        }
        
    }

}
