using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51
{

    public class RadialMenu : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _buttons_prefab;

        public GameObject tower;

        [SerializeField]
        private float _radius = 100;

        private float _angle_rad;

        public Vector3 myPosOnTheWorld;

        private List<GameObject> _buttons = new();

        public OnClick parent;
        public Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            if(_buttons_prefab.Count > 0) {
                _angle_rad = 2 * Mathf.PI / _buttons_prefab.Count;
            }

            for(int i=0; i < _buttons_prefab.Count; ++i){
                // create _button with center at (pos + cos(alpha) * r, pos + sin(alpha) * r)

                var buttonPosX = myPosOnTheWorld[0] + Mathf.Cos(_angle_rad * (i + 1)) * _radius;
                var buttonPosY = myPosOnTheWorld[1] + Mathf.Sin(_angle_rad * (i + 1)) * _radius;

                var button = Instantiate(
                    _buttons_prefab[i],
                    new Vector3(buttonPosX, buttonPosY, myPosOnTheWorld[2]),
                    Quaternion.identity
                );
                _buttons.Add(button);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }


        private bool IsTheButtonHere(GameObject button, Vector3 posOnTheWorld)
        {
            var sr = button.GetComponent<SpriteRenderer>();
            if( sr.bounds.Contains(posOnTheWorld) ) {
                return true;
            }
            return false;
        }

        private int WhichButtonWasClicked(Vector3 posOnTheWorld){
            for(int i=0; i < _buttons.Count; ++i){
                if(IsTheButtonHere(_buttons[i], posOnTheWorld)) {
                    return i;
                }
            }
            return -1;
        }


        public void Click(InputAction.CallbackContext value) {
            if(value.performed){
                var pos = Mouse.current.position.ReadValue();

                var posOnTheWorld = cam.ScreenToWorldPoint(
                    new Vector3(
                        pos[0],
                        pos[1],
                        cam.nearClipPlane
                    )
                );

                // get which button was clicked
                int indexButton = WhichButtonWasClicked(posOnTheWorld);
                Debug.Log($"Button {indexButton} clicked !");
            }
            // in any cases
            parent.active = true;
            foreach(var button in _buttons){
                Destroy(button);
            }
            Destroy(this);
        }
    }

}
