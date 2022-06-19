using System;
using UniRx;

namespace Common.UI.Windows
{
    public class GameOverWindow : MainMenuWindow
    {
        public IObservable<Unit> RestartButtonClicked => CloseButtonClicked;
    }
}