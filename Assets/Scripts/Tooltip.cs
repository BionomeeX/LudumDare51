using TMPro;
using UnityEngine;

namespace LudumDare51
{
    public class Tooltip : MonoBehaviour
    {
        public static Tooltip Instance { private set; get; }

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private TMP_Text _text;

        private void Awake()
        {
            Instance = this;
            _container.SetActive(false);
        }

        public void Show(string text)
        {
            _container.SetActive(true);
            _text.text = text;
        }

        public void Hide()
        {
            _container.SetActive(false);
        }
    }
}
