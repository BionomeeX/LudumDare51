using TMPro;
using UnityEngine;

namespace LudumDare51.Enemy
{
    public class Eater : MonoBehaviour
    {
        [SerializeField]
        private Node _target;

        [SerializeField]
        private Sprite[] _angrySprites;

        [SerializeField]
        private TMP_Text _textInfo;

        private SpriteRenderer _sr;

        public Node Goal => _target;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            EaterManager.Instance.Register(this);
        }

        public void AddEat()
        {
            EaterManager.Instance.AddEat(this);
        }

        public void UpdateSprite(int score)
        {
            _sr.sprite = _angrySprites[score / 10];
            if (score == 0)
            {
                _textInfo.text = string.Empty;
            }
            else
            {
                _textInfo.text = $"+{score}";
            }
        }
    }
}
