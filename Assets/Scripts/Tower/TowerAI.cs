using LudumDare51.Enemy;
using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare51
{
    public class TowerAI : MonoBehaviour
    {
        [SerializeField]
        private TowerInfo _info;

        private List<EnemyAI> _enemiesInRange = new();

        private bool _canShoot = true;

        private void Awake()
        {
            GetComponent<CircleCollider2D>().radius = _info.Range;
            GetComponent<SpriteRenderer>().color = _info.Color;
        }

        private void Update()
        {
            if (_canShoot)
            {
                _enemiesInRange.RemoveAll(x => x.gameObject == null);
                if (_enemiesInRange.Count > 0)
                {
                    var bullet = Instantiate(_info.Bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.Speed = 50;
                    bullet.Target = _enemiesInRange[0].transform.position;
                    Destroy(bullet.gameObject, 5f);
                    StartCoroutine(Reload());
                    _canShoot = false;
                }
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_info.ReloadTime);
            _canShoot = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                _enemiesInRange.Add(collision.GetComponent<EnemyAI>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                _enemiesInRange.Remove(collision.GetComponent<EnemyAI>());
            }
        }

        public void ModifyType(){
            Debug.Log("Type modified");
        }
    }
}
