using LudumDare51.SO;
using UnityEngine;

namespace LudumDare51.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public Node NextNode { set; private get; }

        public EnemyInfo Info { set; private get; }

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        private int _health;
        public bool IsAlive { private set; get; } = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _health = Info.BaseHealth;
            _sr.sprite = Info.SpriteAlive;
        }

        private void FixedUpdate()
        {
            var targetPos = NextNode.transform.position;
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            _rb.velocity = Info.Speed * Time.fixedDeltaTime * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            if (Vector2.Distance(transform.position, NextNode.transform.position) <= .1f)
            {
                NextNode = NextNode.NextNode;
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                IsAlive = false;
                _sr.sprite = Info.SpriteDead;
            }
        }
    }
}
