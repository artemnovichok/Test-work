using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Loading());
	}

	private IEnumerator Loading()
	{
		PlayerSave.StartLoad();
		yield return new WaitForEndOfFrame();
		SceneManager.LoadSceneAsync(1);
	}
}
