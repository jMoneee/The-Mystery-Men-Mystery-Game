using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
	public Text text;
	[SerializeField] string fullText;
	public float progress;
	[SerializeField] AudioMixer mixer;

    void Awake()
    {
		fullText = text.text;
		Debug.Log(text.text + " fed into " + fullText);
		DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(int scene)
	{
		StartCoroutine(LoadSceneRoutine(scene));
	}

	private IEnumerator LoadSceneRoutine(int scene)
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(scene);

		mixer.GetFloat("volume", out float volume);
		mixer.SetFloat("volume", -1000f);
		while (!loading.isDone)
		{
			progress = Mathf.Clamp01(loading.progress / 0.9f);
			text.text = fullText.Substring(0, (int)Mathf.Lerp(0, fullText.Length, progress));
			yield return null;
		}
		mixer.SetFloat("volume", volume);

		Destroy(gameObject);
	}

	internal static void LoadNext()
	{
		LoadScreen ls = Instantiate(Resources.Load("Prefabs/LoadScreen") as GameObject, Vector3.zero, Quaternion.identity).GetComponent<LoadScreen>();
		ls.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
