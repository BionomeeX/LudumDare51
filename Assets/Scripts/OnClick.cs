using LudumDare51.SO;
using LudumDare51.Tower;
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

        [SerializeField]
        private GameObject _tower;

        [SerializeField]
        private RadialMenu _radialMenu;

        public TowerInfo CurrentTowerInfo {private set; get;}

        [SerializeField]
        private TowerInfo[] _info;

        private readonly List<GameObject> _towers = new();

        public static OnClick Instance {private set; get;}

        private int _layerTower;

        private void Awake()
        {

            Instance = this;

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
                CurrentTowerInfo = null;
            }
        }


        public void Click(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, _layerTower);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("TowerSlot"))
                    {
                        AddTower(hit.collider.transform.position);
                    }
                }  
            }
            // if (value.performed)
            // {
            //     if (_radialMenu.gameObject.activeInHierarchy)
            //     {
            //         _radialMenu.Click();
            //         _radialMenu.gameObject.SetActive(false);
            //     }
            //     else
            //     {
            //         var pos = Mouse.current.position.ReadValue();
            //         pos /= _gridsize;
            //         pos[0] = Mathf.Floor(pos[0]);
            //         pos[1] = Mathf.Floor(pos[1]);

            //         // Compute world position of the center of the case where the mouse clicked
            //         var posOnTheWorld = _cam.ScreenToWorldPoint(
            //             new Vector3(
            //                 pos[0] * _gridsize + _gridsize / 2,
            //                 pos[1] * _gridsize + _gridsize / 2,
            //                 _cam.nearClipPlane
            //             )
            //         );

            //         var towerHere = WhichTowerExistsHere(posOnTheWorld);
            //         // check if there is already a turret
            //         if (towerHere != null)
            //         {
            //             ClickOnATower(towerHere, posOnTheWorld);
            //         }
            //         // Tower existing, modify tower type
            //         else
            //         {
            //             ClickOnEmptyCase(posOnTheWorld);
            //         }
            //     }
            // }
        }

        private void OnDrawGizmos()
        {
            // if (EditorApplication.isPlaying)
            // {
            //     var x = 1 + Mathf.FloorToInt((_cam.pixelWidth - 1) / _gridsize);
            //     var y = 1 + Mathf.FloorToInt((_cam.pixelHeight - 1) / _gridsize);
            //     Gizmos.color = new Color(1f, 1f, 1f, .5f);

            //     for (var i = 0; i < x; ++i)
            //     {

            //         var start = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, 0, _cam.nearClipPlane));
            //         var stop = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, _cam.pixelHeight, _cam.nearClipPlane));

            //         Gizmos.DrawLine(start, stop);
            //     }
            //     for (var i = 0; i < y; ++i)
            //     {

            //         var start = _cam.ScreenToWorldPoint(new Vector3(0, i * _gridsize, _cam.nearClipPlane));
            //         var stop = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, i * _gridsize, _cam.nearClipPlane));

            //         Gizmos.DrawLine(start, stop);
            //     }
            // }
        }

        public void SetTowerInfo(int infoIndex)
        {
            CurrentTowerInfo = _info[infoIndex];
        }
    }
}
