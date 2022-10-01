using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51
{
    public class OnClick : MonoBehaviour
    {

        [SerializeField]
        private Camera cam;

        [SerializeField]
        private int _gridsize = 32;

        [SerializeField]
        private GameObject _tower;

        private List<GameObject> _towers = new();

        // Start is called before the first frame update
        void Start()
        {

        }

        private bool TowerExistOnPos(GameObject tower, Vector2 pos){
            if (Mathf.FloorToInt(tower.transform.position[0] / _gridsize) == Mathf.FloorToInt(pos[0]) &&
                Mathf.FloorToInt(tower.transform.position[1] / _gridsize) == Mathf.FloorToInt(pos[1])) {
                return true;
            }
            return false;
        }

        private bool TowerExisteHere(Vector2 pos) {
            foreach(var tower in _towers) {
                if(TowerExistOnPos(tower, pos)) {
                    return true;
                }
            }
            return false;
        }

        public void Click(InputAction.CallbackContext value) {
            if(value.performed){
                var pos = Mouse.current.position.ReadValue();
                pos /= _gridsize;
                pos[0] = Mathf.Floor(pos[0]);
                pos[1] = Mathf.Floor(pos[1]);

                // check if there is already a turret
                if(!TowerExisteHere(pos)){
                    var position = Mouse.current.position.ReadValue();

                    var newtower = Instantiate(_tower, cam.ScreenToWorldPoint(new Vector3(position[0], position[1], cam.nearClipPlane)), Quaternion.identity);
                    _towers.Add(newtower);
                }

            }
        }

        private void OnDrawGizmos()
        {
            //Debug.Log(Screen.width);
            //Debug.Log(Screen.height);

            var x = Mathf.FloorToInt(cam.pixelWidth / _gridsize);
            var y = Mathf.FloorToInt(cam.pixelHeight / _gridsize);

            for(var i = 0 ; i < x; ++i){
                Gizmos.color = Color.white;

                var start = cam.ScreenToWorldPoint(new Vector3(i * _gridsize, 0, cam.nearClipPlane));
                var stop = cam.ScreenToWorldPoint(new Vector3(i * _gridsize, cam.pixelHeight, cam.nearClipPlane));

                Gizmos.DrawLine(start, stop);
            }
            for(var i = 0 ; i < y; ++i){
                Gizmos.color = Color.white;

                var start = cam.ScreenToWorldPoint(new Vector3(0, i * _gridsize, cam.nearClipPlane));
                var stop = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, i * _gridsize, cam.nearClipPlane));

                Gizmos.DrawLine(start, stop);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
