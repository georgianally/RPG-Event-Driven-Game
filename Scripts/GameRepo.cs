using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRepo
{
    //Events
    public static List<GameObject> movementListObjects = new List<GameObject>();
    public static List<GameObject> fightListObjects = new List<GameObject>();
    public static List<GameObject> mysteryListObjects = new List<GameObject>();

    //SubEventData
    public static List<GameObject> choiceListObjects = new List<GameObject>();
    public static List<GameObject> monstersListObjects = new List<GameObject>();
    public static List<GameObject> skillListObjects = new List<GameObject>();

    // Start is called before the first frame update

    //When changing levels pass in level and get right pack of cards
    public GameRepo()
    {
        LoadPrefabEvents("Movement");
        LoadPrefabEvents("Choices");

        LoadPrefabEvents("Fight");
        LoadPrefabEvents("Monsters");
        LoadPrefabEvents("Skills");

        LoadPrefabEvents("Mystery");
    }

    void LoadPrefabEvents(string eventType)
    {
        Object[] tempList = Resources.LoadAll<GameObject>("Prefabs/Cards/" + eventType);
        List<GameObject> tempListObjects = new List<GameObject>();

        foreach (GameObject subListObject in tempList)
        {
            GameObject lo = (GameObject)subListObject;

            tempListObjects.Add(lo);
        }

        switch (eventType)
        {
            case "Movement":
                movementListObjects = tempListObjects;
                break;
            case "Choices":
                choiceListObjects = tempListObjects;
                break;
            case "Fight":
                fightListObjects = tempListObjects;
                break;            
            case "Monsters":
                monstersListObjects = tempListObjects;
                break;
            case "Skills":
                skillListObjects = tempListObjects;
                break;
            case "Mystery":
                mysteryListObjects = tempListObjects;
                break;
        }
    }

    public List<GameObject> GetGameObjects(string eventType)
    {
        switch (eventType)
        {
            case "Movement":
                return movementListObjects;
            case "Choices":
                return choiceListObjects;
            case "Fight":
                return fightListObjects;           
            case "Monster":
                return monstersListObjects;
            case "Skills":
                return skillListObjects;
            case "Mystery":
                return mysteryListObjects;
            default:
                return movementListObjects;
        }
    }

    public List<GameObject> GetRandomChoices()
    {
        int i = 0;
        List<GameObject> nextChoices = new List<GameObject>();
        while (i != 2)
        {
            //Get random choices
            int whichItem = Random.Range(0, choiceListObjects.Count);

            nextChoices.Add(choiceListObjects[whichItem]);

            i++;
        }

        return nextChoices;
    }

    public GameObject GetRandomMonster()
    {
        //Get random monster
        int whichItem = Random.Range(0, monstersListObjects.Count);

        return monstersListObjects[whichItem];
    }

    public GameObject GetBoss()
    {
        Object temp = Resources.Load<GameObject>("Prefabs/Cards/Monsters/Boss/1_Boss");

        return (GameObject)temp;
    }
}
