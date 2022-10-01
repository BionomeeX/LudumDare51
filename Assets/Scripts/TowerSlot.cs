using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare51
{
    public class TowerSlot : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            OnClick.Instance.AddTower(this.transform.position);
        }

    }
}
