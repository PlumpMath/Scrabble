using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {

	//Variable for GUI
	private string strEdit = "Potatoe";
	private string strHolder = "";
	private string msg;
	private bool langChange=false;
	private bool en=true;
	private bool fr=false;	

	void OnGUI () {
		if(langChange){//radio button has changed
		
			if(en){wordCheck.setLanguage("en");}//load english dictionary  <-------  wordCheck.setLanguage('text file in resources folder') resets dictionary to new language.    --------//
			else if(fr){wordCheck.setLanguage("fr");}//load french dictionary
			/* else if(...){checkScript.setLanguage("...");}//load other dictionary */
			
			langChange=false;
			strHolder="";
		}
		if(GUI.Toggle(new Rect (20,90,150,20), en, "English")){if(!en){en = setLang();}}
		if(GUI.Toggle(new Rect (20,110,150,20), fr, "French")){if(!fr){fr = setLang();}}
	
	    strEdit = GUI.TextField(new Rect (20, 20, 200, 20), strEdit, 25);
	    if(strHolder != strEdit){//The word has changed.
	    
	    	if(   wordCheck.isWord(strEdit)   ){ //<-------  wordCheck.isWord('word to check here') checks to see if a word is in the current dictionary.    --------//
	    
	    		if(en){msg="'"+strEdit+"' is a word.";}
	    		else if(fr){msg="'"+strEdit+"' est un mot.";}
	    		/* else if(...){msg="'"+strEdit+"' is a word.";} */
	    	}else{
	    		if(en){msg="'"+strEdit+"' is NOT a word.";}
	    		else if(fr){msg="'"+strEdit+"' n'est pas un mot.";}
	    		/* else if(...){msg="'"+strEdit+"' is NOT a word.";} */
	    	}
	    }
	    GUI.Label(new Rect (20, 50, 500, 20), msg);
	    strHolder = strEdit;
	}

	private bool setLang(){//radio button control
  		en = fr /* =... */ = false;//set all languages to false here
  		langChange=true;
  		return true;//set only currently clicked toggle to true
	}

}
