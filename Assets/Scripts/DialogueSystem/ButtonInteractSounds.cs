using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteractSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] AudioClip m_PointerEnterSound;
	[SerializeField] AudioClip m_PointerExitSound;

	private AudioSource sound;

	void Start()
    {
		sound = GetComponent<AudioSource>();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		sound.PlayOneShot(m_PointerEnterSound);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		sound.PlayOneShot(m_PointerExitSound);
	}
}
