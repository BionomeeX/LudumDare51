using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare51.MiniGame
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (transform.position.x < -8)
            {
                StartCoroutine(Loose());
            }
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(-200f, 0f) * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Bullet"))
            {
                Spawner.Instance.AddKill();
                Destroy(collision.collider.gameObject);
                Destroy(gameObject);
            }
        }

        private IEnumerator Loose()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null && p.activeInHierarchy)
            {
                p.SetActive(false);
                Spawner.Instance.Loose();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
