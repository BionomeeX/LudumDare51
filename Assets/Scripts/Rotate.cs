using UnityEngine;

namespace LudumDare51
{
    public class Rotate : MonoBehaviour
    {
        private float _speed;

        private void Awake()
        {
            _speed = Random.Range(30f, 50f) * (Random.Range(0, 2) == 0 ? 1f : -1f);
        }

        private void Update()
        {
            transform.Rotate(0, 0, _speed * Time.deltaTime);
        }
    }
}
