using GameData;
using System.Collections.Generic;
using UnityEngine;

namespace QuizMono
{
    public class GenerateAnswerField : MonoBehaviour, IGeneratebleGameField // �������� �� �������� ���� � �������� � �� ����� ������� (��� ����� ��������� ������ �� ���� ��������� ���� � �� ����������)
    {
        private List<string> usedQuestions = new List<string>();
        private int countUsedAnswer;
        private IAnswer correctAnswer;
        private GameObject fieldCenterObject;
        public void Generate(GameObject cellPrefab, float cellSpacing, int stageGame, StageDataSO data)
        {
            int columns = data.countStage[stageGame].countColums;
            int rows = data.countStage[stageGame].countLines;
            int[] randomAnswers = CreateRandomAnswers(data.countStage[stageGame] // ������� ��������� ������ ��������
                .countAnswer.Length, rows * columns);
            string question = SelectQuestion(data, randomAnswers, stageGame); // �������� ������
            ShuffleAnswerInArray(randomAnswers); // � ���������� � ��������� ����� ������

            Vector3 cellSize = cellPrefab.GetComponent<SpriteRenderer>().bounds.size; // �������� ������� ������ �� �������

            float fieldWidth = columns * (cellSize.x + cellSpacing) - cellSpacing; // ��������� ������ ����� ����
            float fieldHeight = rows * (cellSize.y + cellSpacing) - cellSpacing;

            Vector3 screenCenter = Camera.main
                .ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // �������� ����� ������ � ������� �����������

            Vector3 fieldCenter = new Vector3(screenCenter.x - fieldWidth / 2 + cellSize.x / 2, 
                screenCenter.y + fieldHeight / 2 - cellSize.y / 2, 0); // ��������� ���������� ������ ����

            if (fieldCenterObject != null)
            {
                Destroy(fieldCenterObject); // ���������� ������ ���� ���� ����
            }
            fieldCenterObject = new GameObject("Field");

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 position = fieldCenter + new Vector3(col * (cellSize.x + cellSpacing), // ��������� ������� ��� ����� ������ � ������ �������
                        -row * (cellSize.y + cellSpacing), 0);

                    GameObject newCell = Instantiate(cellPrefab, position, Quaternion.identity); // ����������� ���� �� �����
                    newCell.transform.SetParent(fieldCenterObject.transform);
                    IAnswer objectInNewCell = newCell.GetComponent<IAnswer>();

                    objectInNewCell.SetName(data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].name); // ��������� ������ ���������
                    objectInNewCell.SetSprite(data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].sprite);
                    if (data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].name == question)
                    {
                        objectInNewCell.SetDefinition(true); // ���� ��� ���������� �����, �� ��������� ���
                        correctAnswer = objectInNewCell;
                    }
                    else
                    {
                        objectInNewCell.SetDefinition(false);
                    }
                    if (stageGame == 0)
                    {
                        objectInNewCell.FirstAppearance(); // ���� ����������� � ������ ���, �� ��������� ���
                        usedQuestions.Clear(); // � �� ���������� ����� ������� �� ����������� � ������� Play Mode. �� ���� ���������� ������, �� ���� ��� ������ ��� �������� �������� ����������. ������� � ������ ������ � ������ ������� ����� ���� ���� �� �������� ������. ���� ������ ��������� ��, �� ���������� ����� ��� ������
                    }
                    countUsedAnswer++;
                }
            }
            countUsedAnswer = 0;
        }

        public IAnswer GetCorrectAnswerCell()
        {
            if (correctAnswer != null)
            {
                return correctAnswer;
            }
            else
            {
                Debug.LogError("��� ����������� ������!");
                return null;
            }
        }

        private string SelectQuestion(StageDataSO data, int[] randomAnwers, int stageGame) // ��������� �� ������� � ������� ���������� � �������� ���������� ������
        {
            if (usedQuestions.Count == 0)
            {
                int randomNum = UnityEngine.Random.Range(0, randomAnwers.Length);
                usedQuestions.Add(data.countStage[stageGame].countAnswer[randomAnwers[randomNum]].name);
                return data.countStage[stageGame].countAnswer[randomAnwers[randomNum]].name;
            }
            bool noneDublicateAnswer;
            for (int i = 0; i < randomAnwers.Length; i++)
            {
                noneDublicateAnswer = true;
                for (int j = 0; j < usedQuestions.Count; j++)
                {
                    if (data.countStage[stageGame].countAnswer[randomAnwers[i]].name == usedQuestions[j])
                    {
                        noneDublicateAnswer = false;
                    }
                }
                if (noneDublicateAnswer)
                {
                    usedQuestions.Add(data.countStage[stageGame].countAnswer[randomAnwers[i]].name);
                    return data.countStage[stageGame].countAnswer[randomAnwers[i]].name;
                }
            }
            Debug.LogError("��� ��������� ��������!");
            return null;
        }

        private int[] CreateRandomAnswers(int length, int lightCells) // ��� ��������� ��������� ������ ������� ��� ������ ����
        {
            int[] randomNumbers = new int[length - (length - lightCells)];

            int[] tempArray = new int[length];
            for (int i = 0; i < length; i++)
            {
                tempArray[i] = i;
            }
            for (int i = 0; i < length - (length - lightCells); i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, length - i);
                randomNumbers[i] = tempArray[randomIndex];
                tempArray[randomIndex] = tempArray[length - i - 1];
            }
            return randomNumbers;
        }

        private void ShuffleAnswerInArray(int[] array) // ��� ���������� ����� �������������� � �������. ����� ��� �������� ���� ����� �� ��� ��� ����� �� ����� �����
        {
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, array.Length);
                int temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }

        public GameObject GetFieldAnswer()
        {
            return fieldCenterObject;
        }
    }
}
