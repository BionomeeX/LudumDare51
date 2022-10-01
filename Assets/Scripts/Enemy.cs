using LudumDare51.SO;
using UnityEngine;

namespace LudumDare51
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Node _nextNode;

        [SerializeField]
        private EnemyInfo _info;

        private Rigidbody2D _rb;

        private int _health;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _health = _info.BaseHealth;
        }

        private void FixedUpdate()
        {
            var targetPos = _nextNode.transform.position;
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            _rb.velocity = _info.Speed * Time.fixedDeltaTime * transform.right;
            if (Vector2.Distance(transform.position, _nextNode.transform.position) < .1f)
            {
                _nextNode = _nextNode.NextNode;
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
