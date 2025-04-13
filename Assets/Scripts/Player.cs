using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class Player : ScriptableObject
{

    const int maxHealth = 20;
    const int startHandSize = 3;
    const int maxHandSize = 7;

    Stats myStats;
    List<string> cuedEffects = new List<string>();
    List<Card> hand = new List<Card>();
    List<Card> deck = new List<Card>();
    Player enemy;

    public Stats GetStats () {
        return myStats;
    }

    public List<Card> GetHand () {
        return hand;
    }

    public List<Card> GetDeck () {
        return deck;
    }

    public void SetEnemy(Player target) {
        enemy = target;
    }

    public void GainHealth(int amount) {
        myStats.health += amount;
        if (myStats.health > maxHealth) {
            myStats.health = maxHealth;
        }
    }

    public void GainShield(int amount) {
        myStats.shield += amount;
    }

    public void GainMana(int amount) {
        myStats.mana += amount;
    }

    public void GainManaGain(int amount) {
        myStats.manaGain += amount;
    }

    public void LoseHealth(int amount) { //Skips shield
        myStats.health -= amount;

        if (myStats.health <= 0) {
            Die();
        }
    }

    void UseMana(int amount) {
        myStats.mana -= amount;
    }

    public void GetHit(int damage) { //Can consume shield
        damage -= myStats.shield;
        if (damage < 0) {
            myStats.shield = -damage;
        } else {
            myStats.shield = 0;
        }

        if (damage <= 0) {
            return;
        }

        LoseHealth(damage);
    }

    void Die() {
        GameManager.reference.GameEnd(myStats.name);
    }

    public void TurnStart() {
        myStats.mana += myStats.manaGain;
        Draw();
        //Clear shield per turn
        myStats.shield = 0;
        PlayCuedEffects();
    }

    public void CueEffect(string effectName) {
        cuedEffects.Add(effectName);
    }

    void PlayCuedEffects() {
        while (cuedEffects.Count > 0) {
            CuedEffects.UseCue(this,enemy,cuedEffects[0]);
            cuedEffects.RemoveAt(0);
        }
    }

    public void Draw() {

        if (deck.Count <= 0) {
            Die();
            return;
        }

        if (hand.Count < 7) {
            hand.Add(deck[0]);
        }
        deck.RemoveAt(0);
    }

    public bool UseCard(int cardIndex) {

        Card curCard = hand[cardIndex];

        if (myStats.mana >= curCard.cost) {
            hand.RemoveAt(cardIndex);
            UseMana(curCard.cost);        
            curCard.UseAbility(this,enemy);
            return true;
        } else {
            return false;
        }
    }

    public void init(string playerName, string deckName) {
        myStats = new Stats(playerName);
        InitDeck(deckName);
    }

    public void InitDeck(string deckName) {

        TextAsset jsonTextFile = Resources.Load<TextAsset>(deckName);

        DeckStringList deckList = JsonUtility.FromJson<DeckStringList>(jsonTextFile.text);

        for (int i = 0; i < deckList.deck.Length; i++) {
            deck.Add(ScriptableObject.CreateInstance<Card>());
            deck[i].init(deckList.deck[i]);
        }

        DeckShuffle();

        for (int i = 0; i < startHandSize; i++) {
            Draw();
        }
    }

    public void DeckShuffle()
    {
        List<Card> b = new List<Card>();
        var randomness = new System.Random();

        for (int i = deck.Count - 1; i >= 0; --i)
        {
            int rand = randomness.Next(deck.Count);
            b.Add(deck[rand]);
            deck.RemoveAt(rand);
        }

        deck = b;
    }

}

class DeckStringList {
    public string[] deck;
}

public struct Stats {

    const int startHealth = 20;
    const int startMana = 2;
    const int startManaGain = 2;

    public string name;
    public int health;
    public int shield;
    public int mana;
    public int manaGain;

    public Stats(string name) {
        this.name = name;
        health = startHealth;
        shield = 0;
        mana = startMana;
        manaGain = startManaGain;
    }
}