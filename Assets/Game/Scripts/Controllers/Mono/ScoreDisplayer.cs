using System;
using Game.Scripts.Enums;
using MessagePipe;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Controllers.Mono
{
    public class ScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private IDisposable _disposable;
        private ISubscriber<GeneralEvents, object> _generalEventsSubscriber;
        private int _score;

        [Inject]
        private void Setup(ISubscriber<GeneralEvents,object> generalEventsSubscriber)
        {
            _generalEventsSubscriber = generalEventsSubscriber;
        }

        private void OnEnable()
        {
            var bag = DisposableBag.CreateBuilder();
            _generalEventsSubscriber.Subscribe(GeneralEvents.OnCellsMatched, OnCellsMatched).AddTo(bag);
            _generalEventsSubscriber.Subscribe(GeneralEvents.OnGridGenerateRequested, OnGridGenerateRequested).AddTo(bag);
            _disposable = bag.Build();   
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        private void OnGridGenerateRequested(object obj)
        {
            //this event send me grid size but i will not use it. this is just for resetting score
            ResetScore();
        }

        private void ResetScore()
        {
            _score = 0;
            UpdateScore();
        }

        private void OnCellsMatched(object obj)
        {
            //this event send me how much cell matched but i will not use it
            _score++;
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreText.text = $"Match Count : { _score.ToString() } ";
        }
    }
}
