namespace QuizMono
{
    public interface IAnswerAnimation
    {
        void PlaySpawnAnimation();
        void PlayCorrectAnimation();
        void PlayIncorrectAnimation();
        void PlayParticle();
        void StandStill();
    }
}
