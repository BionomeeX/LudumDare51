using LudumDare51.Enemy;
using LudumDare51.SO;
using UnityEngine;

namespace LudumDare51.Tower
{
    public class Bullet : MonoBehaviour
    {
        public float Speed { set; private get; }
        public Vector2 Target { set; private get; }
        public TowerInfo Info { set; private get; }

        private void Start()
        {
            var targetPos = Target;
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            GetComponent<Rigidbody2D>().velocity = transform.right * Speed;
        }

        private void Update()
        {
            transform.Rotate(0, 0, 1000f * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                if (Info.SplashDamageRange == -1)
                {
                    collision.collider.GetComponent<EnemyAI>().TakeDamage(Info);
                }
                else
                {
                    var collisions = Physics2D.OverlapCircleAll(collision.contacts[0].point, Info.SplashDamageRange);
                    foreach (var coll in collisions)
                    {
                        if (coll.CompareTag("Enemy"))
                        {
                            coll.GetComponent<EnemyAI>().TakeDamage(Info);
                        }
                    }
                    OnClick.Instance.AddExplosion(collision.contacts[0].point);
                }
            }
            Destroy(gameObject);
        }
    }
}
