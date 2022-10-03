using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51.MiniGame
{
    public class PlayerController : MonoBehaviour
    {
        private float y;
        private Rigidbody2D _rb;

        [SerializeField]
        private GameObject _bullet;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            StartCoroutine(Shoot());
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(0f, y * Time.fixedDeltaTime * 500f);
        }

        public void Move(InputAction.CallbackContext value)
        {
            y = value.ReadValue<Vector2>().y;
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                var go = Instantiate(_bullet, transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10f;
                Destroy(go, 10f);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
