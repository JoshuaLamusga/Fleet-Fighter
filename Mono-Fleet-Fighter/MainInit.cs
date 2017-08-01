using System;

namespace FleetFighter
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (MainLoop game = new MainLoop())
            {
                game.Run();
            }
        }
    }
#endif
}
