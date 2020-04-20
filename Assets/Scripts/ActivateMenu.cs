using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ActivateMenu : MonoBehaviour
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
		if (Input.GetKey(KeyCode.Escape))
		{
			Menu.SetActive(true);
			if (Dialogue != null)
				Dialogue.GetComponentInChildren<Canvas>().enabled = false;
			if (FPController != null)
			{
				//FPController.GetComponent<RigidbodyFirstPersonController>().enabled = false;
			}
		}
    }
}
