using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoScreenController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("Scene");
    }
}
