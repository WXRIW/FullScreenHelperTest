﻿using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;

namespace Lyricify_for_Spotify.Helpers.Device
{
    public static class FullscreenHelper
    {
        public static UserNotificationState State()
        {
            _ = SHQueryUserNotificationState(out UserNotificationState state);

            return state;
        }

        public static bool GameMode
        {
            get
            {
                if (SHQueryUserNotificationState(out var state) == 0)
                {
                    return state == UserNotificationState.RunningDirect3dFullScreen
                        || IsQunsAppState(state);
                }
                return false;
            }
        }

        public static bool FullScreen
        {
            get
            {
                if (SHQueryUserNotificationState(out var state) == 0)
                {
                    //System.Diagnostics.Debug.WriteLine(state);
                    return /*state == UserNotificationState.RunningDirect3dFullScreen
                        || */state == UserNotificationState.Busy
                        || state == UserNotificationState.PresentationMode
                        || IsQunsAppState(state);
                }
                return false;
            }
        }

        private static bool IsQunsAppState(UserNotificationState state)
        {
            if (state == UserNotificationState.QUNS_APP)
            {
                var hwnd = User32.GetForegroundWindow();
                var sb = new StringBuilder(256);
                _ = User32.GetClassName(hwnd, sb, sb.Capacity);
                var className = sb.ToString();

                if (className != "Windows.UI.Core.CoreWindow") return true;
            }
            return false;
        }


        [DllImport("shell32.dll")]
        private static extern int SHQueryUserNotificationState(out UserNotificationState userNotificationState);

    }

    public enum UserNotificationState
    {
        /// <summary>
        /// A screen saver is displayed, the machine is locked,
        /// or a nonactive Fast User Switching session is in progress.
        /// </summary>
        NotPresent = 1,

        /// <summary>
        /// A full-screen application is running or Presentation Settings are applied.
        /// Presentation Settings allow a user to put their machine into a state fit
        /// for an uninterrupted presentation, such as a set of PowerPoint slides, with a single click.
        /// </summary>
        Busy = 2,

        /// <summary>
        /// A full-screen (exclusive mode) Direct3D application is running.
        /// </summary>
        RunningDirect3dFullScreen = 3,

        /// <summary>
        /// The user has activated Windows presentation settings to block notifications and pop-up messages.
        /// </summary>
        PresentationMode = 4,

        /// <summary>
        /// None of the other states are found, notifications can be freely sent.
        /// </summary>
        AcceptsNotifications = 5,

        /// <summary>
        /// Introduced in Windows 7. The current user is in "quiet time", which is the first hour after
        /// a new user logs into his or her account for the first time. During this time, most notifications
        /// should not be sent or shown. This lets a user become accustomed to a new computer system
        /// without those distractions.
        /// Quiet time also occurs for each user after an operating system upgrade or clean installation.
        /// </summary>
        QuietTime = 6,

        /// <summary>
        /// Introduced in Windows 8. A Windows Store app is running.
        /// </summary>
        QUNS_APP = 7
    }
}
