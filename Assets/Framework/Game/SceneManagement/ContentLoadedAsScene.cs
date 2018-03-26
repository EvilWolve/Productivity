// Author:			Wolfgang Neumayer
// Creation Date:	26/03/2018

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ContentLoadedAsScene : MonoBehaviour
{
    public class SpawnInfo
    {
        public Transform parent;
    }

    private static Dictionary<string, SpawnInfo> scenesToSpawn;

    public static void Spawn(string sceneName, SpawnInfo info)
    {
        if (ContentLoadedAsScene.scenesToSpawn.ContainsKey(sceneName))
        {
            ContentLoadedAsScene.scenesToSpawn[sceneName] = info;
        }
        else
        {
            ContentLoadedAsScene.scenesToSpawn.Add (sceneName, info);

            SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);
        }
    }

    protected virtual string SceneName { get { return string.Empty; } }

    private void Awake()
    {
        SpawnInfo info = ContentLoadedAsScene.scenesToSpawn[this.SceneName];
        ContentLoadedAsScene.scenesToSpawn.Remove (this.SceneName);

        this.transform.SetParent (info.parent);
        SceneManager.UnloadSceneAsync (this.SceneName);

        this.InitContent (info);
    }

    protected virtual void InitContent(SpawnInfo info)
    {

    }
}