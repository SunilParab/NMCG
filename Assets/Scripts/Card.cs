using System;
using UnityEngine;

[SerializeField]
public class Card : ScriptableObject
{
    public string type;
    public int cost;

    public void init(string cardName) {
        type = cardName;

        cost = GetCost(cardName);
    }

    public void UseAbility(Player self, Player other) {

        object[] input = {self,other};
        typeof(CardAbilities).GetMethod(type).Invoke(null,input);

    }

    int GetCost(string cardName) {
        switch (cardName) {
            case "Scratch":
                return 2;
            case "Heal":
                return 2;
            case "MissileStrike":
                return 3;
            case "Block":
                return 3;
            case "Focus":
                return 3;
            case "Draw":
                return 3;
            case "CardRain":
                return 3;
            case "ManaShot":
                return 2;
            case "Gun":
                return 4;
            case "Invest":
                return 4;
            default:
                Debug.Log("Card Missing");
                return 0;
        }
    }

}

class CardAbilities
{
    public static void Scratch(Player ally, Player enemy) {
        enemy.GetHit(3);
    }
    public static void Heal(Player ally, Player enemy) {
        ally.GainHealth(4);
    }
    public static void MissileStrike(Player ally, Player enemy) {
        ally.CueEffect("MissileStrike");
    }
    public static void Block(Player ally, Player enemy) {
        ally.GainShield(6);
    }
    public static void Focus(Player ally, Player enemy) {
        ally.GainManaGain(1);
    }
    public static void Draw(Player ally, Player enemy) {
        ally.Draw();
        ally.Draw();
    }
    public static void CardRain(Player ally, Player enemy) {
        for (int i = 0; i < 3; i++) {
            ally.Draw();
            enemy.Draw();
        }
    }
    public static void ManaShot(Player ally, Player enemy) {
        enemy.GetHit(5);
        enemy.GainMana(1);
    }
    public static void Gun(Player ally, Player enemy) {
        enemy.GetHit(5);
    }
    public static void Invest(Player ally, Player enemy) {
        ally.CueEffect("Invest");
    }
}

class CuedEffects
{

    public static void UseCue(Player self, Player other, string type) {

        object[] input = {self,other};
        typeof(CuedEffects).GetMethod(type).Invoke(null,input);

    }

    public static void MissileStrike(Player ally, Player enemy) {
        enemy.GetHit(8);
    }
    public static void Invest(Player ally, Player enemy) {
        ally.GainHealth(5);
        ally.GainMana(5);
    }
}