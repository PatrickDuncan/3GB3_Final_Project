using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused;

    AudioSource[] allAudioSources;		// All audio sources in the scene.
    GameObject pauseText;

    void Start() {
        pauseText = GameObject.FindWithTag("PauseText");
        pauseText.SetActive(false);
    }

	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) Unpause();
            else        DoPause();
            paused = !paused;
        }
    }

    void DoPause() {
        Time.timeScale = 0;
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource sound in allAudioSources) {
			sound.Pause();
		}
        pauseText.SetActive(true);
    }

    void Unpause() {
        Time.timeScale = 1;
		foreach (AudioSource sound in allAudioSources) {
			sound.UnPause();
		}
        pauseText.SetActive(false);
    }
}
