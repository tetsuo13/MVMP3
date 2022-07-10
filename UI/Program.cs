using Lemon.NLog.WinForms;
using NLog;
using NLog.Config;

namespace UI;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var form1 = new Main();
        Initialize(form1);
        Application.Run(form1);
    }

    public static void Initialize(Main input)
    {
        Application.EnableVisualStyles();
        //Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        var config = new LoggingConfiguration();
        config.AddRule(LogLevel.Trace, LogLevel.Fatal,
            new TextBoxTarget(input.LoggerBox) { Layout = "[${date}] [${level}] ${message}" });
        //config.AddRule(LogLevel.Trace, LogLevel.Fatal, new ToolStripStatusLabelTarget(form.LogToolStripStatusLabel) { Layout = "${message}" });
        LogManager.Configuration = config;
    }
}