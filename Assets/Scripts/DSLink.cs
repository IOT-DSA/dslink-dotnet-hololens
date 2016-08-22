#if WINDOWS_UWP
using System.Collections.Generic;
using DSLink.Util.Logger;
using DSLink;
using DebugLog = System.Diagnostics.Debug;

namespace DSHoloLens
{
    public class HoloLensDSLink : DSLinkContainer
    {
        public static HoloLensDSLink Instance;

        public HoloLensDSLink() : base(new Configuration(new List<string>(), "HoloLens", true, true,
                    brokerUrl: "http://octocat.local:8080/conn", communicationFormat: "json"
#if DEBUG
            , logLevel: LogLevel.Info
#endif
            ))
        {
        }

        public static void Start()
        {
            DSLink.UWP.UWPPlatform.Initialize();
            BaseLogger.Logger = typeof(DiagLogger);
            Instance = new HoloLensDSLink();
            Instance.Connect().Wait();
        }
    }

    /// <summary>
    /// Default logger for platforms without proper logging.
    /// </summary>
    public class DiagLogger : BaseLogger
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DSLink.Util.Logger.DiagnosticsLogger"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="toPrint">To print</param>
        public DiagLogger(string name, LogLevel toPrint) : base(name, toPrint)
        {
        }

        /// <summary>
        /// Prints a message to the console.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="message">Message</param>
        public override void Print(LogLevel logLevel, string message)
        {
            if (logLevel.DoesPrint(ToPrint))
            {
                DebugLog.WriteLine(Format(logLevel, message));
            }
        }
    }
}
#endif
