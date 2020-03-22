using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class FlatLookMinigameHandler : MonoBehaviour
{
    //public GameObject startGameText;
    public GameObject selectThisItem;
        
    private bool started;
    private bool movingLeft = false;
    private bool movingRight = false;

    public Camera cam;

    public Transform lookObjectParent;
    public Transform allObjects;
    public Transform rotateAroundPoint;
    private List<GameObject> lookObjects = new List<GameObject>();

    public int selectedObject = 0;

    public float turnSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lookObjectParent.childCount; i++)
        {
            lookObjects.Add(lookObjectParent.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        selectThisItem.SetActive(started);

        if (started)
        {
            //cam.transform.LookAt(lookObjects[lookedAtObject].transform.position);
            if (!movingLeft && !movingRight)
            {
                bool rightPressed = false;
                bool leftPressed = false;

                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    rightPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    leftPressed = true;
                }

                if (rightPressed)
                {
                    movingRight = true;
                    StartCoroutine(MoveRight());
                }
                else if (leftPressed)
                {
                    movingLeft = true;
                    StartCoroutine(MoveLeft());
                }
            }
        }
    }

    IEnumerator MoveRight()
    {
        Vector3 euler = lookObjects[selectedObject].transform.eulerAngles;
        Vector3 ogPos = lookObjects[selectedObject].transform.localPosition;

        int nextObject = selectedObject + 1 < lookObjects.Count ? selectedObject + 1 : 0;
        bool haveShifted = false;
        for (float i = 0; i < turnSpeed; i += Time.deltaTime)
        {
            lookObjects[selectedObject].transform.RotateAround(rotateAroundPoint.transform.position, Vector3.up, 360f / turnSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();

            if (haveShifted == false && i != 0)
            {
                float dis = 0.01f * lookObjects.Count;

                for (int j = nextObject; j < lookObjects.Count; j++)
                {
                    lookObjects[j].transform.localPosition = new Vector3(lookObjects[j].transform.localPosition.x, dis, lookObjects[j].transform.localPosition.z);
                    dis -= 0.01f;
                }
                for (int k = 0; k < nextObject; k++)
                {
                    lookObjects[k].transform.localPosition = new Vector3(lookObjects[k].transform.localPosition.x, dis, lookObjects[k].transform.localPosition.z);
                    dis -= 0.01f;
                }


                haveShifted = true;
            }
        }

        lookObjects[selectedObject].transform.eulerAngles = new Vector3(ogPos.x, 0.01f, ogPos.z);
        lookObjects[selectedObject].transform.eulerAngles = euler;

        selectedObject = nextObject;

        movingRight = false;
    }

    IEnumerator MoveLeft()
    {
        Vector3 euler = lookObjects[selectedObject].transform.eulerAngles;
        Vector3 ogPos = lookObjects[selectedObject].transform.localPosition;

        int nextObject = selectedObject > 0 ? selectedObject - 1 : lookObjects.Count - 1;
        selectedObject = nextObject;

        bool haveShifted = false;
        float dis;
        for (float i = 0; i < turnSpeed; i += Time.deltaTime)
        {
            lookObjects[selectedObject].transform.RotateAround(rotateAroundPoint.transform.position, Vector3.up, -360f / turnSpeed * Time.deltaTime);
            if (/*haveShifted == false &&*/ i >= turnSpeed - Time.deltaTime * 2)
            {
                dis = 0.01f * lookObjects.Count;

                for (int j = nextObject; j < lookObjects.Count; j++)
                {
                    lookObjects[j].transform.localPosition = new Vector3(lookObjects[j].transform.localPosition.x, dis, lookObjects[j].transform.localPosition.z);
                    dis -= 0.01f;
                }
                for (int k = 0; k < nextObject; k++)
                {
                    lookObjects[k].transform.localPosition = new Vector3(lookObjects[k].transform.localPosition.x, dis, lookObjects[k].transform.localPosition.z);
                    dis -= 0.01f;
                }


                haveShifted = true;
            }
            yield return new WaitForEndOfFrame();
        }
        lookObjects[selectedObject].transform.eulerAngles = euler;
        lookObjects[selectedObject].transform.localPosition = new Vector3(ogPos.x, 0.01f * lookObjects.Count, ogPos.z);
        
        dis = 0.01f * lookObjects.Count;

        for (int j = nextObject; j < lookObjects.Count; j++)
        {
            lookObjects[j].transform.localPosition = new Vector3(lookObjects[j].transform.localPosition.x, dis, lookObjects[j].transform.localPosition.z);
            dis -= 0.01f;
        }
        for (int k = 0; k < nextObject; k++)
        {
            lookObjects[k].transform.localPosition = new Vector3(lookObjects[k].transform.localPosition.x, dis, lookObjects[k].transform.localPosition.z);
            dis -= 0.01f;
        }


        movingLeft = false;
    }

    public void StartMatchingGame()
    {
        allObjects.transform.eulerAngles = Vector3.right * 90;

        started = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void SelectCurrentObject()
    {
        allObjects.transform.eulerAngles = Vector3.zero;

        selectThisItem.SetActive(false);
        started = false;

        GetComponent<Taggable>()._interacting = false;
    }
}
