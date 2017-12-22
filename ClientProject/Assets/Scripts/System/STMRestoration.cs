using UnityEngine;
using System.Collections;

namespace MLA.System {
    public class STMRestoration : MonoBehaviour {

        // Use this for initialization
        void Awake() {
            DontDestroyOnLoad(gameObject);
            IEnumerator STMRes = STMRestore();
            StartCoroutine(STMRes);
        }

        IEnumerator STMRestore() {
            yield return new WaitForSeconds(1f);
            while (true) {
                for (int i = 0; i < Database.Instance.ArrayCharFMGetLenght(); i++) {
                    if (Database.Instance.GetCurrSTM(i) < Database.Instance.GetMaxSTM(i)) { Database.Instance.IncreaseCurrSTM(i, 1); }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
