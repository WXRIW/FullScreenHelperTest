using Lyricify_for_Spotify.Helpers.Device;

namespace FullScreenHelperTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserNotificationState? prevState = null;
            while (true)
            {
                var state = FullscreenHelper.State();
                if (prevState != state)
                {
                    prevState = state;
                    Console.WriteLine(DateTime.Now.ToString("O") + " " + Enum.GetName(typeof(UserNotificationState), state));
                }
            }
        }
    }
}
