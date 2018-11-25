using UnityEngine;
using PlayerClasses;
using System.Collections;
using System.Collections.Generic;

public class CardGameManager: MonoSingleton<CardGameManager> {
  [System.Serializable]
  public struct Side {
    public Player Player;
    public List<Zone> CardSlots;
    public Zone Deck;
    public Zone Hand;
    public Zone Discard;
  }

  public int DeckSize;
  [SerializeField] public Side AttackingSide;
  [SerializeField] public Side DefendingSide;
  private Queue<Player> playerQueue = new Queue<Player>();

  public Transform PlayerAreaTransform;
  public float BoardRotationTime;
  private bool Rotating;

  void Start() {
    InitGame();
  }

  public void InitGame() {
    // Set up queue
    playerQueue.Enqueue(AttackingSide.Player);
    playerQueue.Enqueue(DefendingSide.Player);

    SetupDecks();
  }

  void SetupDecks() {
    for (int i = 0; i < DeckSize; i++)
    {
        Card card;
        card = Instantiate(PrefabManager.instance.cardPrefab, AttackingSide.Deck.transform) as Card;
        card.name = PrefabManager.instance.cardPrefab.name;

        card = Instantiate(PrefabManager.instance.cardPrefab, DefendingSide.Deck.transform) as Card;
        card.name = PrefabManager.instance.cardPrefab.name;
    }
  }

  void Update() {
    if(Input.GetKeyDown(KeyCode.Space)) {
      EndTurn();
    }
  }

  void EndTurn() {
    Player currentPlayer = playerQueue.Dequeue();
    playerQueue.Enqueue(currentPlayer);

    StartCoroutine(RotateBoard());

  }

  public IEnumerator RotateBoard() {
    if(Rotating) yield break;

    float rotationStartTime = Time.time;
    Rotating = true;
    float targetZRotation = PlayerAreaTransform.localEulerAngles.z + 180f;
    Debug.Log(string.Format("time: {0}, targetTime: {1}", Time.time, rotationStartTime + BoardRotationTime));
    while(Time.time <= rotationStartTime + BoardRotationTime) {
      PlayerAreaTransform.localRotation *= Quaternion.Euler(0,0,180/BoardRotationTime * Time.deltaTime);
      yield return null;
    }
    PlayerAreaTransform.localRotation = Quaternion.Euler(0,0,targetZRotation);
  }
}