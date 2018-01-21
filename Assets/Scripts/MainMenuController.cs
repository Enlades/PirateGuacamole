using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public AudioSource SfxSource;

    public AudioClip ButtonSfx;

    public void PlayButtonSound() {
        SfxSource.clip = ButtonSfx;
        SfxSource.Play();
    }

    public void StartGame() {
        SceneManager.LoadScene(2);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
