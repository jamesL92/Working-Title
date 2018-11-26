using UnityEngine;

public class PrefabManager: MonoSingleton<PrefabManager> {
    //Prefab References for grid game
    public GameObject walkableTilePrefab;
    public GameObject unwalkableTilePrefab;
    public GameObject castlePrefab;
    public GameObject unitPrefab;

    //Prefab References for the card game.
    public Card cardPrefab;
    public Sprite cardFrontSprite;
    public Sprite cardBackSprite;
}