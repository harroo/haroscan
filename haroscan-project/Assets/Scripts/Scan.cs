
using System.Threading;

using UnityEngine;
using UnityEngine.UI;

using Auki;
using Auki.Barcode;

public class Scan : MonoBehaviour {

    private WebCamTexture camTexture;
    private Thread barcodeThread;

    private Color32[] pixelArray;
    private int widthRes = 820, heightRes = 320;

    private Rect screenRect;


    private void Start () {

        screenRect = new Rect (0, 0, widthRes, heightRes);

        camTexture = new WebCamTexture();
        camTexture.requestedWidth = widthRes;
        camTexture.requestedHeight = heightRes;
        camTexture.Play();

        display.texture = camTexture;

        barcodeThread = new Thread(()=>DecodeLoop());
        barcodeThread.Start();
        
        countText.text = Cache.totalScanned.ToString();
    }


    public RawImage display;
    public Text resultText, countText;
    public AudioSource scanSound;

    private void Update () {

        // For Debugging.
        if (Input.GetKeyDown(KeyCode.Space)) {

            string testCode = "";
            for (int i = 0; i < 16; ++i) testCode += Random.Range(0, 9).ToString();

            output = testCode;
        }

        if (cache == output) return;

        cache = output;

        if (output.Length != 16) return;

        if (Cache.results.Contains(output)) return;

        Cache.results.Add(output);
        Cache.totalScanned++;
        Cache.Save();

        resultText.text = output;
        countText.text = Cache.totalScanned.ToString();

        scanSound.Play();
    }

    private string cache, output;

    private void DecodeLoop () {

        while (true) {

            try {

                IBarcodeReader reader = new BarcodeReader{

                    AutoRotate = true,

                    Options = new Auki.Barcode.Common.DecodingOptions{

                        TryHarder = true
                    }
                };

                var result = reader.Decode(camTexture.GetPixels32(), widthRes, heightRes);

                output = result != null ? result.Text : "Hold Steady...";

                Thread.Sleep(256);

            } catch {}
        }
    }

    private void OnDestroy () {

        camTexture.Stop();
        barcodeThread.Abort();
    }
}
