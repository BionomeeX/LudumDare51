using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare51
{
    public class Tower : MonoBehaviour
    {
        [SerializeField]
        private TowerInfo _info;

        private List<Enemy> _enemiesInRange = new();

        private bool _canShoot = true;

        private void Awake()
        {
            GetComponent<CircleCollider2D>().radius = _info.Range;
        }

        private void Update()
        {
            if (_canShoot)
            {
                _canShoot = false;
                _enemiesInRange.RemoveAll(x => x.gameObject == null);
                if (_enemiesInRange.Count > 0)
                {
                    var bullet = Instantiate(_info.Bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.Speed = 50;
                    bullet.Target = _enemiesInRange[0].transform.position;
                    Destroy(bullet.gameObject, 5f);
                    StartCoroutine(Reload());
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
                _enemiesInRange.Add(collision.GetComponent<Enemy>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                _enemiesInRange.Remove(collision.GetComponent<Enemy>());
            }
        }

        public void ModifyType(){
            Debug.Log("Type modified");
        }
    }
}
