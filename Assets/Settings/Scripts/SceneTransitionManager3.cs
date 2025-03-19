using UnityEngine;
using UnityEngine.SceneManagement;
using Meta.WitAi; // Required for MetaXR functionality

public class LoadSceneMetaXR3 : MonoBehaviour
{
    public string sceneName; // Assign in Inspector

    public void LoadNewScene3()
    {
        SceneManager.LoadScene(0);
    }
}
