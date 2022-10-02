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
        private GameObject _tower;

        [SerializeField]
        private RadialMenu _radialMenu;

        public TowerInfo CurrentTowerInfo {private set; get;}

        [SerializeField]
        private TowerInfo[] _info;

        public TowerInfo[] Info => _info;

        private readonly List<GameObject> _towers = new();

        public static OnClick Instance {private set; get;}

        private int _layerTower;

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

        private void ClickOnEmptyCase(Vector3 posOnTheWorld) {
            if (CurrentTowerInfo != null)
            {
                var newtower = Instantiate(
                    _tower,
                    posOnTheWorld,
                    Quaternion.identity
                );

                newtower.GetComponent<TowerAI>().Info = CurrentTowerInfo;
                _towers.Add(newtower);
            }
        }

        private void ClickOnATower(GameObject tower, Vector3 posOnTheWorld) {
            _radialMenu.MyPosOnTheWorld = posOnTheWorld;
            _radialMenu.gameObject.SetActive(true);
        }


        public void AddTower(Vector3 position)
        {
            if (CurrentTowerInfo != null) // TODO: check if there is no turret already there
            {
                var newtower = Instantiate(
                    _tower,
                    position,
                    Quaternion.identity
                );

                newtower.GetComponent<TowerAI>().Info = CurrentTowerInfo;
                _towers.Add(newtower);
                EnemySpawner.Instance.RemoveTower(Array.IndexOf(_info, CurrentTowerInfo));
                CurrentTowerInfo = null;
            }
        }

        private void Update()
        {
            if (CurrentTowerInfo != null)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, _layerTower);
                if (hit.collider != null && hit.collider.CompareTag("TowerSlot"))
                {
                    _lr.enabled = true;

                    // https://forum.unity.com/threads/drawing-simple-pixel-perfect-shapes-circles-lines-etc-like-in-gamemaker.479181/
                    var thetaScale = 0.01f;
                    float sizeValue = (2.0f * Mathf.PI) / thetaScale;
                    var size = (int)sizeValue;
                    size++;
                    _lr.positionCount = size;
                    var p = hit.collider.transform.position;

                    var r = CurrentTowerInfo.Range * 2f;
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
                }
                else
                {
                    _lr.enabled = false;
                }
            }
            else
            {
                _lr.enabled = false;
            }
        }

        public void Click(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, _layerTower);
                if (hit.collider != null && hit.collider.CompareTag("TowerSlot"))
                {
                    AddTower(hit.collider.transform.position);
                }
            }
        }

        public void SetTowerInfo(int infoIndex)
        {
            CurrentTowerInfo = _info[infoIndex];
        }
    }
}
