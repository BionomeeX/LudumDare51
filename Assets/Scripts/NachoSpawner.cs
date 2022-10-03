using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LudumDare51
{
    public class NachoSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private GameObject _prefab;

        private void Awake()
        {
            var thumbnail = false;
            for (int y = -500; y < Screen.height + 500; y += 100)
            {
                for (int x = -500; x < Screen.width + 500; x += 100)
                {
                    var go = Instantiate(_prefab, _container);
                    go.transform.position = new Vector2(x, y);
                    if (!thumbnail && x == 200 && y == 500)
                    {
                        go.GetComponent<Image>().color = new Color(.8f, .8f, .8f);
                        go.GetComponent<Button>().onClick.AddListener(new(() =>
                        {
                            SceneManager.LoadScene("MiniGame");
                        }));
                    }
                }
            }
        }
    }
}
