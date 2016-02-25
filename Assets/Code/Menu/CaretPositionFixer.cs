using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Just got it from http://answers.unity3d.com/questions/884762/ui-inputfield-caret-position-wrong-when-midline-al.html
/// to fix caret positions on input fields.
/// </summary>
public class CaretPositionFixer : MonoBehaviour
{
	//Controls access to the subset of 'IF' statements
	private bool control1 = false;
	
	//Controls access to  the first step
	private bool control2 = false;
	
	//Controls access to  the second step
	private bool control3 = false;
	
	
	/// <summary>
	/// Font setting where the caret aligns naturally.
	/// Default: 14 (Arial)
	/// </summary>
	public int workingFontSize = 14;
	
	/// <summary>
	/// The ratio used to move the caret relative to the font size.
	/// Default: 0.84 (Arial)
	/// </summary>
	public float ratioToFont = 0.84f;
	
	//Update Method
	void Update(){
		
		if (control1 == false) {
			
			if (control2 == false) {
				
				InputField temp = gameObject.GetComponent<InputField> ();
				
				//Causes the "InputField" to create the "Caret Object'
				temp.text = "1";
				
				temp.text = "";
				
				control2 = true;
				
			} else if (control3 == false) {
				
				ResetCaret ();
				
				control3 = true;
				control1 = true;
				
			}
			
		}
		
	}
	
	/// <summary>
	/// Resets the caret.
	/// Accesses the "Local Position" of the caret Object
	/// and changes the "Y Value" based on the formula in
	/// the "ScaleCaret()" method
	/// </summary>
	public void ResetCaret(){
		
		string name = gameObject.name + " Input Caret";
		
		Transform temp1 = gameObject.transform;
		
		GameObject temp2 = temp1.Find (name).gameObject;
		
		Vector3 temp3 = temp2.transform.localPosition;
		
		temp3.y = temp3.y + ScaleCaret();
		
		temp2.transform.localPosition = temp3;
		
		
	}
	
	/// <summary>
	/// Uses the "workingFontSize" and "ratioToFont to
	/// generate a float value to correct the loaction
	/// of the "Input Field" caret.
	/// </summary>
	/// <returns>New float to add to the Y pos of the caret.</returns>
	private float ScaleCaret(){
		
		InputField temp1 = gameObject.GetComponent<InputField> ();
		
		Text temp2 = temp1.textComponent;
		
		int temp3 = temp2.fontSize;
		
		float temp4 = (float)(temp3 - workingFontSize) * (ratioToFont);
		
		return temp4;
		
	}
}