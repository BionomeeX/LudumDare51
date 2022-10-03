using LudumDare51.Tower;
using UnityEngine;

namespace LudumDare51.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/TowerInfo", fileName = "TowerInfo")]
    public class TowerInfo : ScriptableObject
    {
        public float Range;
        public float ReloadTime;
        public int SplashDamageRange;
        public GameObject Bullet;
        public DamageInfo[] DamageModifiers;
        public Sprite Sprite;
        public Sprite WeaponSprite;
        public Sprite Hat;
        public int Damage;
        public float SpeedModifierForce;
        public float SpeedModifierDuration;
        public bool CleanAll;
        public bool UseFire;
        public float Spread;
        public int NumberBullets = 1;
        public bool TargetDeadPeople;
        public float MinRange;
        public float BulletSpeed = 10f;
        public AudioClip shootSound;
        public AudioClip touchSound;
    }
}
