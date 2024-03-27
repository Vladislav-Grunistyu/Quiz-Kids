using UnityEngine;

namespace QuizMono
{
    public class Cell : MonoBehaviour, IAnswer // Сама клетка с объектом внутри, обрабатывает нажатия
    {
        private new string name;
        private GameObject sprite;
        private bool itsCorrectAnswer;
        private IAnswerAnimation animations;
        private bool hasbeenPressed = false;

        public delegate void CellEventHandler(string nameAnswer);

        public event CellEventHandler CellEvent;

        private void Awake()
        {
            animations = GetComponent<IAnswerAnimation>();
        }
        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetSprite(GameObject sprite)
        {
            this.sprite = sprite;
        }

        public void SetDefinition(bool definition)
        {
            itsCorrectAnswer = definition;
            Transform content = Instantiate(this.sprite, transform.position, Quaternion.identity).transform;
            content.SetParent(transform);
        }

        public string GetName()
        {
            if (name != null)
            {
                return name;
            }
            else
            {
                Debug.LogError("Нет имени!");
                return null;
            }
        }

        public void FirstAppearance()
        {
            animations.PlaySpawnAnimation();
        }

        private void OnMouseDown()
        {
            if (itsCorrectAnswer)
            {   
                if (!hasbeenPressed)
                {
                    animations.PlayCorrectAnimation();
                    CellEvent?.Invoke(name);
                    hasbeenPressed = true;
                }
            }
            else
            {
                if (!hasbeenPressed)
                {
                    animations.PlayIncorrectAnimation();
                }
            }
            
        }
    }
}
