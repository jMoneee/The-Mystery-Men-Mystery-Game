using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PictureTaker : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectsOfType<Camera>().Where(x => x.targetTexture != null).FirstOrDefault();
    }

    public void AddToJournal()
    {
        Texture2D texture = RealTimeImage(cam);
        JournalManager2.AddTextToJournal(null, Sprite.Create(texture, new Rect(0, 0, cam.activeTexture.width, cam.activeTexture.height), Vector2.one * 0.5f), Vector2.one * 1000);
    }

    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RealTimeImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
}
