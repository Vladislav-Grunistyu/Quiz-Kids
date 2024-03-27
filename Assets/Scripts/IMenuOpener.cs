namespace QuizMono
{
    public interface IMenuOpener
    {
        void OpenMenu();
        void StartLoadScreen();
        RestartMenuOpener GetObjectLoaded();
    }
}