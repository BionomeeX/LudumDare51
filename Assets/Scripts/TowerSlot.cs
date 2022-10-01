using UnityEngine;

namespace LudumDare51
{
    public class TowerSlot : MonoBehaviour
    {
        private void OnMouseDown()
        {
            OnClick.Instance.AddTower(transform.position);
        }

    }
}
