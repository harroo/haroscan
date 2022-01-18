
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField field;

    private void Start () {

        field.text = "";

        Cache.results.ForEach(delegate(string barcode) {

            field.text += barcode + '\n';
        });
    }
}
