using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("씬 이동")]
    [SerializeField] private string ingameSceneName = "JHJ_GameTest1";

    public void StartGame()
    {
        SceneManager.LoadScene(ingameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}