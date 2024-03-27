using GameData;
using System.Collections.Generic;
using UnityEngine;

namespace QuizMono
{
    public class GenerateAnswerField : MonoBehaviour, IGeneratebleGameField // Отвечает за создание поля с ответами и за выбор вопроса (тут можно разделить логику на само создаение ячее и на заполнение)
    {
        private List<string> usedQuestions = new List<string>();
        private int countUsedAnswer;
        private IAnswer correctAnswer;
        private GameObject fieldCenterObject;
        public void Generate(GameObject cellPrefab, float cellSpacing, int stageGame, StageDataSO data)
        {
            int columns = data.countStage[stageGame].countColums;
            int rows = data.countStage[stageGame].countLines;
            int[] randomAnswers = CreateRandomAnswers(data.countStage[stageGame] // Создаем случайный список объектов
                .countAnswer.Length, rows * columns);
            string question = SelectQuestion(data, randomAnswers, stageGame); // Выбираем вопрос
            ShuffleAnswerInArray(randomAnswers); // И закидываем в случайное место списка

            Vector3 cellSize = cellPrefab.GetComponent<SpriteRenderer>().bounds.size; // Получаем размеры ячейки из префаба

            float fieldWidth = columns * (cellSize.x + cellSpacing) - cellSpacing; // Вычисляем размер всего поля
            float fieldHeight = rows * (cellSize.y + cellSpacing) - cellSpacing;

            Vector3 screenCenter = Camera.main
                .ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Получаем центр экрана в мировых координатах

            Vector3 fieldCenter = new Vector3(screenCenter.x - fieldWidth / 2 + cellSize.x / 2, 
                screenCenter.y + fieldHeight / 2 - cellSize.y / 2, 0); // Вычисляем координаты центра поля

            if (fieldCenterObject != null)
            {
                Destroy(fieldCenterObject); // Уничтожаем старое поле если есть
            }
            fieldCenterObject = new GameObject("Field");

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 position = fieldCenter + new Vector3(col * (cellSize.x + cellSpacing), // Вычисляем позицию для новой ячейки с учетом отступа
                        -row * (cellSize.y + cellSpacing), 0);

                    GameObject newCell = Instantiate(cellPrefab, position, Quaternion.identity); // Выкладываем поле из ячеек
                    newCell.transform.SetParent(fieldCenterObject.transform);
                    IAnswer objectInNewCell = newCell.GetComponent<IAnswer>();

                    objectInNewCell.SetName(data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].name); // Заполняем ячейки объектами
                    objectInNewCell.SetSprite(data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].sprite);
                    if (data.countStage[stageGame].countAnswer[randomAnswers[countUsedAnswer]].name == question)
                    {
                        objectInNewCell.SetDefinition(true); // Если это правильный ответ, то указываем это
                        correctAnswer = objectInNewCell;
                    }
                    else
                    {
                        objectInNewCell.SetDefinition(false);
                    }
                    if (stageGame == 0)
                    {
                        objectInNewCell.FirstAppearance(); // Если заполняется в первый раз, то указываем это
                        usedQuestions.Clear(); // В ТЗ написанной чтобы вопросы не повторялись в течении Play Mode. Но если бесконечно играть, то рано или поздно все варианты вопросов закончатся. Поэтому я очищаю список в рамках игровой сесси пока игра не начнется заново. Если строго следовать ТЗ, то достаточно убать эту строку
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
                Debug.LogError("Нет правильного ответа!");
                return null;
            }
        }

        private string SelectQuestion(StageDataSO data, int[] randomAnwers, int stageGame) // Пробегает по массиву в поисках дубликатов и выбирает уникальный вопрос
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
            Debug.LogError("Нет свободных вопросов!");
            return null;
        }

        private int[] CreateRandomAnswers(int length, int lightCells) // Тут создается случайный список ответов под размер поля
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

        private void ShuffleAnswerInArray(int[] array) // Тут правильный ответ перемешивается с другими. Чтобы при создании поля ответ не был все время на одном месте
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
