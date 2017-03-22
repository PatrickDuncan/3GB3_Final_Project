using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnEnable() {
		SceneManager.activeSceneChanged += OnLevelChanged;
	}

	void OnLevelChanged(Scene oldScene, Scene newScene) {
		GetComponent<AudioSource>().UnPause();
	}

	public void PlayPressed() {
		GetComponent<AudioSource>().Pause();
		GetComponent<AudioSource>().volume = 0.20f;
	}

	public void LowerPitch() {
        GetComponent<AudioSource>().pitch -= 0.075f;
		GetComponent<AudioSource>().volume += 0.05f;
    }
}
