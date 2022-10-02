using LudumDare51.Enemy;
using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LudumDare51.Tower
{
    public class TowerAI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _fireChildPivot;

        [SerializeField]
        private Collider2D _flameCollider;

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
            _enemiesInRange.RemoveAll(x => x.gameObject == null);
            var target = _enemiesInRange.FirstOrDefault(x => (Info.TargetDeadPeople || x.IsAlive) && (Info.MinRange <= 0f || Vector2.Distance(x.transform.position, transform.position) > Info.MinRange));
            if (Info.UseFire)
            {
                _fireChildPivot.SetActive(target != null);
                if (target != null)
                {
                    Vector3 targetPos = target.transform.position;
                    Vector2 direction = targetPos - transform.position;
                    var euler = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles;
                    transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z + 90f);
                    if (_canShoot)
                    {
                        List<Collider2D> res = new();
                        Physics2D.OverlapCollider(_flameCollider, new ContactFilter2D(), res);
                        foreach (var c in res)
                        {
                            if (c.CompareTag("Enemy"))
                            {
                                c.GetComponent<EnemyAI>().TakeDamage(Info);
                            }
                        }
                        StartCoroutine(Reload());
                        _canShoot = false;
                    }
                }
            }
            else
            {
                if (_canShoot && target != null)
                {
                    for (var i = 0; i < Info.NumberBullets; i++)
                    {
                        var bullet = Instantiate(Info.Bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
                        bullet.Speed = 10;
                        bullet.Target = target.transform.position + new Vector3(Random.Range(-Info.Spread, Info.Spread), Random.Range(-Info.Spread, Info.Spread));
                        bullet.Info = Info;
                        Destroy(bullet.gameObject, 5f);
                        StartCoroutine(Reload());
                        _canShoot = false;
                    }
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
                var enemy = collision.GetComponent<EnemyAI>();
                if (enemy.IsAlive)
                {
                    _enemiesInRange.Add(enemy);
                }
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
