using LudumDare51.Enemy;
using LudumDare51.SO;
using LudumDare51.Tower;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51
{
    public class OnClick : MonoBehaviour
    {
        private Camera _cam;
        private int _gridsize;

        private LineRenderer _lr;

        [SerializeField]
        private LineRenderer _closeLr;

        [SerializeField]
        private GameObject _tower;

        [SerializeField]
        private RadialMenu _radialMenu;

        public TowerInfo CurrentTowerInfo {private set; get;}

        [SerializeField]
        private TowerInfo[] _info;

        [SerializeField]
        private GameObject _explosionPrefab;

        public TowerInfo[] Info => _info;

        private readonly List<GameObject> _towers = new();

        public static OnClick Instance {private set; get;}

        private int _layerTower;

        public void AddExplosion(Vector2 pos)
        {
            var go = Instantiate(_explosionPrefab, pos, Quaternion.identity);
            Destroy(go, 0.7f);
        }

        private void Awake()
        {

            Instance = this;

            _lr = GetComponent<LineRenderer>();

            _cam = Camera.main;

            _layerTower = 1 << LayerMask.NameToLayer("TowerSlot");

            //var worldScale = _cam.ViewportToWorldPoint(Vector3.one);

            _gridsize = Mathf.FloorToInt(
                _tower.GetComponent<CircleCollider2D>().radius * _tower.transform.localScale.x
            );

            // Debug.Log(worldScale[0]);
            // Debug.Log(worldScale[1]);
            // Debug.Log(worldScale[2]);

            // Debug.Log(_tower.transform.localScale.x);
            // Debug.Log(_tower.GetComponent<CircleCollider2D>().radius);
            // Debug.Log(_gridsize);

        }

        private bool TowerExistOnPos(GameObject tower, Vector3 posOnTheWorld){

            // Floor and Cast to int to be sure not to fiddle with strange floating point comparison behavior
            return
                Mathf.FloorToInt(tower.transform.position[0]) == Mathf.FloorToInt(posOnTheWorld[0]) &&
                Mathf.FloorToInt(tower.transform.position[1]) == Mathf.FloorToInt(posOnTheWorld[1])
            ;
        }

        private GameObject WhichTowerExistsHere(Vector3 posOnTheWorld) {
            return _towers.FirstOrDefault(x => TowerExistOnPos(x, posOnTheWorld));
        }


        public void AddTower(Vector3 position, TurretSpot spot)
        {
            if (CurrentTowerInfo != null) // TODO: check if there is no turret already there
            {
                var newtower = Instantiate(
                    _tower,
                    position,
                    Quaternion.identity
                );

                var ai = newtower.GetComponent<TowerAI>();
                ai.Info = CurrentTowerInfo;
                _towers.Add(newtower);
                EnemySpawner.Instance.RemoveTower(Array.IndexOf(_info, CurrentTowerInfo));
                spot.Turret = ai;
                CurrentTowerInfo = null;
            }
        }

        private TowerAI _lastHover;
        private void Update()
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, _layerTower);
            if (hit.collider != null && hit.collider.CompareTag("TowerSlot"))
            {
                var slot = hit.collider.GetComponent<TurretSpot>();
                TowerInfo target;
                float m = 1f;
                if (slot.Turret == null && CurrentTowerInfo != null)
                {
                    target = CurrentTowerInfo;
                    if (_lastHover != null)
                    {
                        _lastHover.ToggleBlush(false);
                        _lastHover = null;
                    }
                }
                else if (slot.Turret != null && CurrentTowerInfo == null)
                {
                    target = slot.Turret.Info;
                    if (slot.Turret.HasHat)
                    {
                        m = 1.5f;
                    }
                    if (_lastHover != slot.Turret)
                    {
                        if (_lastHover != null)
                        {
                            _lastHover.ToggleBlush(false);
                        }
                        _lastHover = slot.Turret;
                        _lastHover.ToggleBlush(true);
                    }
                }
                else
                {
                    if (_lastHover != null)
                    {
                        _lastHover.ToggleBlush(false);
                        _lastHover = null;
                    }
                    _lr.enabled = false;
                    _closeLr.enabled = false;
                    return;
                }

                _lr.enabled = true;

                // https://forum.unity.com/threads/drawing-simple-pixel-perfect-shapes-circles-lines-etc-like-in-gamemaker.479181/
                var thetaScale = 0.01f;
                float sizeValue = (2.0f * Mathf.PI) / thetaScale;
                var size = (int)sizeValue;
                size++;
                _lr.positionCount = size;
                var p = hit.collider.transform.position;

                var r = target.Range * m * 2f;
                Vector3 pos;
                float theta = 0f;
                for (int i = 0; i < size; i++)
                {
                    theta += (2.0f * Mathf.PI * thetaScale);
                    float x = r * Mathf.Cos(theta) + p.x;
                    float y = r * Mathf.Sin(theta) + p.y;
                    pos = new Vector3(x, y, 0);
                    _lr.SetPosition(i, pos);
                }

                _closeLr.enabled = target.MinRange > 0f;
                if (target.MinRange > 0f)
                {
                    _closeLr.positionCount = size;
                    var cr = target.MinRange * 2f;
                    theta = 0f;
                    for (int i = 0; i < size; i++)
                    {
                        theta += (2.0f * Mathf.PI * thetaScale);
                        float x = cr * Mathf.Cos(theta) + p.x;
                        float y = cr * Mathf.Sin(theta) + p.y;
                        pos = new Vector3(x, y, 0);
                        _closeLr.SetPosition(i, pos);
                    }
                }
            }
            else
            {
                _lr.enabled = false;
                _closeLr.enabled = false;
                if (_lastHover != null)
                {
                    _lastHover.ToggleBlush(false);
                    _lastHover = null;
                }
            }
        }

        public void Click(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, _layerTower);
                if (hit.collider != null && hit.collider.CompareTag("TowerSlot"))
                {
                    var spot = hit.collider.GetComponent<TurretSpot>();
                    if (spot.Turret == null)
                    {
                        AddTower(hit.collider.transform.position, hit.collider.GetComponent<TurretSpot>());
                    }
                    else if (CurrentTowerInfo == null)
                    {
                        if (_currSelection == CurrSelection.Trash)
                        {
                            Destroy(spot.Turret.gameObject);
                            spot.Turret = null;
                            _currSelection = CurrSelection.None;
                            EnemySpawner.Instance.RemoveDelete();
                        }
                        else if (_currSelection == CurrSelection.Hat && !spot.Turret.HasHat)
                        {
                            spot.Turret.SetHat();
                            _currSelection = CurrSelection.None;
                            EnemySpawner.Instance.RemoveHat();
                        }
                    }
                }
            }
        }

        public void SetTowerInfo(int infoIndex)
        {
            CurrentTowerInfo = _info[infoIndex];
        }

        public void SetSelectionTrash()
        {
            CurrentTowerInfo = null;
            _currSelection = CurrSelection.Trash;
        }

        public void SetSelectionHat()
        {
            CurrentTowerInfo = null;
            _currSelection = CurrSelection.Hat;
        }

        private enum CurrSelection
        {
            None,
            Trash,
            Hat
        }
        private CurrSelection _currSelection;
    }
}
