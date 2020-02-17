using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class LookMinigameHandler : MonoBehaviour
{
    public GameObject startGameText;
    public GameObject selectThisItem;
    public GameObject PathParent;
    private List<Transform> pathObjects;
    private bool closeEnoughToStart = false;
    private bool started;
    private bool movingLeft = false;
    private bool movingRight = false;

    private List<LookObject> lookObjects;
    private List<Transform> lookObjectCameraPositions;

    public int selectedObject = -1;
    public int lookedAtObject = -1;
    public int pathIndex;

    // Start is called before the first frame update
    void Start()
    {
        lookObjects = GetComponentsInChildren<LookObject>().ToList();
        lookObjectCameraPositions = lookObjects.Select(lo => lo.CameraPosition.transform).ToList();
        pathObjects = PathParent.GetComponentsInChildren<Transform>().Where(p => p.gameObject != PathParent).ToList();

        foreach(Transform trans in pathObjects)
        {
            trans.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (JournalActivator.IsPaused == false)
        {
            startGameText.SetActive(closeEnoughToStart && !started);

            if (closeEnoughToStart && Input.GetKeyDown(KeyCode.E))
            {
                started = true;

                DetachCamera.Detach();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                LookObject closestObject = lookObjects.OrderBy(o => Vector3.Distance(o.transform.position, Camera.main.transform.position)).First();
                lookedAtObject = lookObjects.IndexOf(closestObject);
                pathIndex = pathObjects.IndexOf(closestObject.CameraPosition.transform);
            }

            selectThisItem.SetActive(started);

            if (started)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SelectCurrentObject();
                }

                if (movingLeft || movingRight)
                {
                    float stepAmt = 5 * Time.deltaTime;
                    if (Vector3.Distance(Camera.main.transform.position, pathObjects[pathIndex].transform.position) < stepAmt)
                    {
                        if (lookObjectCameraPositions.Contains(pathObjects[pathIndex]))
                        {
                            Camera.main.transform.position = pathObjects[pathIndex].transform.position;
                            movingLeft = false;
                            movingRight = false;
                        }
                        else
                        {
                            if (movingLeft)
                            {
                                pathIndex = pathIndex > 0 ? pathIndex - 1 : pathObjects.Count - 1;
                            }
                            else if (movingRight)
                            {
                                pathIndex = pathIndex + 1 < pathObjects.Count ? pathIndex + 1 : 0;
                            }
                        }
                    }

                    Vector3 newPos = Vector3.MoveTowards(Camera.main.transform.position, pathObjects[pathIndex].transform.position, stepAmt);
                    Camera.main.transform.position = newPos;
                    //Camera.main.transform.LookAt(lookObjects[lookedAtObject].transform);

                    //Vector3 perp = Vector3.Cross(travelDir, Vector3.up).normalized;
                    //Camera.main.transform.LookAt(Camera.main.transform.position + perp, Vector3.up);

                }

                Vector3 lookDir = (lookObjects[lookedAtObject].transform.position - Camera.main.transform.position);
                Quaternion toRotation = Quaternion.LookRotation(lookDir);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, toRotation, 5 * Time.deltaTime);

                //Camera.main.transform.LookAt(lookObjects[lookedAtObject].transform.position);
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

                    int tempIndexRight = pathIndex + 1 < pathObjects.Count ? pathIndex + 1 : 0;
                    int tempIndexLeft = pathIndex > 0 ? pathIndex - 1 : pathObjects.Count - 1;
                    if (rightPressed)
                    {
                        if (Mathf.Sign(pathObjects[tempIndexRight].localPosition.x - pathObjects[tempIndexLeft].localPosition.x) == Mathf.Sign(Camera.main.transform.right.x))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else
                        {
                            pathIndex = tempIndexLeft;
                            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;

                            movingLeft = true;
                        }
                    }
                    else if (leftPressed)
                    {
                        if (Mathf.Sign(pathObjects[tempIndexRight].localPosition.x - pathObjects[tempIndexLeft].localPosition.x) != Mathf.Sign(Camera.main.transform.right.x))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else
                        {
                            pathIndex = tempIndexLeft;
                            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;

                            movingLeft = true;
                        }
                    }
                }
            }
        }
    }
    
    public void SelectCurrentObject()
    {
        selectedObject = lookedAtObject;
        selectThisItem.SetActive(false);
        DetachCamera.Reattach();
        Camera.main.transform.localPosition = Vector3.up * 0.6f;
        started = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        closeEnoughToStart = true;
    }
    private void OnTriggerExit(Collider other)
    {
        closeEnoughToStart = false;
    }
}
