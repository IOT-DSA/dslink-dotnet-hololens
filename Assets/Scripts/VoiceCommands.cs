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
                var switchObject = Instantiate(Resources.Load("Light Switch") as GameObject, Camera.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                if (switchObject != null)
                {
                    switchObject.transform.parent = SpaceCollection.transform;
                }
            });

            keywords.Add("move", () =>
            {
                //GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer);
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
