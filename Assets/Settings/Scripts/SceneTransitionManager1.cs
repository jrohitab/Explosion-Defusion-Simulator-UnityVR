using UnityEngine;
using UnityEngine.SceneManagement;
using Meta.WitAi; // Required for MetaXR functionality

public class LoadSceneMetaXR1 : MonoBehaviour
{
    public string sceneName; // Assign in Inspector

    public void LoadNewScene1()
    {
        SceneManager.LoadScene(2);
    }
}
