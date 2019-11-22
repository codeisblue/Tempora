using System;
using Tempora.Engine;

namespace TestGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameManager(new TestGame()))
                game.Run();
        }
    }
}
