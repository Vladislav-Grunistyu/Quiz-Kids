using UnityEngine;
using static QuizMono.Cell;

namespace QuizMono
{
    public interface IAnswer
    {
        void SetName(string name);
        void SetSprite(GameObject sprite);
        void SetDefinition(bool definition);
        string GetName();
        public event CellEventHandler CellEvent;
        void FirstAppearance();
    }
}
