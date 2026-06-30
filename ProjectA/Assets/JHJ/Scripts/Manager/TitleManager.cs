using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("씬 이동")]
    [SerializeField] private string ingameSceneName = "CGS_Ingame";

    [Header("시작 시 비활성화되는 패널들")]
    [SerializeField] private GameObject[] panelsToHideOnStart;

    private void Start()
    {
        foreach (GameObject panel in panelsToHideOnStart)
        {
            if (panel != null) panel.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(ingameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(false);
    }
}