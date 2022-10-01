using UnityEngine;

namespace LudumDare51.Enemy
{
    public class Eater : MonoBehaviour
    {
        [SerializeField]
        private Node _target;

        public Node Goal => _target;
    }
}
