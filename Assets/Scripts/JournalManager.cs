using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    public static JournalManager instance;
    private void Awake()
    {
        instance = this;
    }

    private Animator _animator = null;
    private Animator animator { get { if (_animator == null) { _animator = GetComponentInChildren<Animator>(); } return _animator; } }

    private int page_number = 1;
    private int total_pages = 1;

    public GameObject LeftPage;
    public GameObject RightPage;

    private bool onLeft = true;

    private Dictionary<int, List<GameObject>> left_pages;
    private Dictionary<int, List<GameObject>> right_pages;

    // Start is called before the first frame update
    void Start()
    {
        left_pages = new Dictionary<int, List<GameObject>>();
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(LeftPage.transform.GetChild(0).gameObject);
        left_pages.Add(1, gameObjects);

        right_pages.Add(1, new List<GameObject>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("StillPage"))
            {
                if (page_number < total_pages)
                {
                    animator.SetTrigger("TurnPageRight");
                    StartCoroutine(TurnPage(true));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("StillPage"))
            {
                if (page_number > 1)
                {
                    animator.SetTrigger("TurnPageLeft");
                    StartCoroutine(TurnPage(false));
                }
            }
        }
    }

    public IEnumerator TurnPage(bool right)
    {
        while (LeftPage.transform.childCount > 0)
        {
            GameObject.Destroy(LeftPage.transform.GetChild(0));
        }

        while (RightPage.transform.childCount > 0)
        {
            GameObject.Destroy(RightPage.transform.GetChild(0));
        }

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("StillPage") == false)
        {
            yield return new WaitForEndOfFrame();
        }

        page_number += right ? 1 : -1;
        for(int i = 0; i < left_pages[page_number].Count; i++)
        {
            GameObject insObj = GameObject.Instantiate(left_pages[page_number][i], LeftPage.transform);
        }
        for (int i = 0; i < right_pages[page_number].Count; i++)
        {
            GameObject insObj = GameObject.Instantiate(right_pages[page_number][i], RightPage.transform);
        }
    }



}
