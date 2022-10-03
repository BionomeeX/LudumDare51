using LudumDare51.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare51
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private TMP_Text _finalText;

        private void Awake()
        {
            _finalText.text = Translate.Instance.Tr("score") + ": " + DataKeeper.Instance.FinalScore;
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
