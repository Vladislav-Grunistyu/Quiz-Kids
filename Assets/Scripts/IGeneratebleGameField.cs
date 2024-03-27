using GameData;
using UnityEngine;

namespace QuizMono
{
    public interface IGeneratebleGameField
    {
        public void Generate(GameObject cellPrefab, float cellSpacing, int stageGame, StageDataSO data);
        public IAnswer GetCorrectAnswerCell();
        public GameObject GetFieldAnswer();
    }
}
