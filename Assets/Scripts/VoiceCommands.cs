using System;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommands : MonoBehaviour {
    private Dictionary<string, Action> keywords = new Dictionary<string,Action>();
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

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
	    keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
	    keywordRecognizer.Start();
    }
	
	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
	    Action keywordAction;
	    if (keywords.TryGetValue(args.text.ToLower(), out keywordAction))
	    {
	        keywordAction.Invoke();
	    }
	}
}
