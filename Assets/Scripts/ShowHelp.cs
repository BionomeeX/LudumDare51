using LudumDare51.Translation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LudumDare51
{
    internal class ShowHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string _helpText;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (string.IsNullOrWhiteSpace(_helpText)) return;
            Tooltip.Instance.Show(Translate.Instance.Tr(_helpText));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Instance.Hide();
        }
    }
}
