using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void OnClick_PlayButton()
    {
        print("Hello");
        SceneManager.LoadScene("Game");
    }
}
