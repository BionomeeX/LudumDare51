using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare51
{

    public class RadialMenu : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _buttons_prefab;

        [SerializeField]
        private float _radius = 100;

        private float _angle_rad;

        private Vector3 _myPosOnTheWorld;
        public Vector3 MyPosOnTheWorld
        {
            set
            {
                _myPosOnTheWorld = value;
                for (int i = 0; i < _buttons.Count; i++)
                {
                    var b = _buttons[i];
                    // center at (pos + cos(alpha) * r, pos + sin(alpha) * r)
                    var buttonPosX = MyPosOnTheWorld[0] + Mathf.Cos(_angle_rad * (i + 1)) * _radius;
                    var buttonPosY = MyPosOnTheWorld[1] + Mathf.Sin(_angle_rad * (i + 1)) * _radius;

                    b.transform.position = new Vector3(buttonPosX, buttonPosY, MyPosOnTheWorld[2]);
                }
            }
            get => _myPosOnTheWorld;
        }

        private readonly List<GameObject> _buttons = new();

        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;

            if (_buttons_prefab.Count > 0)
            {
                _angle_rad = 2 * Mathf.PI / _buttons_prefab.Count;
            }
            for (int i = 0; i < _buttons_prefab.Count; ++i)
            {
                _buttons.Add(Instantiate(
                    _buttons_prefab[i],
                    transform
                ));
            }

            gameObject.SetActive(false);
        }

        private bool IsTheButtonHere(GameObject button, Vector3 posOnTheWorld)
        {
            var sr = button.GetComponent<SpriteRenderer>();
            return sr.bounds.Contains(posOnTheWorld);
        }

        private GameObject WhichButtonWasClicked(Vector3 posOnTheWorld)
        {
            return _buttons.FirstOrDefault(x => IsTheButtonHere(x, posOnTheWorld));
        }

        public void Click()
        {
            var pos = Mouse.current.position.ReadValue();

            var posOnTheWorld = _cam.ScreenToWorldPoint(
                new Vector3(
                    pos[0],
                    pos[1],
                    _cam.nearClipPlane
                )
            );

            // get which button was clicked
            var button = WhichButtonWasClicked(posOnTheWorld);
            if (button != null)
            {
                Debug.Log($"Button {button.GetInstanceID()} clicked !");
            }
        }
    }
}
