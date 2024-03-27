using UnityEngine;
using TMPro;

namespace QuizMono
{
    [RequireComponent(typeof(IQuestionStageAnimated))]
    public class QuestionTextHandler : MonoBehaviour, IQuestionTextHandlered // Отвечает за изменение текста вопроса
    {
        [SerializeField] private TextMeshProUGUI questionText;
        private new IQuestionStageAnimated animation;
        private void Awake()
        {
            animation = GetComponent<IQuestionStageAnimated>();
        }
        public void UpdateQuestion(string newQuestion)
        {
            questionText.text = "Find: " + newQuestion;
        }
        public void UpdateStage(int stage)
        {
            if (stage == 0)
            {
                animation.PlaySpawnAnimation(questionText);
            }
        }
    }
}
