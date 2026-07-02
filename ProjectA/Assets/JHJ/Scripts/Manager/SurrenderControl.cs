using UnityEngine;
using UnityEngine.SceneManagement;

public class SurrenderControl : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "JHJ_TitleScene";

    public void GoToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}