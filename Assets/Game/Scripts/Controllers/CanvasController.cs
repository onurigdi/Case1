using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.Controllers
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtInfo;
        [SerializeField] private TMP_InputField inputSize;
        [SerializeField] private Button btnCreate;

        
        [Inject]
        private void Setup()
        {

        }
    }
}
