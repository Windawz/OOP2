using System.Reflection;

using WinFormsTasks.Common;

namespace WinFormsTasks.Task8;
internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var selectorForm = new SelectorForm();
        Glitch.Attach(selectorForm, typeof(TestListForm));
        Application.Run(selectorForm);
    }
}