using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager2 : MonoBehaviour
{
    public GameObject Content;
    public Image BackgroundImage;
    public GameObject textPrefabNoImage;
    public GameObject textPrefabLeftImage;
    public GameObject textPrefabRightImage;
    private bool nextImageLeft = true;
    public GameObject textHolder;
    private float beginningHeight;
    private float beginningSizeDeltaY;

    private ScrollRect scrollRect;


    public static JournalManager2 instance;
    private void Awake()
    {
        instance = this;
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //BackgroundImage.material = new Material(BackgroundImage.material);
        //BackgroundImage.material.EnableKeyword("_MainTex");
        beginningHeight = Content.GetComponent<RectTransform>().rect.height;
        beginningSizeDeltaY = Content.GetComponent<RectTransform>().sizeDelta.y;
        AddTextToJournal("This case is tough, so I need to document my steps here should I need to return to them.");
    }

    // Update is called once per frame
    void Update()
    {
        //BackgroundImage.GetComponent<RectTransform>().sizeDelta = Content.GetComponent<RectTransform>().sizeDelta;
        //Debug.Log((Content.GetComponent<RectTransform>().rect.height));
        BackgroundImage.materialForRendering.SetTextureScale("_MainTex", new Vector2(1,  Content.GetComponent<RectTransform>().rect.height / beginningHeight));
        //BackgroundImage.material.mainTextureScale = new Vector2(1, (Content.GetComponent<RectTransform>().rect.height) / beginningHeight);

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddTextToJournal("Randomized Journal Entry Number: " + Random.Range(0, 12345));
        }

    }

    public static void AddTextToJournal(string text)
    {
        GameObject insText = GameObject.Instantiate(instance.textPrefabNoImage, instance.textHolder.transform);
        insText.GetComponent<TextMeshProUGUI>().text = text;

        instance.StartCoroutine(instance.setSize());
        //instance.Content.GetComponent<RectTransform>().sizeDelta = new Vector2(instance.Content.GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(instance.textHolder.GetComponent<RectTransform>().sizeDelta.y + insText.GetComponent<RectTransform>().sizeDelta.y, instance.beginningSizeDeltaY));
    }

    private IEnumerator setSize()
    {
        yield return new WaitForEndOfFrame();
        instance.Content.GetComponent<RectTransform>().sizeDelta = new Vector2(instance.Content.GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(instance.textHolder.GetComponent<RectTransform>().sizeDelta.y, instance.beginningSizeDeltaY));
        Debug.Log("SIZE DELTA CONTENT: " + Content.GetComponent<RectTransform>().sizeDelta + " SIZE DELTA TEXTHOLDER: " + textHolder.GetComponent<RectTransform>().sizeDelta);

        yield return new WaitForEndOfFrame();

        scrollRect.verticalNormalizedPosition = 0;
    }

    public void SetScrollRectPosition (Vector2 vec)
    {
        textHolder.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = vec.y;
    }

    public static void AddTextToJournal(string text, Sprite image, PhotoPlacementOptions photoPlacement = PhotoPlacementOptions.DifferentThanLast)
    {
        if (photoPlacement == PhotoPlacementOptions.Left || (photoPlacement == PhotoPlacementOptions.DifferentThanLast && instance.nextImageLeft))
        {
            GameObject insText = GameObject.Instantiate(instance.textPrefabNoImage, instance.textHolder.transform);
            insText.GetComponent<TextMeshProUGUI>().text = text;

            instance.nextImageLeft = false;
        }
        else if (photoPlacement == PhotoPlacementOptions.Right || (photoPlacement == PhotoPlacementOptions.DifferentThanLast && !instance.nextImageLeft))
        {
            GameObject insText = GameObject.Instantiate(instance.textPrefabNoImage, instance.textHolder.transform);
            insText.GetComponent<TextMeshProUGUI>().text = text;

            instance.nextImageLeft = false;
        }
    }
}

public enum PhotoPlacementOptions
{
    Left,
    Right,
    DifferentThanLast
}
