using DG.Tweening;
using UnityEngine;

namespace QuizMono
{
    public class RestartMenuOpener : MonoBehaviour, IMenuOpener // Отвечает за появление меню и эффект "Перезапуска игры"
    {
        [SerializeField] private CanvasGroup restartMenuBackground;
        [SerializeField] private CanvasGroup restartMenuButton;
        [SerializeField] private CanvasGroup loadNewGameFade;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float loadTime;

        public delegate void RestartMenuEventHandler();

        public event RestartMenuEventHandler RestartMenuEvent;

        public void OpenMenu() // Показывает меню с кнопкой
        {
            restartMenuBackground.DOFade(1f, fadeDuration);
            restartMenuBackground.blocksRaycasts = true;
            restartMenuBackground.interactable = true;
            restartMenuButton.DOFade(1f, fadeDuration);
            restartMenuButton.blocksRaycasts = true;
            restartMenuButton.interactable = true;
        }

        public void StartLoadScreen() // Полностью затемняет экран
        {
            loadNewGameFade.DOFade(1f, fadeDuration)
                .OnComplete(() => LoadingScreenActive());
        }

        private void LoadingScreenActive() // Пока затемнение, убирает меню с кнопкой
        {
            restartMenuButton.blocksRaycasts = false;
            restartMenuBackground.blocksRaycasts = false;
            restartMenuBackground.interactable = false;
            restartMenuButton.alpha = 0f;
            restartMenuBackground.alpha = 0f;
            DOVirtual.DelayedCall(loadTime, () =>
            {
                EndLoanScreen();
                loadNewGameFade.DOFade(0f, fadeDuration);
            });
        }

        private void EndLoanScreen() // Говорим о том что все готово
        {
            RestartMenuEvent?.Invoke();
        }

        public RestartMenuOpener GetObjectLoaded()
        {
            return this;
        }
    }
}
