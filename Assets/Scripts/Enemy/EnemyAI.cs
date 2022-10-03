using LudumDare51.SO;
using UnityEngine;

namespace LudumDare51.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public Node NextNode { set; private get; }

        public EnemyInfo Info { set; private get; }

        public Vector3 Offset { set; private get; }
        public float _slowForce;
        public float _slowDuration;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Sprite _baseSprite;

        private int _health;
        public bool IsAlive { private set; get; } = true;
        private bool _isBeingEaten = false;

        private Eater _eater;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _baseSprite = _sr.sprite;
        }

        private void Start()
        {
            _health = Info.BaseHealth;
            _sr.sprite = Info.SpriteAlive;
        }

        private void Update()
        {
            if (_slowDuration > 0f)
            {
                _slowDuration -= Time.deltaTime;
                if (_slowDuration <= 0f)
                {
                    _sr.color = Color.white;
                }
            }
        }

        private void FixedUpdate()
        {
            var targetPos = NextNode.transform.position + (_isBeingEaten ? Vector3.zero : Offset);
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x);
            _rb.velocity = Info.Speed * Time.fixedDeltaTime * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * (_slowDuration > 0f ? _slowForce : 1f);
            if (targetPos.magnitude <= .2f)
            {
                if (NextNode.NextNode != null)
                {
                    NextNode = NextNode.NextNode;
                }
                else
                {
                    _eater.AddEat();
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsAlive && collision.CompareTag("Goal"))
            {
                _isBeingEaten = true;
                _eater = collision.GetComponentInParent<Eater>();
                NextNode = _eater.Goal;
                _rb.constraints = RigidbodyConstraints2D.None;
                _rb.AddTorque(100f * Random.Range(0, 2) == 0 ? -1f : 1f, ForceMode2D.Impulse);
            }
        }

        public void TakeDamage(TowerInfo info, bool hasHat)
        {
            var m = hasHat ? 1.5f : .5f;

            _health -= Mathf.CeilToInt(info.Damage * m);
            if (info.SpeedModifierDuration > 0f)
            {
                _sr.color = Color.yellow;
                if (info.SpeedModifierDuration * m > _slowDuration) _slowDuration = info.SpeedModifierDuration * m;
                if (info.SpeedModifierForce * m > _slowForce) _slowForce = info.SpeedModifierForce * m;
            }
            if (info.CleanAll)
            {
                _sr.color = Color.white;
                _slowDuration = 0f;
                if (!IsAlive)
                {
                    _health = Mathf.FloorToInt(Info.BaseHealth / 2f * m);
                    IsAlive = true;
                    _sr.sprite = _baseSprite;
                    gameObject.layer = LayerMask.NameToLayer("Enemy");
                }
            }
            if (_health <= 0)
            {
                IsAlive = false;
                _sr.sprite = Info.SpriteDead;
                gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
        }
    }
}
