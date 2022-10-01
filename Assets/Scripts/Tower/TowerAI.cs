using LudumDare51.Enemy;
using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare51.Tower
{
    public class TowerAI : MonoBehaviour
    {
        public TowerInfo Info { set; private get; }

        private List<EnemyAI> _enemiesInRange = new();

        private bool _canShoot = true;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = Info.Range;
            GetComponent<SpriteRenderer>().sprite = Info.Sprite;
        }

        private void Update()
        {
            if (_canShoot)
            {
                _enemiesInRange.RemoveAll(x => x.gameObject == null);
                if (_enemiesInRange.Count > 0)
                {
                    var bullet = Instantiate(Info.Bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
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
            yield return new WaitForSeconds(Info.ReloadTime);
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
