using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnEnable() {
		SceneManager.activeSceneChanged += OnLevelChanged;
	}

	public void FullVolume() {
		GetComponent<AudioSource>().volume = 1f;
	}

	public void LowerPitch() {
        GetComponent<AudioSource>().pitch -= 0.075f;
		GetComponent<AudioSource>().volume += 0.05f;
    }

	public void HighPitch() {
        GetComponent<AudioSource>().pitch = 1.1f;
    }

	void OnLevelChanged(Scene oldScene, Scene newScene) {
		GetComponent<AudioSource>().UnPause();
	}

	public void PlayPressed() {
		GetComponent<AudioSource>().Pause();
		GetComponent<AudioSource>().volume = 0.20f;
	}

	public void Restart() {
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().pitch = 1f;
		GetComponent<AudioSource>().volume = 0.20f;
	}
}
