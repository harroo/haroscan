
using System.Threading;

using UnityEngine;
using UnityEngine.UI;

using Auki;
using Auki.Barcode;

public class SimpleTest : MonoBehaviour {

    private WebCamTexture camTexture;
    private Thread barcodeThread;

    private Color32[] pixelArray;
    private int width, height;

    private Rect screenRect;


    private void Start () {

        screenRect = new Rect (0, 0, Screen.width, Screen.height);

        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        camTexture.Play();
        width = camTexture.width;
        height = camTexture.height;

        barcodeThread = new Thread(()=>DecodeLoop());
        barcodeThread.Start();
    }


    public RawImage imagething;

    private void Update () {

        // if (pixelArray == null)
        //     pixelArray = camTexture.GetPixel32();

        if (Input.GetKey(KeyCode.R)) {

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(System.IO.File.ReadAllBytes("/home/haro/Downloads/barcode1.png"));

            imagething.texture = texture;

            pixelArray = texture.GetPixels32();

            IBarcodeReader reader = new BarcodeReader();

            var result = reader.Decode(pixelArray, texture.width, texture.height);
            if (result != null) {

                CheckResult(result.Text);
            }
        }
    }


    private void OnDestroy () {

        barcodeThread.Abort();
        camTexture.Stop();
    }

    private void OnApplicationQuit () {

        shouldStop = true;
    }


    private string resultCache = "";

    private void CheckResult (string result) {

        if (resultCache == result) return;
        resultCache = result;

        Debug.Log(result);
    }


    private bool shouldStop = false;

    private void DecodeLoop () {

        IBarcodeReader reader = new BarcodeReader();

        while (true) {

            if (shouldStop) break;

            try {

                if (pixelArray == null) continue;

                var result = reader.Decode(pixelArray, width, height);

                if (result != null) {

                    CheckResult(result.Text);
                }

                Thread.Sleep(256);
                pixelArray = null;

            } catch {}
        }
    }
}
