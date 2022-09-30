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

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            transform.LookAt(_nextNode.transform.position, Vector3.forward);
            _rb.velocity = transform.up * Time.fixedDeltaTime * _info.Speed;
            if (Vector2.Distance(transform.position, _nextNode.transform.position) < .1f)
            {
                _nextNode = _nextNode.NextNode;
            }
        }
    }
}
