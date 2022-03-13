using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCard : MonoBehaviour
{
    public string monsterName;
    public string description;
    public int health;
    public int maxHealth;
    public int attack;
    public Sprite sprite;

    public HealthBar healthBar;

    //public void startFight(Text storyText, Text choice1Text, Text choice2Text, GameObject ImageRight, List<SkillCard> skillCards, GameObject prefabSkillButton, RectTransform skillButonPanel)
    //{
    //    storyText.text = "You've come across a " + this.monsterName + " " + this.description;
    //    choice1Text.text = "Attack";
    //    choice2Text.text = "Run";

    //    ImageRight.GetComponent<Image>().sprite = this.sprite;



    //    //Only display on fight - hide when not used... move to fight controller
    //    for (int i = 0; i < skillCards.Count; i++)
    //    {
    //        prefabSkillButton.GetComponent<SkillCard>().description = skillCards[i].GetComponent<SkillCard>().description;
    //        prefabSkillButton.GetComponent<SkillCard>().potency = skillCards[i].GetComponent<SkillCard>().potency;
    //        prefabSkillButton.GetComponent<SkillCard>().skillname = skillCards[i].GetComponent<SkillCard>().skillname;
    //        prefabSkillButton.GetComponent<SkillCard>().skillType = skillCards[i].GetComponent<SkillCard>().skillType;
    //        prefabSkillButton.GetComponent<Image>().sprite = skillCards[i].GetComponent<SkillCard>().sprite;
    //        GameObject goButton = (GameObject)Instantiate(prefabSkillButton);

    //        if (skillButonPanel.transform.childCount > 0)
    //        {
    //            foreach (Transform child in skillButonPanel.transform)
    //            {
    //                GameObject.Destroy(child.gameObject);
    //            }
    //        }
    //        goButton.transform.SetParent(skillButonPanel, false);
    //        goButton.transform.localScale = new Vector3(1, 1, 1);

    //        Button tempButton = goButton.GetComponent<Button>();

    //        tempButton.onClick.AddListener(() => hit(currentMonsterCard, goButton.GetComponent<SkillCard>().potency));
    //    }
    //}

    public void setUp(HealthBar healthBar)
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(health);
        Debug.Log(health);
    }
    public void TakeHit(int damage)
    {
        health -= damage;
        healthBar.setHealth(health);

        if (health <= 0)
        {
            //monster killed yay
        }
    }



}
