using UnityEngine;

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
            for (int y = -500; y < Screen.height + 500; y += 100)
            {
                for (int x = -500; x < Screen.width + 500; x += 100)
                {
                    var go = Instantiate(_prefab, _container);
                    go.transform.position = new Vector2(x, y);
                }
            }
        }
    }
}
