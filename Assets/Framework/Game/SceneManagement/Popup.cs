// Author:			Wolfgang Neumayer
// Creation Date:	26/03/2018

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class Popup : MonoBehaviour
{
    public class SpawnInfo
    {

    }

    private static Dictionary<string, SpawnInfo> popupsToSpawn;

    protected static List<Popup> visiblePopups;

    public static void Spawn(string sceneName, SpawnInfo info)
    {
        if (Popup.popupsToSpawn.ContainsKey (sceneName))
        {
            Popup.popupsToSpawn[sceneName] = info;
        }
        else
        {
            Popup.popupsToSpawn.Add (sceneName, info);

            SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);
        }
    }

    protected virtual string SceneName { get { return string.Empty; } }

    private void Awake()
    {
        SpawnInfo info = Popup.popupsToSpawn[this.SceneName];
        Popup.popupsToSpawn.Remove (this.SceneName);

        this.OnAwake ();

        this.InitContent (info);
    }

    protected virtual void OnAwake()
    {

    }

    private void RegisterPopup()
    {
        Popup.visiblePopups.Add (this);
    }

    protected virtual void InitContent(SpawnInfo info)
    {

    }

    protected void Close()
    {
        this.UnregisterPopup ();

        this.OnClose ();
    }

    private void UnregisterPopup()
    {
        Assert.IsTrue (Popup.visiblePopups.Contains (this), string.Format ("Popup {0} is being removed but is not in the list of visible popups!", this.SceneName));
        Assert.IsTrue (Popup.visiblePopups[Popup.visiblePopups.Count - 1] == this, string.Format ("Popup {0} is being removed but is not the topmost popup!", this.SceneName));

        Popup.visiblePopups.Remove (this);
    }

    protected virtual void OnClose()
    {

    }

    #region Static access

    public static bool IsPopupVisible<T>() where T : Popup
    {
        Type targetType = typeof (T);
        foreach (Popup p in Popup.visiblePopups)
        {
            if (p.GetType() == targetType)
            {
                return true;
            }
        }
        
        return false;
    }

    public static int GetVisiblePopupCount()
    {
        return Popup.visiblePopups.Count;
    }

    #endregion
}