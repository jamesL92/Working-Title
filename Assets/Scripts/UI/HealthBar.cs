using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {

	public Image fillImage;
	public Text healthText;
	public IHealthBarHandler handler;
	void LateUpdate() {
		if(handler != null) {
			if(fillImage) {
				fillImage.fillAmount = 1f * handler.GetCurrentHealth() / handler.GetMaxHealth();
			}
			if(healthText) {
				healthText.text = string.Format("{0} / {1}", handler.GetCurrentHealth(), handler.GetMaxHealth());
			}
		}
	}
}

public interface IHealthBarHandler {
	int GetMaxHealth();
	int GetCurrentHealth();
}