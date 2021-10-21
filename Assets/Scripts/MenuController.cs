using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField]

    public void StartGame()
    {
        StartCoroutine(WaitForCameraToLoad());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitForCameraToLoad()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Gameplay");
    }
    
}
