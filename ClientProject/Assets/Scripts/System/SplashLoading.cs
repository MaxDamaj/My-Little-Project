using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashLoading : MonoBehaviour {

    [SerializeField]
    Database DBd = null;
    [SerializeField]
    SoundManager SM = null;
    [SerializeField]
    DBSimulation DBS = null;

    public Button confirmButton;
    public Slider loadingProgress;
    public RectTransform loadingWindow;

    private AsyncOperation loadingScene;

    void Start() {
        confirmButton.onClick.AddListener(BeginGame);
        if (FindObjectOfType<Database>() != null) {
            Destroy(Database.Instance.gameObject);
        }
        if (FindObjectOfType<SoundManager>() != null) {
            Destroy(SoundManager.Instance.gameObject);
        }
        if (FindObjectOfType<DBSimulation>() != null) {
            Destroy(DBSimulation.Instance.gameObject);
        }

        GameObject tmp = Instantiate(DBd.gameObject);
        tmp.name = "GameDatabase";
        DontDestroyOnLoad(tmp);
        tmp = Instantiate(SM.gameObject);
        tmp.name = "SoundManager";
        DontDestroyOnLoad(tmp);
        tmp = Instantiate(DBS.gameObject);
        tmp.name = "Database_Simulation";
        DontDestroyOnLoad(tmp);

        SoundManager.Instance.UpdateSoundList();
    }


    void BeginGame() {
        loadingScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("menu");
        loadingWindow.gameObject.SetActive(true);
        IEnumerator loading = ProgressWatching(loadingScene);
        StartCoroutine(loading);
    }

    IEnumerator ProgressWatching(AsyncOperation load) {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            loadingProgress.value = load.progress;
        }
    }

}
