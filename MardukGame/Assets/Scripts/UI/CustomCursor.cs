using UnityEngine;  
using System.Collections;  
  
public class CustomCursor : MonoBehaviour   
{  
    //The texture that's going to replace the current cursor  
    public Texture2D cursorTexture;  
	public Texture2D cursorTextureDown;
  
    //This variable flags whether the custom cursor is active or not  
    public bool ccEnabled = false;  
  
    void Start()  
    {  
        //Call the 'SetCustomCursor' (see below) with a delay of n seconds.   
        Invoke("SetCustomCursor",0);  

    }  
  
	void Update(){
		if (Input.GetButtonDown ("Fire1")) 
			Cursor.SetCursor(this.cursorTextureDown, Vector2.zero, CursorMode.Auto); 
		if(Input.GetButtonUp("Fire1"))
			Cursor.SetCursor(this.cursorTexture, Vector2.zero, CursorMode.Auto); 
	}

    void OnDisable()   
    {  
        //Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  
        //Set the _ccEnabled variable to false  
        this.ccEnabled = false;  
    }  

    private void SetCustomCursor()  
    {  
        //Replace the 'cursorTexture' with the cursor    
        Cursor.SetCursor(this.cursorTexture, Vector2.zero, CursorMode.Auto);  
        //Set the ccEnabled variable to true  
        this.ccEnabled = true;  
    }  
}  