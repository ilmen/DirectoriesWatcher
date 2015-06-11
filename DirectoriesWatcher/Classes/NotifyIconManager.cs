using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectoriesWatcher.Classes
{
    public class NotifyIconManager : DirectoriesWatcher.Classes.INotifyIconManager
    {
        FakeContainer iconContainer = new FakeContainer();
        NotifyIcon icon;

        public NotifyIconManager()
        {
            icon = new NotifyIcon(iconContainer)    // NotifyIcon не отображается, если нет контейнера для иконки - лечим это
            {
                Icon = new Icon(System.IO.Directory.GetCurrentDirectory() + "/Red.ico"),
                Text = "DirectoriesWatcher",
                BalloonTipText = " ",
                BalloonTipTitle = "DirectoriesWatcher",
                Visible = true,
            };
            icon.Click += (s, e) => ShowBallonToolTip();
        }

        public void ShowBallonToolTip(string text = " ")
        {
            icon.BalloonTipText = text;
            icon.ShowBalloonTip(0);
        }
    }
}
