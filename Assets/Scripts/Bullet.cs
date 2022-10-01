using UnityEngine;

namespace LudumDare51
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D _rb;

        public float Speed { set; private get; }
        public Vector2 Target { set; private get; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<Enemy>().TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
