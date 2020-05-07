using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;

public class LookMinigameHandler : MonoBehaviour
{
    //public GameObject startGameText;
    public GameObject selectThisItem;
    public Image item;
    public GameObject PathParent;
    public GameObject LabelText;
    protected List<Transform> pathObjects;
    protected bool started;
    protected bool movingLeft = false;
    protected bool movingRight = false;

    public Camera cam;

    protected List<LookObject> lookObjects;
    protected List<Transform> lookObjectCameraPositions;

    public int selectedObject = 0;
    public int lookedAtObject = 0;
    public int correctAnswer;
    public TextAsset correctText;
    public TextAsset incorrectText;
    public int pathIndex;

    public bool lookAtTarget = true;

    // Start is called before the first frame update
    protected void Start()
    {
        fpsplayer = FindObjectOfType<RigidbodyFirstPersonController>();

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
        HandleMovement();

        DisplayLabelIfNecessary();
    }

    public virtual void HandleMovement()
    {
        if (action == null)
        {

            selectThisItem.SetActive(started);

            if (started)
            {
                //if (Input.GetKeyDown(KeyCode.Return))
                //{
                //    SelectCurrentObject();
                //}

                if (movingLeft || movingRight)
                {
                    float stepAmt = 5 * Time.deltaTime;
                    if (Vector3.Distance(cam.transform.position, pathObjects[pathIndex].transform.position) < stepAmt)
                    {
                        if (lookObjectCameraPositions.Contains(pathObjects[pathIndex]))
                        {
                            cam.transform.position = pathObjects[pathIndex].transform.position;
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

                    Vector3 newPos = Vector3.MoveTowards(cam.transform.position, pathObjects[pathIndex].transform.position, stepAmt);
                    cam.transform.position = newPos;
                    //cam.transform.LookAt(lookObjects[lookedAtObject].transform);

                    //Vector3 perp = Vector3.Cross(travelDir, Vector3.up).normalized;
                    //cam.transform.LookAt(cam.transform.position + perp, Vector3.up);

                }

                Debug.Log("look objects: " + lookObjects.Count + " look at object " + lookedAtObject);
                Vector3 lookDir = (lookObjects[lookedAtObject].transform.position - cam.transform.position);
                Quaternion toRotation = Quaternion.LookRotation(lookDir);
                if (lookAtTarget)
                {
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, toRotation, 5 * Time.deltaTime);
                }

                //cam.transform.LookAt(lookObjects[lookedAtObject].transform.position);
                if (!movingLeft && !movingRight)
                {
                    bool rightPressed = false;
                    bool leftPressed = false;
                    bool upPressed = false;
                    bool downPressed = false;

                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    {
                        rightPressed = true;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    {
                        leftPressed = true;
                    }
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    {
                        upPressed = true;
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    {
                        downPressed = true;
                    }

                    int tempIndexRight = pathIndex + 1 < pathObjects.Count ? pathIndex + 1 : 0;
                    int tempIndexLeft = pathIndex > 0 ? pathIndex - 1 : pathObjects.Count - 1;

                    if (rightPressed)
                    {
                        if (pathObjects[tempIndexRight].localPosition.x - pathObjects[pathIndex].localPosition.x != 0 &&
                            Mathf.Sign(pathObjects[tempIndexRight].localPosition.x - pathObjects[tempIndexLeft].localPosition.x) == Mathf.Sign(cam.transform.right.x))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else if (pathObjects[tempIndexLeft].localPosition.x - pathObjects[pathIndex].localPosition.x != 0)
                        {
                            pathIndex = tempIndexLeft;
                            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;

                            movingLeft = true;
                        }
                    }
                    if (leftPressed)
                    {
                        if (pathObjects[tempIndexRight].localPosition.x - pathObjects[pathIndex].localPosition.x != 0 &&
                            Mathf.Sign(pathObjects[tempIndexRight].localPosition.x - pathObjects[tempIndexLeft].localPosition.x) != Mathf.Sign(cam.transform.right.x))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else if (pathObjects[tempIndexLeft].localPosition.x - pathObjects[pathIndex].localPosition.x != 0)
                        {
                            pathIndex = tempIndexLeft;
                            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;

                            movingLeft = true;
                        }
                    }
                    if (upPressed || downPressed)
                        Debug.Log(pathObjects[tempIndexRight].localPosition.z - pathObjects[tempIndexLeft].localPosition.z + " " + pathObjects[tempIndexRight].localPosition + " " + pathObjects[tempIndexLeft].localPosition);
                    if (upPressed)
                    {
                        if (pathObjects[tempIndexRight].localPosition.z - pathObjects[pathIndex].localPosition.z != 0 &&
                            Mathf.Sign(pathObjects[tempIndexRight].localPosition.z - pathObjects[tempIndexLeft].localPosition.z) == Mathf.Sign(cam.transform.forward.z))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else if (pathObjects[tempIndexLeft].localPosition.z - pathObjects[pathIndex].localPosition.z != 0)
                        {
                            pathIndex = tempIndexLeft;
                            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;

                            movingLeft = true;
                        }
                    }
                    if (downPressed)
                    {
                        if (pathObjects[tempIndexRight].localPosition.z - pathObjects[pathIndex].localPosition.z != 0 &&
                            Mathf.Sign(pathObjects[tempIndexRight].localPosition.z - pathObjects[tempIndexLeft].localPosition.z) != Mathf.Sign(cam.transform.forward.z))
                        {
                            pathIndex = tempIndexRight;
                            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;

                            movingRight = true;
                        }
                        else if (pathObjects[tempIndexLeft].localPosition.z - pathObjects[pathIndex].localPosition.z != 0)
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

    public void DisplayLabelIfNecessary()
    {
        if (started && string.IsNullOrEmpty(lookObjects[lookedAtObject].Label) == false)
        {
            LabelText.SetActive(true);
            LabelText.GetComponentInChildren<TextMeshProUGUI>().text = lookObjects[lookedAtObject].Label;
        }
        else
        {
            LabelText.SetActive(false);
        }
    }

    public void StartMatchingGame(MinigameListing details)
    {
        DialogueController = FindObjectOfType<DialogueController>();

        selectThisItem.gameObject.SetActive(true);
        gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        cachedDetails = details;

        StartCoroutine(StartGame(details));
    }

    MinigameListing cachedDetails;

    protected IEnumerator StartGame(MinigameListing details)
    {
        if (PlayerStateTracker.startedMatchingMinigame == false)
        {
            yield return WaitForDialog(FindObjectOfType<PlayerStateTracker>().MatchingMinigameTutorialText);
            PlayerStateTracker.startedMatchingMinigame = true;
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
        }

        started = true;


        if (selectedObject == -1)
        {
            LookObject closestObject = lookObjects.OrderBy(o => Vector3.Distance(o.transform.position, cam.transform.position)).First();
            lookedAtObject = lookObjects.IndexOf(closestObject);
            pathIndex = pathObjects.IndexOf(closestObject.CameraPosition.transform);
        }

        Debug.Log("OBJECTS: " + pathObjects.Count);
        Debug.Log("index: " + pathIndex);

        GetComponent<CameraLerper>().Play(pathObjects[pathIndex].transform.position);
        HandleMinigameDetails(details);
    }

    public virtual void HandleMinigameDetails(MinigameListing details)
    {
        correctAnswer = details.correctIndex;
        correctText = details.correct;
        incorrectText = details.incorrect;
        item.sprite = details.objectMatch;
    }

    protected DialogueController DialogueController;
    protected Coroutine action;
    public RigidbodyFirstPersonController fpsplayer;
    public void SelectCurrentObject()
    {
        if (lookedAtObject == correctAnswer)
        {
            action = StartCoroutine(CorrectAnswer());
        }
        else
        {
            action = StartCoroutine(IncorrectAnswer());
        }
    }

    protected IEnumerator CorrectAnswer()
    {
        selectThisItem.gameObject.SetActive(false);

        yield return WaitForDialog(correctText);
        selectedObject = lookedAtObject;
        //selectThisItem.SetActive(false);
        started = false;

        //GetComponent<Playable>()._interacting = false;

        GetComponent<CameraLerper>().EndGame();
        
        if (!cachedDetails.postMatchName.Equals(""))
            cachedDetails.triggerObject.name = cachedDetails.postMatchName;

        action = null;

        gameObject.SetActive(false);
        cachedDetails.success = true;
    }

    protected IEnumerator IncorrectAnswer()
    {
        selectThisItem.gameObject.SetActive(false);

        yield return WaitForDialog(incorrectText);
        action = null;

        selectThisItem.gameObject.SetActive(true);
    }

    protected IEnumerator WaitForDialog(TextAsset asset)
    {
        DialogueController.HandleDialogueText(asset);
        yield return new WaitUntil(() => DialogueController.HandlingText == false);
        Debug.LogWarning("DONE WAITING");
    }
}
