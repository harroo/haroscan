
using System.Threading;

using UnityEngine;
using UnityEngine.UI;

using Auki;
using Auki.Barcode;

public class SimpleTest : MonoBehaviour {

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
    }


    public RawImage display;
    public Text resultText;

    private float timer;
    int x=0;

    private void Update () {

        if (cache != output) {

            cache = output;

            resultText.text = output;
        }
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
                if (result != null)
                    output = result.Text;
                else output = "failed" + (x++);

                Thread.Sleep(256);

            } catch {}
        }
    }

    private void OnDestroy () {

        camTexture.Stop();
        barcodeThread.Abort();
    }
}
