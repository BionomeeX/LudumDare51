using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare51
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
