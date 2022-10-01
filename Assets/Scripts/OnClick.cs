using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        private readonly List<GameObject> _towers = new();

        public bool Active { set; private get; } = true;

        private void Awake()
        {
            _cam = Camera.main;
            _gridsize = Mathf.CeilToInt(_tower.transform.localScale.x * 50f);
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
            var newtower = Instantiate(
                _tower,
                posOnTheWorld,
                Quaternion.identity
            );

            _towers.Add(newtower);
        }

        private void ClickOnATower(GameObject tower, Vector3 posOnTheWorld) {
            Active = false;
            _radialMenu.Tower = tower;
            _radialMenu.MyPosOnTheWorld = posOnTheWorld;
            _radialMenu.gameObject.SetActive(true);
            //_towers[indexTowerHere].GetComponent<Tower>().ModifyType();
        }

        public void Click(InputAction.CallbackContext value)
        {
            if (Active && value.performed)
            {
                var pos = Mouse.current.position.ReadValue();
                pos /= _gridsize;
                pos[0] = Mathf.Floor(pos[0]);
                pos[1] = Mathf.Floor(pos[1]);

                // Compute world position of the center of the case where the mouse clicked
                var posOnTheWorld = _cam.ScreenToWorldPoint(
                    new Vector3(
                        pos[0] * _gridsize + _gridsize / 2,
                        pos[1] * _gridsize + _gridsize / 2,
                        _cam.nearClipPlane
                    )
                );

                var towerHere = WhichTowerExistsHere(posOnTheWorld);
                // check if there is already a turret
                if (towerHere != null)
                {
                    ClickOnATower(towerHere, posOnTheWorld);
                }
                // Tower existing, modify tower type
                else
                {
                    ClickOnEmptyCase(posOnTheWorld);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                var x = 1 + Mathf.FloorToInt((_cam.pixelWidth - 1) / _gridsize);
                var y = 1 + Mathf.FloorToInt((_cam.pixelHeight - 1) / _gridsize);

                for (var i = 0; i < x; ++i)
                {
                    Gizmos.color = Color.white;

                    var start = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, 0, _cam.nearClipPlane));
                    var stop = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, _cam.pixelHeight, _cam.nearClipPlane));

                    Gizmos.DrawLine(start, stop);
                }
                for (var i = 0; i < y; ++i)
                {
                    Gizmos.color = Color.white;

                    var start = _cam.ScreenToWorldPoint(new Vector3(0, i * _gridsize, _cam.nearClipPlane));
                    var stop = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, i * _gridsize, _cam.nearClipPlane));

                    Gizmos.DrawLine(start, stop);
                }
            }
        }
    }
}
