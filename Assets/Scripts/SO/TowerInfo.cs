using LudumDare51.Tower;
using UnityEngine;

namespace LudumDare51.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/TowerInfo", fileName = "TowerInfo")]
    public class TowerInfo : ScriptableObject
    {
        public int Range;
        public int ReloadTime;
        public int SplashDamageRange;
        public GameObject Bullet;
        public DamageInfo[] DamageModifiers;
        public Sprite Sprite;
        public int Damage;
        public float SpeedModifierForce;
        public float SpeedModifierDuration;
    }
}
