using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wordCheck : MonoBehaviour {

	public string defaultLanguage="en";
	private static Dictionary<string, bool> wordList;
	[SerializeField] public List<string> m_words;

	public static wordCheck Instance { get; private set; }

	private void Awake ()
	{
		wordCheck.Instance = this;
	}

	void Start () {
		setLanguage(defaultLanguage);
	}
	
	public static bool isWord(string word){
		return wordList.ContainsKey(word.ToLower());
	}
	
	public static void setLanguage(string res){
		string[] words = (Resources.Load(res) as TextAsset).text.Split("\n"[0]);
		wordList = new Dictionary<string,bool>();
		foreach (string word in words)
		{
			wordList.Add(word.Trim(),true);
			//m_words.Add(word.Trim());

			wordCheck.Instance.m_words.Add(word.Trim());
		}
	}
}