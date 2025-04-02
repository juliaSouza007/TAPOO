using Avalonia;
using System;

namespace AvaloniaUI
{
    class Program
    {
        [STAThread] //define que o aplicativo usa um unico thread para a interface grafica
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                             .UsePlatformDetect()
                             .LogToTrace();
        }
    }
}
