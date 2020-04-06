using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

	//an activation condition array i had to implement
	//to force tagging evidence before taking the picture
	public Taggable[] conditions;

    private void OnTriggerEnter(Collider other)
    {
        if (conditions != null && conditions.Where(x => !x.interacting).Count() == 0 && other.tag == "Player")
        {
            OnTriggerEnterEvent.Invoke();
        }
    }

	private void OnTriggerStay(Collider other)
	{
		if (conditions != null && conditions.Where(x => !x.interacting).Count() == 0 && other.tag == "Player")
		{
			OnTriggerEnterEvent.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnTriggerExitEvent.Invoke();
        }
    }
}
