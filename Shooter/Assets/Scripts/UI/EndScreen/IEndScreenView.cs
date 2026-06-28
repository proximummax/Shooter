using System;

namespace Shooter.UI
{
    public interface IEndScreenView
    {
        event Action RestartClicked;

        void SetVisible(bool visible);
        void SetTitle(string title);
    }
}
