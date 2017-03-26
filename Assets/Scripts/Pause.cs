using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused;

    AudioSource[] allAudioSources;		// All audio sources in the scene.
    GameObject pauseText;
    PlayerHealth playerH;
    WaveLogic waveLogic;

    void Start() {
        pauseText = GameObject.FindWithTag("PauseText");
        pauseText.SetActive(false);
        playerH = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        waveLogic = GameObject.FindWithTag("Wave Logic").GetComponent<WaveLogic>();
    }

	void Update() {
        // Hit escape key, not dead and hasn't won
        if (Input.GetKeyDown(KeyCode.Escape)
        && playerH.GetHealth() > 0
        && !waveLogic.Won()) {
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
