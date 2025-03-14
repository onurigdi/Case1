using Game.Scripts.Enums;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Scripts.Mono
{
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        #region Serialized Fields

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer xSpriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider2D;

        #endregion

        #region Private Fields

        private bool _isMarked;
        private IPublisher<GeneralEvents, object> _generalEventsPublisher;

        #endregion

        #region Properties

        public bool IsMarked => _isMarked;

        #endregion

        [Inject]
        private void Setup(IPublisher<GeneralEvents,object> generalEventsPublisher)
        {
            _generalEventsPublisher = generalEventsPublisher;
        }

        #region Public Methods

        public void SetSize(Vector2 size)
        {
            spriteRenderer.size = size;
            xSpriteRenderer.size = size;
            boxCollider2D.size = size;
            spriteRenderer.transform.localScale = Vector3.one;
            xSpriteRenderer.transform.localScale = Vector3.one;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ChangeMarked(bool isMarked)
        {
            _isMarked = isMarked;
            xSpriteRenderer.gameObject.SetActive(_isMarked);
            
            if (_isMarked)
                _generalEventsPublisher?.Publish(GeneralEvents.OnCellMarked, this);
        }

        #endregion

        #region Click Detection

        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeMarked(true);
        }

        #endregion
    }
}