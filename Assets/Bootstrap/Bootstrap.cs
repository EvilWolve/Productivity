using UnityEngine;
using UnityEngine.SceneManagement;

namespace bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        // TODO(Wolf): Take care of initialisation of frameworks and potentially fetching basic user profile data here

        // TODO(Wolf): Play splash screen

        void Start()
        {
            SceneManager.LoadScene("Start");
        }
    }
}
