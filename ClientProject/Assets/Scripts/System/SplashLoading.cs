using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashLoading : MonoBehaviour {

    [SerializeField]
    Database DBd = null;

    [SerializeField]
    Button confirmButton = null;

    // Use this for initialization
    void Start() {
        confirmButton.onClick.AddListener(BeginGame);
		if (FindObjectOfType<Database>() != null) {
			Destroy(FindObjectOfType<Database>().gameObject);
		}
		GameObject tmp = Instantiate(DBd.gameObject);
		tmp.name = "GameDatabase";
		DontDestroyOnLoad(tmp);
    }


    void BeginGame() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

}
