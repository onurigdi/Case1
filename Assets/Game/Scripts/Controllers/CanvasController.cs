using System;
using Game.Scripts.Enums;
using Game.Scripts.Events;
using MessagePipe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.Controllers
{
    public class CanvasController : MonoBehaviour
    {
        #region UI Elements References

        [SerializeField] private TextMeshProUGUI txtInfo;
        [SerializeField] private TMP_InputField inputSize;
        [SerializeField] private Button btnCreate;

        #endregion

        #region Zenject

        private IPublisher<GeneralEvents, object> _generalEventPublisher;

        [Inject]
        private void Setup(IPublisher<GeneralEvents, object> generalEventPublisher)
        {
            _generalEventPublisher = generalEventPublisher;
        }

        #endregion

        #region Unity

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        #endregion

        #region Private Methods

        private void AddListeners()
        {
            btnCreate?.onClick.AddListener(RequestGridGeneration);
        }

        private void RemoveListeners()
        {
            btnCreate?.onClick.RemoveAllListeners();
        }

        private void RequestGridGeneration()
        {
            Vector2Int gridSize;
            int sizeValue;
            if (int.TryParse(inputSize.text, out sizeValue) && sizeValue > 0)
            {
                gridSize = new Vector2Int(sizeValue, sizeValue);
            }
            else
            {
                Debug.LogWarning("Not a valid integer value");
                gridSize = new Vector2Int(5, 5); // Varsayılan bir değer ver
            }
            
            _generalEventPublisher?.Publish(GeneralEvents.OnGridGenerateRequested,
                new GridGenerateRequest(gridSize));
        }

        #endregion
    }
}