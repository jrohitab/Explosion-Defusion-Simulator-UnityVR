using UnityEngine;
using UnityEngine.SceneManagement;
using Meta.WitAi; // Required for MetaXR functionality

public class LoadSceneMetaXR2 : MonoBehaviour
{
    public string sceneName; // Assign in Inspector

    public void LoadNewScene2()
    {
        SceneManager.LoadScene(3);
    }
}
