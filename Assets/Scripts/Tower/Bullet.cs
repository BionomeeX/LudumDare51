using UnityEngine;

namespace LudumDare51
{
    public class Bullet : MonoBehaviour
    {
        public float Speed { set; private get; }
        public Vector2 Target { set; private get; }

        private void Start()
        {
            var targetPos = Target;
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            GetComponent<Rigidbody2D>().velocity = transform.right * Speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<EnemyAI>().TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
