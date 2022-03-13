using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    //**UI**
    //Movement Buttons
    public GameObject choice1Button;
    public Text choice1Text;
    public GameObject choice2Button;
    public Text choice2Text;
    public HealthBar playerHealthBar;
    public HealthBar monsterHealthBar;

    //Common Display
    public Text storyText;
    public TextMeshProUGUI currentLevelText;
    public GameObject backgroundImage;
    public GameObject ImageLeft;
    public GameObject ImageRight;
    public GameObject frogBackImage;

    //Fight Display
    public GameObject prefabSkillButton;
    public RectTransform skillButonPanel;
    public GameObject fightPanel;


    //**Data**
    //Repo
    GameRepo gameRepo;
    PlayerRepo playerRepo;

    //Currents
    protected GameObject currentEventObj;
    protected List<GameObject> currentChoiceObjs = new List<GameObject>();
    protected int currentLevel = 1;
    GameObject currentMonsterObject;
    MonsterCard currentMonsterCard;


    //test image
    Sprite frogImage;

    // Start is called before the first frame update
    void Start()
    {
        fightPanel.SetActive(false);
        gameRepo = new GameRepo();
        playerRepo = new PlayerRepo(playerHealthBar);

        //Add Fire skill
        playerRepo.skillCards.Add(gameRepo.GetGameObjects("Skills")[0].GetComponent<SkillCard>());
        playerRepo.skillCards.Add(gameRepo.GetGameObjects("Skills")[1].GetComponent<SkillCard>());

        choice1Button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(0));
        choice2Button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(1));

        LoadFirstScreen();

        frogImage = Resources.Load<Sprite>("Images/Frog/fightfrog");
    }

    private void FixedUpdate()
    {
        currentLevelText.text = "Current Level: " + currentLevel;
    }

    void ButtonClick(int choiceNo)
    {
        //get event type from choice
        ChoiceCard buttonClicked = currentChoiceObjs[choiceNo].GetComponent<ChoiceCard>();
        string nextEvent = "Movement";

        List<KeyValuePair<string, int>> elements = new List<KeyValuePair<string, int>> {
            new KeyValuePair<string, int>("Movement", buttonClicked.movementPercentage),
            new KeyValuePair<string, int>("Fight", buttonClicked.monsterPercentage),
            new KeyValuePair<string, int>("Mystery", buttonClicked.mysteryPercentage),
        };

        //Randomize next event based on % chance from choice
        double cumulative = 0.0;
        int dice = Random.Range(0, 100);
        Debug.Log(dice);
        for (int i = 0; i < elements.Count; i++)
        {
            cumulative += elements[i].Value;
            if (dice < cumulative)
            {
                nextEvent = elements[i].Key;
                break;
            }
        }

        SelectRandomEvent(nextEvent);
    }

    void SelectRandomEvent(string eventType)
    {
        //Get random of certain eventType
        //change to juyst return one random not whole list
        List<GameObject> myListObjects = gameRepo.GetGameObjects(eventType);

        int whichItem = Random.Range(0, myListObjects.Count);

        //Remove currentEvent
        if (currentEventObj != null) Destroy(currentEventObj);

        //Start new event
        currentEventObj = Instantiate(myListObjects[whichItem]) as GameObject;
        SetEventUI(currentEventObj.GetComponent<EventCard>());
    }

    void SetEventUI(EventCard nextEvent)
    {
        currentLevel++;

        if (nextEvent.eventType == "Movement")
        {
            movementEvent(nextEvent);
        }
        else if (nextEvent.eventType == "Fight")
        {

            monsterEvent(nextEvent);
        }
        else if (nextEvent.eventType == "Mystery")
        {
            mysteryEvent(nextEvent);
        }
    }

    void LoadFirstScreen()
    {
        currentEventObj = Instantiate(gameRepo.GetGameObjects("Movement")[0]);
        currentChoiceObjs.Add(Instantiate(gameRepo.GetGameObjects("Choices")[0]));
        currentChoiceObjs.Add(Instantiate(gameRepo.GetGameObjects("Choices")[1]));

        storyText.text = "Before you lies a " + currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceDescription + " and a " + currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceDescription;
        choice1Text.text = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceDescription;
        choice2Text.text = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceDescription;
        ImageLeft.GetComponent<Image>().sprite = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceImage;
        ImageRight.GetComponent<Image>().sprite = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceImage;
    }

    void movementEvent(EventCard nextEvent)
    {
        //Get Choices
        RemoveCurrentChoicesAndSetNew();
        choice1Button.GetComponent<Button>().onClick.RemoveAllListeners();
        choice1Button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(0));

        storyText.text = "Before you lies a " + currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceDescription + " and a " + currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceDescription;
        choice1Text.text = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceDescription;
        choice2Text.text = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceDescription;

        ImageLeft.GetComponent<Image>().sprite = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceImage;
        ImageRight.GetComponent<Image>().sprite = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceImage;
        backgroundImage.GetComponent<Image>().sprite = nextEvent.backgroundImage;

        choice1Button.SetActive(true);
        choice2Button.SetActive(true);
        frogBackImage.SetActive(true);
        fightPanel.SetActive(false);
    }

    void monsterEvent(EventCard nextEvent)
    {
        if (currentMonsterObject != null)
            Destroy(currentMonsterObject);

        //Get Monster
        GameObject nextMonster;
        if (currentLevel > 9)
        {
            nextMonster = gameRepo.GetBoss();
        }
        else
        {
            nextMonster = gameRepo.GetRandomMonster();
        }
        nextMonster.GetComponent<MonsterCard>().healthBar = monsterHealthBar;
        currentMonsterObject = Instantiate(nextMonster);

        currentMonsterCard = currentMonsterObject.GetComponent<MonsterCard>();
        currentMonsterCard.setUp(monsterHealthBar);

        storyText.text = "You've come across a " + currentMonsterCard.monsterName + " " + currentMonsterCard.description;
        choice1Text.text = "Attack";
        choice2Text.text = "Run";

        ImageLeft.GetComponent<Image>().sprite = frogImage;
        ImageRight.GetComponent<Image>().sprite = currentMonsterCard.sprite;
        backgroundImage.GetComponent<Image>().sprite = nextEvent.backgroundImage;

        choice1Button.SetActive(false);
        choice2Button.SetActive(false);
        frogBackImage.SetActive(false);
        fightPanel.SetActive(true);

        //Only display on fight - hide when not used... move to fight controller
        if (skillButonPanel.transform.childCount > 0)
        {
            foreach (Transform child in skillButonPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < playerRepo.skillCards.Count; i++)
        {
            prefabSkillButton.GetComponent<SkillCard>().description = playerRepo.skillCards[i].GetComponent<SkillCard>().description;
            prefabSkillButton.GetComponent<SkillCard>().potency = playerRepo.skillCards[i].GetComponent<SkillCard>().potency;
            prefabSkillButton.GetComponent<SkillCard>().skillname = playerRepo.skillCards[i].GetComponent<SkillCard>().skillname;
            prefabSkillButton.GetComponent<SkillCard>().skillType = playerRepo.skillCards[i].GetComponent<SkillCard>().skillType;
            prefabSkillButton.GetComponent<Image>().sprite = playerRepo.skillCards[i].GetComponent<SkillCard>().sprite;
            GameObject goButton = (GameObject)Instantiate(prefabSkillButton);

            goButton.transform.SetParent(skillButonPanel, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);

            Button tempButton = goButton.GetComponent<Button>();

            tempButton.onClick.AddListener(() => onSkillClick(currentMonsterCard, goButton.GetComponent<SkillCard>()));
        }
    }

    void mysteryEvent(EventCard nextEvent)
    {
        choice1Button.GetComponent<Button>().onClick.RemoveAllListeners();
        choice1Button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(0));
        storyText.text = "Item! Mysety!";
        choice1Text.text = "Pick Up Goblin Item";
        choice2Text.text = "Run";
        backgroundImage.GetComponent<Image>().sprite = nextEvent.backgroundImage;
        choice1Button.SetActive(true);
        choice2Button.SetActive(true);
        frogBackImage.SetActive(true);
        fightPanel.SetActive(false);
    }

    void onSkillClick(MonsterCard monster, SkillCard skillCard)
    {
        switch (skillCard.skillType)
        {
            case SkillCard.eSkillType.Attack:
                hit(monster, skillCard.potency);
                break;
            case SkillCard.eSkillType.Heal:
                playerRepo.Heal(skillCard.potency);
                break;
            case SkillCard.eSkillType.Debuff:

                break;
        }

        playerRepo.TakeHit(monster.attack);

        if (monster.health <= 0)
        {
            storyText.text = "You defeated " + currentMonsterCard.monsterName + "!";
            choice1Text.text = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceDescription;
            choice2Text.text = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceDescription;
            ImageLeft.GetComponent<Image>().sprite = currentChoiceObjs[0].GetComponent<ChoiceCard>().choiceImage;
            ImageRight.GetComponent<Image>().sprite = currentChoiceObjs[1].GetComponent<ChoiceCard>().choiceImage;
            choice1Button.GetComponent<Button>().onClick.RemoveAllListeners();
            choice1Button.GetComponent<Button>().onClick.AddListener(() => ButtonClick(0));
            fightPanel.SetActive(false);
            choice1Button.SetActive(true);
            choice2Button.SetActive(true);
            frogBackImage.SetActive(true);

            if (currentMonsterObject != null)
                Destroy(currentMonsterObject);
        }
    }
    void hit(MonsterCard monster, int damage)
    {
        if (monster && damage > 0)
        {
            monster.TakeHit(damage);
        }
        else
        {
            monster.TakeHit(playerRepo.attackDmg);
        }
    }

    void RemoveCurrentChoicesAndSetNew()
    {
        //Remove currentChoices
        if (currentChoiceObjs.Count > 0)
        {
            foreach (GameObject choice in currentChoiceObjs)
            {
                if(choice != null)
                    Destroy(choice);
            }
        }

        currentChoiceObjs = new List<GameObject>();
        List<GameObject> nextChoices =  gameRepo.GetRandomChoices();
        foreach(GameObject choice in nextChoices)
        {
           currentChoiceObjs.Add(Instantiate(choice));
        }
    }
}
