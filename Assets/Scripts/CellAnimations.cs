using DG.Tweening;
using UnityEngine;

namespace QuizMono
{
    [RequireComponent(typeof(ParticleSystem))]
    public class CellAnimations : MonoBehaviour, IAnswerAnimation // јнимации дл€ €чеек
    {
        [SerializeField] private float spawnAnswerPowerAnimation;
        [SerializeField] private float spawnDuractionAimation;

        [SerializeField] private float correctAnswerPowerAnimation;
        [SerializeField] private float correctAnswerDuractionAnimation;

        [SerializeField] private float incorrectAnswerPowerAnimation;
        [SerializeField] private float incorrectAnswerDuractionAnimation;


        private bool playIncorrectAnimationIsPlaing = false;

        public void PlayIncorrectAnimation()
        {
            if (!playIncorrectAnimationIsPlaing)
            {
                Transform targetTransform = transform.GetChild(1);
                Sequence sequence = DOTween.Sequence();
                playIncorrectAnimationIsPlaing = true;
                sequence.Append(targetTransform.DOLocalMoveX(incorrectAnswerPowerAnimation, incorrectAnswerDuractionAnimation).SetEase(Ease.InOutQuad));
                sequence.Append(targetTransform.DOLocalMoveX(-incorrectAnswerPowerAnimation, incorrectAnswerDuractionAnimation).SetEase(Ease.InOutQuad));
                sequence.Append(targetTransform.DOLocalMoveX(Vector3.zero.x, incorrectAnswerDuractionAnimation).SetEase(Ease.InOutQuad)).OnComplete(() => playIncorrectAnimationIsPlaing = false);
            }
        }

        public void PlaySpawnAnimation()
        {
            transform.DOPunchScale(new Vector3(spawnAnswerPowerAnimation, spawnAnswerPowerAnimation, spawnAnswerPowerAnimation), spawnDuractionAimation);
        }

        public void PlayCorrectAnimation()
        {
            if (!playIncorrectAnimationIsPlaing)
            {
                Transform targetTransform = transform.GetChild(1);
                targetTransform.DOPunchScale(new Vector3(correctAnswerPowerAnimation, correctAnswerPowerAnimation, correctAnswerPowerAnimation), correctAnswerDuractionAnimation);
                PlayParticle();
            }
        }

        public void PlayParticle()
        {
            GetComponent<ParticleSystem>().Play();
        }

        public void StandStill()
        {
            playIncorrectAnimationIsPlaing = true;
        }
    }
}
