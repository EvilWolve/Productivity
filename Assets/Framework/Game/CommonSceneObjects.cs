using UnityEngine;

namespace framework.game
{
    public class CommonSceneObjects : MonoBehaviour
    {
        // TODO(Wolf): Initialise persistent scene objects here.

        static CommonSceneObjects instance;

        void Awake()
        {
            if (CommonSceneObjects.instance != null)
            {
                MonoBehaviour.Destroy (this.gameObject);
                return;
            }

            CommonSceneObjects.instance = this;

            DontDestroyOnLoad (this.gameObject);

            // TODO(Wolf): Pull localisation files from somewhere and initialize them here!
        }
    }
}