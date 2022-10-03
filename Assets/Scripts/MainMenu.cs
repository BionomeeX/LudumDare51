using LudumDare51.Translation;
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

        public void SetEnglish()
        {
            Translate.Instance.CurrentLanguage = "english";
        }

        public void SetFrench()
        {
            Translate.Instance.CurrentLanguage = "french";
        }
    }
}
