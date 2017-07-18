using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashLoading : MonoBehaviour {

    [SerializeField]
    Database DBd = null;
    [SerializeField]
    SoundManager SM = null;

    [SerializeField]
    Button confirmButton = null;

    // Use this for initialization
    void Start() {
        confirmButton.onClick.AddListener(BeginGame);
		if (FindObjectOfType<Database>() != null) {
			Destroy(Database.Instance.gameObject);
		}
        if (FindObjectOfType<SoundManager>() != null) {
            Destroy(SoundManager.Instance.gameObject);
        }

		GameObject tmp = Instantiate(DBd.gameObject);
		tmp.name = "GameDatabase";
        DontDestroyOnLoad(tmp);
        tmp = Instantiate(SM.gameObject);
        tmp.name = "SoundManager"; 
		DontDestroyOnLoad(tmp);

        SoundManager.Instance.UpdateSoundList();
    }


    void BeginGame() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

}
