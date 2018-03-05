using UnityEngine;
using UnityEngine.SceneManagement;

namespace bootstrap
{
    public class StartScene : MonoBehaviour
    {
        // TODO(Wolf): Load necessary user data
        // TODO(Wolf): Initialize services

        // TODO(Wolf): Play first-time intro video before going to main?

        void Awake()
        {
            SceneManager.LoadScene ("Productivity Main");
        }
    }
}