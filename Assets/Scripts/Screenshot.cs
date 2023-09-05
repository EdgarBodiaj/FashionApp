using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public Canvas canvas;
    // Update is called once per frame
    public void TakeScreenshot()
    {
        if (canvas != null)
            canvas.enabled = false;
            ScreenCapture.CaptureScreenshot("screenshot.png");

        if (canvas != null)
            canvas.enabled = true;
    }
}
