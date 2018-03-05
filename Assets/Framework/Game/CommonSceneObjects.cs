using UnityEngine;

namespace framework.game
{
    public class CommonSceneObjects : MonoBehaviour
    {
        // TODO(Wolf): Initialise persistent scene objects here.

        void Awake()
        {
            DontDestroyOnLoad (this.gameObject);
        }
    }
}