using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    private void Awake()
    {
        instance = this;
    }

    //This is used to keep all of are IHandleLine classes
    private Dictionary<LineType, IHandleLine> LineHandlers;
    // Start is called before the first frame update
    void Start()
    {
        LineHandlers = new Dictionary<LineType, IHandleLine>();
        LineHandlers.Add(LineType.Normal, new StandardLineHandler());
        GetComponent<CanvasGroup>().ChangeCanvasGroupVisibility(false);
        //LineHandlers.Add(LineType.Choice, new HandleChoiceLine());
        //LineHandlers.Add(LineType.Input, new HandleInputLine());
    }

    //Variables For Handling The Reading Of Different Lines
    public Coroutine HandlingLineCoroutine;
    public bool isHandlingLine { get { return HandlingLineCoroutine != null; } }

    public void StopHandlingLine()
    {
        if (isHandlingLine)
        {
            StopCoroutine(HandlingLineCoroutine);
            HandlingLineCoroutine = null;
        }
    }

    [HideInInspector]
    public string cachedLastSpeaker;
    //Variables for keeping track of current place in the story
    public bool _next = false;
    public void Next() { _next = true; }
    public int chapterProgress = 0;

    private void Update()
    {
        if (HandlingText)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) 
                || Input.GetMouseButtonDown(0))
            {
                Next();
            }
        }
    }

    private Coroutine handlingText;
    public bool HandlingText { get { return handlingText != null; } }

    private List<string> data;

    public void LoadFile(string textAsset)
    {
        LoadFile(Resources.Load(textAsset) as TextAsset);
    }

    public void LoadFile(TextAsset textAsset)
    {
        data = FileManager.ReadTextAsset(textAsset);
    }

    public void HandleDialogueText(TextAsset text)
    {
        DetachCamera.Detach();
        GetComponent<CanvasGroup>().ChangeCanvasGroupVisibility(true);
        LoadFile(text);
        handlingText = StartCoroutine(_HandleDialogueText(text));
        Next();
    }

    private IEnumerator _HandleDialogueText(TextAsset text)
    {
        yield return new WaitForEndOfFrame();


        while (chapterProgress < data.Count)
        {
            //Debug.Log("waiting for next in chapter file");

            if (_next && isHandlingLine == false)
            {
                string line = data[chapterProgress];
                LineType lineType = GetLineType(line);

                HandlingLineCoroutine = StartCoroutine(LineHandlers[lineType].HandleLine(line));
                while (isHandlingLine)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }


        while (!_next)
        {
            yield return new WaitForEndOfFrame();
        }

        DetachCamera.Reattach();
        GetComponent<CanvasGroup>().ChangeCanvasGroupVisibility(false);
        chapterProgress = 0;
        data = null;
        handlingText = null;
    }

    public LineType GetLineType(string line)
    {
        if (line.ToLower().StartsWith("choice"))
        {
            return LineType.Choice;
        }

        if (line.ToLower().StartsWith("input"))
        {
            return LineType.Input;
        }

        return LineType.Normal;
    }
}
public enum LineType
{
    Normal,
    Choice,
    Input
}

