using DG.Tweening;
using GameData;
using System.Collections;
using UnityEngine;

namespace QuizMono
{
    public class ChangeStage : MonoBehaviour // Отвечает за смену стадии  случае выиграша
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellSpacing;
        [SerializeField] private StageDataSO stageData;
        [SerializeField] private float timeOfPraise;

        private int stageGame = 0; // Текущая стадия (сложность)
        private IAnswer correctAnswer;
        private IGeneratebleGameField quizField;
        private IQuestionTextHandlered questionField;
        private IMenuOpener restartMenu;

        private void Start()
        {
            GameObject fieldCells = new GameObject();
            quizField = fieldCells.AddComponent<GenerateAnswerField>();
            questionField = GetComponent<IQuestionTextHandlered>();
            restartMenu = GetComponent<IMenuOpener>();
            restartMenu.GetObjectLoaded().RestartMenuEvent += StartStage;
            StartStage();
        }

        private void StartStage() // Начинает новую стадию
        {
            quizField.Generate(cellPrefab, cellSpacing, stageGame, stageData);
            questionField.UpdateStage(stageGame);
            correctAnswer = quizField.GetCorrectAnswerCell();
            questionField.UpdateQuestion(correctAnswer.GetName());
            correctAnswer.CellEvent += responseComparison;
        }

        private void responseComparison(string answer)
        {
            if (answer == correctAnswer.GetName())
            {
                stageGame++;
                StartCoroutine(ChangeStageDelay(timeOfPraise));
            }
        }

        private IEnumerator ChangeStageDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            DOTween.CompleteAll();
            DOTween.Clear();
            if (stageGame == stageData.countStage.Length)
            {
                stageGame = 0;
                for (int i = 0; i < quizField.GetFieldAnswer().transform.childCount; i++)
                quizField.GetFieldAnswer().transform.GetChild(i).GetComponent<IAnswerAnimation>().StandStill();
                restartMenu.OpenMenu();
            }
            else
            {
                StartStage();
            }
        }
        private void OnDisable()
        {
            correctAnswer.CellEvent -= responseComparison;
            restartMenu.GetObjectLoaded().RestartMenuEvent -= StartStage;
        }
    }
}
