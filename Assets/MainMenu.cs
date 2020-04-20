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
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Debug.Log("You just lost the game");
		Application.Quit();
	}

	public void ContinueGame()
	{
		Menu.SetActive(false);
		if (Dialogue != null)
			Dialogue.GetComponentInChildren<Canvas>().enabled = true;
		if (FPController != null)
		{
			//FPController.GetComponent<RigidbodyFirstPersonController>().enabled = true;
		}
	}
}
