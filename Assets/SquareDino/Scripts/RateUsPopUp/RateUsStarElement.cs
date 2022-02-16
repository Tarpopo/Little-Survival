using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SquareDino.Scripts.RateUsPopUp
{
    public class RateUsStarElement : MonoBehaviour, IPointerClickHandler
    {
        public event System.Action<RateUsStarElement> OnSelected;

        [SerializeField] private GameObject star;

        private bool _enabled;

        public void Enable(bool enable)
        {
            _enabled = enable;
            star.SetActive(enable);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }
    }
}