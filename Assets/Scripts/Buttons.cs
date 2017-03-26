using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    bool inMainMenu = true;

    Music music;

    public void Start() {
        music = GameObject.FindWithTag("Music").GetComponent<Music>();
    }

    public void Play() {
        if (inMainMenu) {	// Stops player from press Start multiple times and creating clones of all the gameobjects
			inMainMenu = false;
            music.PlayPressed();
			Invoke("NextScene", 1f);
		}
	}

    void NextScene() {
        SceneManager.LoadScene(1);
    }

    public void Restart() {
        Time.timeScale = 1;
        GameObject.FindWithTag("Music").GetComponent<Music>().Restart();
        NextScene();
    }

    public void Quit() {
		#if UNITY_STANDALONE
			Application.Quit();
		#endif
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
