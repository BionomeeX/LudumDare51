using UnityEngine;

namespace LudumDare51.Enemy
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private Node _nextNode;

        public Node NextNode => _nextNode;

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
