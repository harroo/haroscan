
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

        if (timer < 0) { timer = 1.0f;

            // Texture2D uncroppedTexture = new Texture2D(widthRes, heightRes);
            // uncroppedTexture.SetPixels32(camTexture.GetPixels32());
            // Texture2D croppedTexture = TextureTools.ResampleAndCrop(uncroppedTexture, widthRes / 3, heightRes);
            // pixelArray = croppedTexture.GetPixels32();

            // widthRes = croppedTexture.width;
            // heightRes = croppedTexture.height;

            // IBarcodeReader reader = new BarcodeReader{
            //
            //     AutoRotate = true,
            //
            //     Options = new Auki.Barcode.Common.DecodingOptions{
            //
            //         TryHarder = true
            //     }
            // };
            // var result = reader.Decode(camTexture.GetPixels32(), widthRes, heightRes);
            // if (result != null)
            //     resultText.text = result.Text;
            // else resultText.text = "failed" + (x++);

            // display.texture = croppedTexture;

        } else timer -= Time.deltaTime;

        if (cache != output) {

            cache = output;

            resultText.text = output;
        }

        // if (pixelArray == null)
        //     pixelArray = camTexture.GetPixels32();

        // if (Input.GetKey(KeyCode.R)) {
        //
        //     Texture2D texture = new Texture2D(2, 2);
        //     texture.LoadImage(System.IO.File.ReadAllBytes("/home/haro/Downloads/barcode1.png"));
        //
        //     widthRes = texture.width;
        //     heightRes = texture.height;
        //
        //     display.texture = texture;
        //
        //     pixelArray = texture.GetPixels32();
        //
        //     // IBarcodeReader reader = new BarcodeReader();
        //     //
        //     // var result = reader.Decode(pixelArray, texture.width, texture.height);
        //     // if (result != null) {
        //     //
        //     //     CheckResult(result.Text);
        //     // }
        //
        //     // Texture2D cropTexture = new Texture2D(widthRes, heightRes);
        //     // int third = cropTexture.height / 3;
        //     // Texture2D newTexture = new Texture2D(widthRes, third);
        //     //
        //     // cropTexture.SetPixels32(pixelArray);Debug.Log(third + " " + newTexture.width + " " + newTexture.height +  " "+ cropTexture.width + " " + cropTexture.height +  " ");
        //     //
        //     // for (int y = 0; y < third; ++y){
        //     //     for (int x = 0; x < cropTexture.width; ++x){
        //     //         newTexture.SetPixel(x, y, cropTexture.GetPixel(x, third + y));}}
        //     //
        //     // newTexture.Apply();
        //     // pixelArray = newTexture.GetPixels32();
        //
        //     //Debug.Log("asd");
        //
        //     Texture2D uncroppedTexture = new Texture2D(widthRes, heightRes);
        //     uncroppedTexture.SetPixels32(pixelArray);
        //     Texture2D croppedTexture = TextureTools.ResampleAndCrop(uncroppedTexture, widthRes, heightRes / 3);
        //     pixelArray = croppedTexture.GetPixels32();
        //     widthRes = croppedTexture.width;
        //     heightRes = croppedTexture.height;
        //
        //     IBarcodeReader reader = new BarcodeReader{
        //
        //         AutoRotate = true,
        //
        //         Options = new Auki.Barcode.Common.DecodingOptions{
        //
        //             TryHarder = true
        //         }
        //     };
        //     var result = reader.Decode(pixelArray, croppedTexture.width, croppedTexture.height);
        //     if (result != null)
        //         resultText.text = result.Text;
        //
        //     display.texture = croppedTexture;
        // }
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
