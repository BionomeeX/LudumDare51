using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnClick : MonoBehaviour
{
    [SerializeField]
        private int _gridsize = 32;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Click(InputAction.CallbackContext value) {
        if(value.performed){
            var pos = Mouse.current.position.ReadValue();
            pos /= _gridsize;
            pos[0] = Mathf.Floor(pos[0]);
            pos[1] = Mathf.Floor(pos[1]);
            Debug.Log(pos);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
