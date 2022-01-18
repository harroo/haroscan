
using UnityEngine;

public class ShowOnce : MonoBehaviour {

    private void Start () {

        if (PlayerPrefs.GetInt("STARTCACHE", 0) == 0) {

            PlayerPrefs.SetInt("STARTCACHE", 1);

            return;
        }

        Destroy(this.gameObject);
    }
}
