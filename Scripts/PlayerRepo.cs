using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepo
{
    public Color frogColour;
    public int health;
    public int maxHealth = 10;
    public int attackDmg = 1;

    public HealthBar healthBar;

    //items holding list
    //skill cards list
    public List<SkillCard> skillCards;

    public PlayerRepo(HealthBar bar)
    {
        healthBar = bar;
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(health);
        skillCards = new List<SkillCard>();
    }

    public void TakeHit(int damage)
    {
        health -= damage;
        healthBar.setHealth(health);

        if(health <= 0)
        {
            //Game Over bog time
        }
    }

    public void Heal(int heal)
    {
        health += heal;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.setHealth(health);

        if (health <= 0)
        {
            //Game Over bog time
        }
    }

}
