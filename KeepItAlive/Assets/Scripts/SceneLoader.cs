using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{

    public static SceneLoader Instance { get; private set; }

    private Queue<string> ScenesToLoad = new Queue<string>();
    private List<string> LoadedScenes = new List<string>();

    private string _previousActiveScene;

    public void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        //else if(Instance != this) {
        //    Destroy(gameObject);
        //}
    }

    // Start is called before the first frame update
    protected SceneLoader() {
        Instance = this;
    }

    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive) {
        //SceneManager.LoadScene("Loading");

        _previousActiveScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadAfterTimer(sceneName, mode));
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void NextLevel() {
        GameState.Level++;

        LoadScene("WorldView", LoadSceneMode.Single);
    }

    public void UnloadScene(string sceneName) {
        //SceneManager.LoadScene("Loading");

        StartCoroutine(UnLoadAfterTimer(sceneName));
    }

    public void Quit() {
        Debug.Log("Quit");
        Application.Quit();
    }

    private IEnumerator UnLoadAfterTimer(string sceneName) {
        // the reason we use a coroutine is simply to avoid a quick "flash" of the 
        // loading screen by introducing an artificial minimum load time :boo:
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(FinallyUnloadScene(sceneName));
    }

    private IEnumerator LoadAfterTimer(string sceneName, LoadSceneMode mode) {
        // the reason we use a coroutine is simply to avoid a quick "flash" of the 
        // loading screen by introducing an artificial minimum load time :boo:
        yield return new WaitForSeconds(2.0f);

        ScenesToLoad.Enqueue(sceneName);

        StartCoroutine(FinallyLoadScene(mode));
    }

    private IEnumerator FinallyUnloadScene(string sceneName) {
        LoadedScenes.Remove(sceneName);

        var loading = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return loading;

        var scene = SceneManager.GetSceneByName(_previousActiveScene);
        SceneManager.SetActiveScene(scene);
    }

    private IEnumerator FinallyLoadScene(LoadSceneMode mode = LoadSceneMode.Additive) {
        var sceneName = ScenesToLoad.Dequeue();

        if (!LoadedScenes.Contains(sceneName)) {
            LoadedScenes.Add(sceneName);
            ScenesToLoad.Clear();

            var loading = SceneManager.LoadSceneAsync(sceneName, mode);
            yield return loading;

            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }
    }
}
