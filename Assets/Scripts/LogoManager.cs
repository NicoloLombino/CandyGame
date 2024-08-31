using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += LoadGameScene;
    }

    private void LoadGameScene(VideoPlayer video)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
