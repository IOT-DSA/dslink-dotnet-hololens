using System;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Debug = System.Diagnostics.Debug;

namespace DSHoloLens
{
    public class VoiceCommands : MonoBehaviour
    {
        public GameObject SpaceCollection;
        public GameObject Camera;

        private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
        private KeywordRecognizer keywordRecognizer;
        public static GameObject HeldObject;

        private void Start()
        {
            keywords.Add("remove", () =>
            {
                var focusObject = GazeManager.Instance.FocusedObject;
                if (focusObject != null)
                {
                    focusObject.SendMessage("OnVoiceRemove");
                }
            });

            keywords.Add("add switch", () =>
            {
                var pos = Camera.transform.position + Camera.transform.forward * 1f;
                var switchObject = Instantiate(Resources.Load("Light Switch") as GameObject, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
                if (switchObject != null)
                {
                    switchObject.transform.parent = SpaceCollection.transform;
                }
            });

            keywords.Add("add light", () =>
            {
                var pos = Camera.transform.position + Camera.transform.forward*1f;
                var switchObject =
                    Instantiate(Resources.Load("Light") as GameObject, pos, new Quaternion(180, 0, 0, 0)) as
                        GameObject;
                if (switchObject != null)
                {
                    switchObject.transform.parent = SpaceCollection.transform;
                }
            });

            keywords.Add("move", () =>
            {
                var focusObject = GazeManager.Instance.FocusedObject;
                if (focusObject != null)
                {
                    HeldObject = focusObject;
                    focusObject.SendMessage("OnVoiceMove");
                }
            });

            keywords.Add("place", () =>
            {
                if (HeldObject != null)
                {
                    HeldObject.SendMessage("OnVoicePlace");
                }
            });

            keywords.Add("activate", () =>
            {
                var focusObject = GazeManager.Instance.FocusedObject;
                if (focusObject != null)
                {
                    focusObject.SendMessage("OnVoiceActivate");
                }
            });

            keywords.Add("deactivate", () =>
            {
                var focusObject = GazeManager.Instance.FocusedObject;
                if (focusObject != null)
                {
                    focusObject.SendMessage("OnVoiceDeactivate");
                }
            });

            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
            keywordRecognizer.Start();
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.WriteLine("Voice command detected: " + args.text);
            Action keywordAction;
            if (keywords.TryGetValue(args.text.ToLower(), out keywordAction))
            {
                keywordAction.Invoke();
            }
        }
    }
}
