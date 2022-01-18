
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField field;

    private void Start () {

        Cache.Load();

        field.text = "";

        Cache.results.ForEach(delegate(string barcode) {

            field.text += barcode + '\n';
        });
    }

    public void ResetCache () {

        Cache.totalScanned = 0;
        Cache.results.Clear();
        Cache.Save();
    }
}
