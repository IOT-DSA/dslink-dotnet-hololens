#if WINDOWS_UWP
using System.Collections.Generic;
using DSLink.Util.Logger;
using DSLink;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using HoloToolkit.Unity;
using UnityEngine;
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
            Responder.AddNodeClass("createSwitchAction", node =>
            {
                node.DisplayName = "Create Switch";
                node.SetAction(new ActionHandler(Permission.Write, async request =>
                {
                    MainThreadManager.Instance().Enqueue(() =>
                    {
                        if (Camera.main != null)
                        {
                            var pos = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                            Object.Instantiate(Resources.Load("Light Switch") as GameObject, pos, new Quaternion(0, 0, 0, 0));
                        }
                    });

                    await request.Close();
                }));
            });

            Responder.AddNodeClass("createLightAction", node =>
            {
                node.DisplayName = "Create Light";
                node.SetAction(new ActionHandler(Permission.Write, async request =>
                {
                    MainThreadManager.Instance().Enqueue(() =>
                    {
                        if (Camera.main != null)
                        {
                            var pos = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                            Object.Instantiate(Resources.Load("Light") as GameObject, pos, new Quaternion(0, 0, 0, 0));
                        }
                    });

                    await request.Close();
                }));
            });

            Responder.AddNodeClass("createSquareLightAction", node =>
            {
                node.DisplayName = "Create Square Light";
                node.SetAction(new ActionHandler(Permission.Write, async request =>
                {
                    MainThreadManager.Instance().Enqueue(() =>
                    {
                        if (Camera.main != null)
                        {
                            var pos = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                            Object.Instantiate(Resources.Load("Light Square") as GameObject, pos, new Quaternion(0, 0, 0, 0));
                        }
                    });

                    await request.Close();
                }));
            });
        }

        public static void Start()
        {
            DSLink.UWP.UWPPlatform.Initialize();
            BaseLogger.Logger = typeof(DiagLogger);
            Instance = new HoloLensDSLink();
            Instance.Connect().Wait();
        }

        public override void InitializeDefaultNodes()
        {
            Responder.SuperRoot.CreateChild("createSwitch", "createSwitchAction").BuildNode();
            Responder.SuperRoot.CreateChild("createLight", "createLightAction").BuildNode();
            Responder.SuperRoot.CreateChild("createSquareLight", "createSquareLightAction").BuildNode();
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
