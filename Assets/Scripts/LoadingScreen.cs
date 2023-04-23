using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public float loadTime = 10;
    public Slider ProgressBar;
    public List<LoadedAsset> Assets;
    public List<LoadedAsset> ResourcesAssets;
    public List<LoadedAsset> StreamingAssets;
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private CancellationToken cancellationToken;

    private async void Start()
    {
        cancellationToken = cancellationTokenSource.Token;
        await LoadAllAssets();
        await LoadAllAssetsFromResources();
        await LoadAllAssetsFromStreamingAssets();
    }

    public async Task LoadAllAssets()
    {
        foreach (var asset in Assets)
        {
            await Task.Delay(5);
            Task.Run(async () =>
            {
                await asset.Load(cancellationToken);
            }).ContinueWith(UpdateSlider);
        }
    }

    public async Task UpdateSlider(Task task)
    {
        ProgressBar.value += 1.0f/Assets.Count;
    }

    public async Task LoadAllAssetsFromResources()
    {
        foreach (var asset in ResourcesAssets)
        {
            await Task.Delay(5);
            asset.LoadFromResources(cancellationToken);
        }
    }

    public async Task LoadAllAssetsFromStreamingAssets()
    {
        foreach (var asset in StreamingAssets)
        {
            await Task.Delay(5);
            asset.LoadFromStreamingAssets(cancellationToken);
        }
    }

    // good for testing!
    [ContextMenu("Cancel")]
    public void CancelLoading()
    {
        cancellationTokenSource.Cancel();
    }
}
