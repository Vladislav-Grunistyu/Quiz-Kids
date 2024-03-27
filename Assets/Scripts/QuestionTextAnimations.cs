using DG.Tweening;
using TMPro;
using UnityEngine;

namespace QuizMono
{
    public class QuestionTextAnimations : MonoBehaviour, IQuestionStageAnimated // Анимация к полю текста с вопросом
    {
        [SerializeField] private float spawnQuestionDuractionAnimation;

        public void PlaySpawnAnimation(TextMeshProUGUI questionText)
        {
            questionText.DOFade(1f, spawnQuestionDuractionAnimation)
             .From(0f)
             .SetEase(Ease.InOutQuad);
        }
    }
}
