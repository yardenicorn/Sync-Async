using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class LoadedAsset
{
    [SerializeField] int delayTime = 1000;
    [SerializeField] string assetID;

    // גורם חיצוני יכול לחכות על הפונקציה שהיא תסתיים
    public async Task Load(CancellationToken cancellationToken)
    {
        await Task.Delay(delayTime, cancellationToken);
    }

    public async Task LoadFromResources(CancellationToken cancellationToken)
    {
        var request = Resources.LoadAsync(assetID);
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
}
