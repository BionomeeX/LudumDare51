using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51
{
    public class OnClick : MonoBehaviour
    {

        [SerializeField]
        private Camera _cam;

        [SerializeField]
        private int _gridsize = 32;

        [SerializeField]
        private GameObject _tower;

        [SerializeField]
        private GameObject _radialMenu;

        private List<GameObject> _towers = new();

        public bool active = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        private bool TowerExistOnPos(GameObject tower, Vector3 posOnTheWorld){

            // Floor and Cast to int to be sure not to fiddle with strange floating point comparison behavior
            if (
                Mathf.FloorToInt(tower.transform.position[0]) == Mathf.FloorToInt(posOnTheWorld[0]) &&
                Mathf.FloorToInt(tower.transform.position[1]) == Mathf.FloorToInt(posOnTheWorld[1])
            ) {
                return true;
            }
            return false;
        }

        private int WhichTowerExistsHere(Vector3 posOnTheWorld) {
            for(int i = 0; i < _towers.Count; ++i)
            {
                if(TowerExistOnPos(_towers[i], posOnTheWorld)) {
                    return i;
                }
            }
            return -1;
        }

        private void ClickOnEmptyCase(Vector3 posOnTheWorld) {
            var newtower = Instantiate(
                _tower,
                posOnTheWorld,
                Quaternion.identity
            );

            _towers.Add(newtower);
        }

        private void ClickOnATower(int indexTowerHere, Vector3 posOnTheWorld) {
            active = false;
            var radialMenu = Instantiate(
                _radialMenu,
                posOnTheWorld, Quaternion.identity
            );
            var radmenu = radialMenu.GetComponent<RadialMenu>();
            radmenu.tower = _towers[indexTowerHere];
            radmenu.myPosOnTheWorld = posOnTheWorld;
            radmenu.parent = this;
            radmenu.cam = _cam;
            //_towers[indexTowerHere].GetComponent<Tower>().ModifyType();
        }

        public void Click(InputAction.CallbackContext value)
        {
            if (active && value.performed)
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

                var indexTowerHere = WhichTowerExistsHere(posOnTheWorld);
                // check if there is already a turret
                if (indexTowerHere > -1)
                {
                    ClickOnATower(indexTowerHere, posOnTheWorld);
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
            //Debug.Log(Screen.width);
            //Debug.Log(Screen.height);

            var x = 1 + Mathf.FloorToInt((_cam.pixelWidth - 1) / _gridsize);
            var y = 1 + Mathf.FloorToInt((_cam.pixelHeight - 1) / _gridsize);

            for(var i = 0 ; i < x; ++i){
                Gizmos.color = Color.white;

                var start = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, 0, _cam.nearClipPlane));
                var stop = _cam.ScreenToWorldPoint(new Vector3(i * _gridsize, _cam.pixelHeight, _cam.nearClipPlane));

                Gizmos.DrawLine(start, stop);
            }
            for(var i = 0 ; i < y; ++i){
                Gizmos.color = Color.white;

                var start = _cam.ScreenToWorldPoint(new Vector3(0, i * _gridsize, _cam.nearClipPlane));
                var stop = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, i * _gridsize, _cam.nearClipPlane));

                Gizmos.DrawLine(start, stop);
            }
        }
    }
}
