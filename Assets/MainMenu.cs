using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class MainMenu : MonoBehaviour
{
	public GameObject Menu;
	public GameObject Dialogue;
	public GameObject FPController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Cursor.visible = true;
    }

	public void PlayGame()
	{
		LoadScreen.LoadNext();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ContinueGame()
	{
		FindObjectOfType<ActivateMenu>().Resume();
	}
}
