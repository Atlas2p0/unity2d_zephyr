using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossHealthBar : MonoBehaviour {

	// Use this for initialization
	private Transform bar;
	void Start () {

	bar = transform.Find("BarSprite");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setSize(float sizeNormalized)
	{
		Debug.Log("Here");
		bar.localScale = new Vector2(bar.localScale.x * sizeNormalized, bar.localScale.y);	
		
	}
}
