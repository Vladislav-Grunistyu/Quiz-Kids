using DG.Tweening;
using UnityEngine;

namespace QuizMono
{
    public class RestartMenuOpener : MonoBehaviour, IMenuOpener // �������� �� ��������� ���� � ������ "����������� ����"
    {
        [SerializeField] private CanvasGroup restartMenuBackground;
        [SerializeField] private CanvasGroup restartMenuButton;
        [SerializeField] private CanvasGroup loadNewGameFade;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float loadTime;

        public delegate void RestartMenuEventHandler();

        public event RestartMenuEventHandler RestartMenuEvent;

        public void OpenMenu() // ���������� ���� � �������
        {
            restartMenuBackground.DOFade(1f, fadeDuration);
            restartMenuBackground.blocksRaycasts = true;
            restartMenuBackground.interactable = true;
            restartMenuButton.DOFade(1f, fadeDuration);
            restartMenuButton.blocksRaycasts = true;
            restartMenuButton.interactable = true;
        }

        public void StartLoadScreen() // ��������� ��������� �����
        {
            loadNewGameFade.DOFade(1f, fadeDuration)
                .OnComplete(() => LoadingScreenActive());
        }

        private void LoadingScreenActive() // ���� ����������, ������� ���� � �������
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

        private void EndLoanScreen() // ������� � ��� ��� ��� ������
        {
            RestartMenuEvent?.Invoke();
        }

        public RestartMenuOpener GetObjectLoaded()
        {
            return this;
        }
    }
}
