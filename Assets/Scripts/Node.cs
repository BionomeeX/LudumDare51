using UnityEngine;

namespace LudumDare51
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private GameObject _nextNode;

        public GameObject NextNode => _nextNode;

        private void OnDrawGizmos()
        {
            if (NextNode != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, NextNode.transform.position);
            }
        }
    }
}
