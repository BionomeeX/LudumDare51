using LudumDare51.Enemy;
using UnityEngine;

namespace LudumDare51.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/EnemyInfo", fileName = "EnemyInfo")]
    public class EnemyInfo : ScriptableObject
    {
        public float Speed;
        public int BaseHealth;
        public EnemyType Type;
    }
}
