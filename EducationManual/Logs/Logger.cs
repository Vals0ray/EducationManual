using log4net;
using log4net.Config;

namespace EducationManual.Logs
{
    public static class Logger
    {
        public static ILog Log { get; } = LogManager.GetLogger("LOGGER");

        public static void InitLogger() => XmlConfigurator.Configure();

        public static void UserUpdateLog(string content) => Log.Info(content);
    }
}