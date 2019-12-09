using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    [SerializeField]
    string mouseHoverSound = "ButtonHover";

    [SerializeField]
    string buttonPressSound = "ButtonPress";

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("FREAL OUT! No audioManager foudn in the scene");
        }
    }

public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);

        Debug.Log("APPLICATION QUIT!");
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        audioManager.PlaySound(buttonPressSound);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
