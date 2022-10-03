using Assets.Scripts;
using LudumDare51.Enemy;
using LudumDare51.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace LudumDare51.Tower
{
    public class TowerAI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _weapon, _fireWeapon;

        [SerializeField]
        private Collider2D _flameCollider;

        public TowerInfo Info { set; get; }

        private List<EnemyAI> _enemiesInRange = new();

        private bool _canShoot = true;

        [SerializeField]
        private SpriteRenderer _weaponSR, _hatSR, _blushSR;

        public bool _hasHat = false;

        public bool HasHat => _hasHat;

        public void SetHat()
        {
            _hatSR.enabled = true;
            _hasHat = true;
            if (!Info.UseFire) // Fire has fixed range
            {
                GetComponent<CircleCollider2D>().radius *= 1.5f;
            }
        }

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = Info.Range;
            GetComponent<SpriteRenderer>().sprite = Info.Sprite;
            _weaponSR.sprite = Info.WeaponSprite;
            _hatSR.sprite = Info.Hat;
            _hatSR.enabled = false;
            _blushSR.enabled = false;
        }

        public void ToggleBlush(bool value) => _blushSR.enabled = value;

        private void Update()
        {
            _enemiesInRange.RemoveAll(x => x.gameObject == null);
            var target = _enemiesInRange.FirstOrDefault(x => (Info.TargetDeadPeople || x.IsAlive) && (Info.MinRange <= 0f || Vector2.Distance(x.transform.position, transform.position) > Info.MinRange));
            if (Info.UseFire)
            {
                _fireWeapon.SetActive(target != null);
                if (target != null)
                {
                    Vector3 targetPos = target.transform.position;
                    Vector2 direction = targetPos - transform.position;
                    var euler = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles;
                    transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z + 90f);
                    if (_canShoot)
                    {
                        if (Info.shootSound != null)
                            SFXManager.Instance.PlaySound(Info.shootSound);
                        List<Collider2D> res = new();
                        Physics2D.OverlapCollider(_flameCollider, new ContactFilter2D(), res);
                        foreach (var c in res)
                        {
                            if (c.CompareTag("Enemy"))
                            {
                                c.GetComponent<EnemyAI>().TakeDamage(Info, _hasHat);
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
                    if (Info.shootSound != null)
                        SFXManager.Instance.PlaySound(Info.shootSound);
                    Vector3 targetPos = target.transform.position;
                    Vector2 direction = targetPos - transform.position;
                    var euler = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles;
                    transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z + 90f);
                    for (var i = 0; i < Info.NumberBullets; i++)
                    {
                        var bullet = Instantiate(Info.Bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
                        bullet.Speed = Info.BulletSpeed;
                        bullet.Target = target.transform.position + new Vector3(Random.Range(-Info.Spread, Info.Spread), Random.Range(-Info.Spread, Info.Spread));
                        bullet.Info = Info;
                        bullet.HasHat = _hasHat;
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
