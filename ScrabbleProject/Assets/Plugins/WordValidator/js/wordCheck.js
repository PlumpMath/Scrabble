#pragma strict

import System.Collections.Generic;

public var defaultLanguage:String="en";
static private var wordList:Dictionary.<String, boolean>;

function Start () {
	setLanguage(defaultLanguage);	
}

static function isWord(word:String):boolean{
	return wordList.ContainsKey(word.ToLower());
}

static function setLanguage(res:String){
	var words:String[] = (Resources.Load(res) as TextAsset).text.Split("\n"[0]);
	wordList = new Dictionary.<String, boolean>();
	for(word in words)
		wordList.Add(word.Trim(),true);
}