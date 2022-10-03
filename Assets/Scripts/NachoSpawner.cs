using UnityEngine;
using UnityEngine.UI;

namespace LudumDare51
{
    public class NachoSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container, _containerSpe;

        [SerializeField]
        private GameObject _prefab, _prefabSpe;

        private void Awake()
        {
            var thumbnail = false;
            for (int y = -500; y < Screen.height + 500; y += 100)
            {
                for (int x = -500; x < Screen.width + 500; x += 100)
                {
                    var spe = !thumbnail && x == 200 && y == 500;
                    var go = Instantiate(spe ? _prefabSpe : _prefab, spe ? _containerSpe : _container);
                    go.transform.position = new Vector2(x, y);
                    if (spe)
                    {
                        go.name = "Special Nachos";
                        go.GetComponent<Image>().color = new Color(.75f, .75f, .75f);
                    }
                }
            }
        }
    }
}
