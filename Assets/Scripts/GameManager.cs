using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player1;
    Player player2;
    int turnCount;
    bool player1Turn;

    [SerializeField]
    TextMeshProUGUI player1Stats;
    [SerializeField]
    TextMeshProUGUI player1Mana;
    [SerializeField]
    TextMeshProUGUI player1Deck;
    [SerializeField]
    TextMeshProUGUI player1Hand;

    [SerializeField]
    TextMeshProUGUI player2Stats;
    [SerializeField]
    TextMeshProUGUI player2Mana;
    [SerializeField]
    TextMeshProUGUI player2Deck;
    [SerializeField]
    TextMeshProUGUI player2Hand;

    [SerializeField]
    TextMeshProUGUI turnDisplay;
    [SerializeField]
    TextMeshProUGUI winText;

    public static GameManager reference;

    bool gameEnded = false;

    void Awake()
    {
        reference = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = ScriptableObject.CreateInstance<Player>();
        player2 = ScriptableObject.CreateInstance<Player>();

        player1.init("Player1","deck1");
        player2.init("Player2","deck1");

        player1.SetEnemy(player2);
        player2.SetEnemy(player1);

        turnCount = 1;
        player1Turn = true;

        player1.TurnStart();

        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        if (gameEnded) {
            return;
        }

        List<Card> hand1 = player1.GetHand();

        List<Card> hand2 = player2.GetHand();

        if (player1Turn) {
            for (int i = 0; i < hand1.Count; i++) {
                if (Input.GetKeyDown( "" + (i+1) )) {
                    player1.UseCard(i);
                    break;
                }
            }
        } else {
            for (int i = 0; i < hand2.Count; i++) {
                if (Input.GetKeyDown( "" + (i+1) )) {
                    player2.UseCard(i);
                    break;
                }
            }
        }

        if (Input.GetKeyDown("e")) {
            FinishAction();
        }

        Stats stats1 = player1.GetStats();
        List<Card> deck1 = player1.GetDeck();

        Stats stats2 = player2.GetStats();
        List<Card> deck2 = player2.GetDeck();

        player1Stats.text = stats1.name+"\n"+"H: "+stats1.health+"\n"+"S: "+stats1.shield;
        player1Mana.text = "\n"+"M: "+stats1.mana+"\n"+"G: "+stats1.manaGain;
        player1Deck.text = ""+deck1.Count;

        string handText = "";
        for (int i = 0; i < hand1.Count; i++) {
            handText += (i+1)+": "+hand1[i].type+"\n";
        }
        if (handText.Length > 0) {
            handText = handText.Remove(handText.Length - 1);
        }
        player1Hand.text = handText;


        player2Stats.text = stats2.name+"\n"+"H: "+stats2.health+"\n"+"S: "+stats2.shield;
        player2Mana.text = "\n"+"M: "+stats2.mana+"\n"+"G: "+stats2.manaGain;
        player2Deck.text = ""+deck2.Count;

        handText = "";
        for (int i = 0; i < hand2.Count; i++) {
            handText += (i+1)+": "+hand2[i].type+"\n";
        }
        if (handText.Length > 0) {
            handText = handText.Remove(handText.Length - 1);
        }
        player2Hand.text = handText;


        string turnHalf;

        if (player1Turn) {
            turnHalf = "Player 1 Turn";
        } else {
            turnHalf = "Player 2 Turn";
        }

        turnDisplay.text = "Turn: "+turnCount+"\n"+turnHalf;
    }

    void FinishAction() {
        if (player1Turn) {
            player1Turn = false;

            player2.TurnStart();
        } else {
            player1Turn = true;
            turnCount++;
            player1.TurnStart();
        }
    }

    public void GameEnd(string name) {
        winText.text = name+" Wins!";
        gameEnded = true;
    }
}
