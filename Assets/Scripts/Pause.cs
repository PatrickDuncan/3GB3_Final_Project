using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused;

    AudioSource[] allAudioSources;		// All audio sources in the scene.
    GameObject pauseText;
    GameObject[] buttons;
    PlayerHealth playerH;
    WaveLogic waveLogic;

    void Awake() {
        buttons = GameObject.FindGameObjectsWithTag("DeathScreen");
    }

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
		foreach (GameObject gO in buttons) {
            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (gO.name != "Death") {
                gO.SetActive(true);
            }
		}
        pauseText.SetActive(true);
    }

    void Unpause() {
        Time.timeScale = 1;
		foreach (AudioSource sound in allAudioSources) {
			sound.UnPause();
		}
        foreach (GameObject gO in buttons) {
            Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (gO.name != "Death") {
                gO.SetActive(false);
            }
		}
        pauseText.SetActive(false);
    }
}
