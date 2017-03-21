using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    bool inMainMenu = true;

    public void Play() {
        if (inMainMenu) {	// Stops player from press Start multiple times and creating clones of all the gameobjects
			inMainMenu = false;
			Invoke("NextScene", 1f);
		}
	}

    void NextScene() {
//        showPanels.ToggleLoading(true);
        SceneManager.LoadScene(1);
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
