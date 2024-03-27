using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "FieldData", menuName = "Game/Field Data")]
    public class StageDataSO : ScriptableObject // SO � ������� ��������� ���������� � ��������� �������
    {
        public AnswerCountInLevel[] countStage;
    }

    [System.Serializable]
    public struct CellWithAnswerData
    {
        public string name;
        public GameObject sprite;
    }

    [System.Serializable]
    public struct AnswerCountInLevel
    {
        public int countLines;
        public int countColums;
        public CellWithAnswerData[] countAnswer;
    }
}
