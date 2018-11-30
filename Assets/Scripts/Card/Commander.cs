using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour, IHealthBarHandler {

	private int currentHealth;
	private int maxHealth;

	//Don't like adding this here. :(
	//Ideally I should be able to hook this up in the
	//Unity editor - annoyingly though the editor
	//doesn't seem to want to serialize interfaces :(
	//TODO: need to find a cleaner way to do this.
	[SerializeField] private HealthBar healthBar;

	void Start() {
		//TODO: see the todo around the healthBar field.
		if(healthBar) {
			healthBar.handler = this;
		}
	}
	public void LoadCommander(int health) {
		this.maxHealth = health;
		this.currentHealth = health;
	}

	public void Damage(int damage) {
		this.currentHealth -= damage;
	}

  public int GetMaxHealth()
  {
    return maxHealth;
  }

  public int GetCurrentHealth()
  {
    return currentHealth;
  }
}
