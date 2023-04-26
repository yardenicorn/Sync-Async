using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class LoadedAsset
{
    [SerializeField] int delayTime = 1000;
    [SerializeField] string Prefab;
    public RawImage myImage;

    // גורם חיצוני יכול לחכות על הפונקציה שהיא תסתיים
    public async Task Load(CancellationToken cancellationToken)
    {
        await Task.Delay(delayTime, cancellationToken);
    }

    public async Task LoadFromResources(CancellationToken cancellationToken)
    {
        var request = Resources.LoadAsync(Prefab);
        while (!request.isDone)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await Task.Yield();
        }
        GameObject.Instantiate(request.asset);
    }

    public async Task LoadFromStreamingAssets()
    {
        string ImagePath = Application.streamingAssetsPath + "/" + "Cute-Eevee" + ".png";
        LoadImageFromUrl(ImagePath);
        myImage.gameObject.SetActive(true);
    }

    void LoadImageFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return;

        byte[] imageData = File.ReadAllBytes(url);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageData))
        {
            myImage.texture = texture;
            myImage.SetNativeSize();
        }
    }
}
