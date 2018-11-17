using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Working_Title.Assets.Scripts;

[RequireComponent(typeof(Text))]
public class PlayerGoldText : MonoBehaviour {

	private Text playerGoldText;
	// Use this for initialization
	void Start () {
		playerGoldText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		playerGoldText.text = GameManager.instance.currentPlayer.gold.ToString();
	}
}
