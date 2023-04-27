using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PokemonAlly : MonoBehaviour
{
    #region Variables
    private PokemonEnemy pokemonEnemy;
    private BattleController battleController;

    // Status Player
    private string dialogue;
    private Transform barLife, barExperience;
    private Vector3 vector3;
    private GameObject buttonA, buttonB, buttonC, buttonD;
    public string namePokemon;
    public int level;
    public int experience;
    public float lifePointMax, lifePoint, percentual;
    public string[] actionsPokemon;
    public int[] damageActions;
    public GameObject[] animationsAttack;

    private int idCommand;
    private int hit;

    private int idStage;

    private Text nameTextUi;
    private Text levelTextUi;
    #endregion

    #region Mono
    private void Start()
    {
        pokemonEnemy = FindObjectOfType(typeof(PokemonEnemy)) as PokemonEnemy;
        battleController = FindObjectOfType(typeof(BattleController)) as BattleController;

        nameTextUi = GameObject.Find("NamePokemonAllyText").GetComponent<Text>();
        levelTextUi = GameObject.Find("LevelPokemonAllyText").GetComponent<Text>();
        nameTextUi.text = namePokemon;
        levelTextUi.text = level.ToString();

        // Life Controller
        lifePoint = lifePointMax;
        barLife = GameObject.Find("HP_Player").transform;
        barExperience = GameObject.Find("XP_Player").transform;
        percentual = lifePoint /lifePointMax;
        vector3 = barLife.localScale;
        vector3.x = percentual;
        barLife.localScale = vector3;

        percentual = experience / 100.0f;
        vector3 = barExperience.localScale;
        vector3.x = percentual;
        barExperience.localScale = vector3;
    }
    #endregion

    #region Receive Damage
    public void ReceiveDamage(int hit)
    {
        // Life Controller
        lifePoint -= hit;
        if (lifePoint <= 0)
        {
            lifePoint = 0;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        percentual = lifePoint /lifePointMax;
        vector3 = barLife.localScale;
        vector3.x = percentual;
        barLife.localScale = vector3;
    }
    #endregion

    #region Rename Buttons
    public void RenameButtons()
    {
        // Find Buttons
        buttonA = GameObject.Find("CommandsTextA");
        buttonB = GameObject.Find("CommandsTextB");
        buttonC = GameObject.Find("CommandsTextC");
        buttonD = GameObject.Find("CommandsTextD");

        buttonA.GetComponent<Text>().text = actionsPokemon[0];
        buttonB.GetComponent<Text>().text = actionsPokemon[1];
        buttonC.GetComponent<Text>().text = actionsPokemon[2];
        buttonD.GetComponent<Text>().text = actionsPokemon[3];
    }
    #endregion

    #region Actions Command
    public IEnumerator ActionsCommand(int idAction)
    {
        switch(idAction)
        {
            case 1:
                idCommand = 0;
                dialogue = namePokemon + " use " + actionsPokemon[idCommand] + "!";
                StartCoroutine("Dialogue", dialogue);
                break;
            case 2:
                idCommand = 1;
                dialogue = namePokemon + " use " + actionsPokemon[idCommand] + "!";
                StartCoroutine("Dialogue", dialogue);
                break;
            case 3:
                idCommand = 2;
                dialogue = namePokemon + " use " + actionsPokemon[idCommand] + "!";
                StartCoroutine("Dialogue", dialogue);
                break;
            case 4:
                idCommand = 3;
                dialogue = namePokemon + " use " + actionsPokemon[idCommand] + "!";
                StartCoroutine("Dialogue", dialogue);
                break;
        }
        idStage = 1;
        return null;
    }
    #endregion

    #region Dialogue
    public IEnumerator Dialogue(string txt)
    {
        // Animation Text
        int letter = 0;
        battleController.textBaseSystem.text = "";
        while (letter <= txt.Length - 1)
        {
            battleController.textBaseSystem.text += txt[letter];
            letter += 1;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(1.0f);

        switch (idStage)
        {
            case 1:
                StartCoroutine("ApplyDamage");
                break;
            case 2:
                pokemonEnemy.StartCoroutine("ActionInitial");
                break;
            case 3:
                battleController.menuA.SetActive(true);
                break;
            case 4:
                dialogue = pokemonEnemy.namePokemon + " was defeated.";
                StartCoroutine("Dialogue", dialogue);
                idStage = 5;
                break;
            case 5:
                StartCoroutine("ReceiveXP", pokemonEnemy.experienceReceive);
                idStage = 6;
                break;

            case 6:
                SceneManager.LoadScene(0);
                break;
        }
    }
    #endregion

    #region Apply Damage
    public IEnumerator ApplyDamage()
    {
        // Animation Attack
        GameObject tempAnimAttack = Instantiate(animationsAttack[idCommand]) as GameObject;
        tempAnimAttack.transform.position = pokemonEnemy.transform.position;

        hit = Random.Range(damageActions[idCommand], damageActions[idCommand]);
        dialogue = " ";
        StartCoroutine("Dialogue", dialogue);
        yield return new WaitForSeconds(1.0f);
        pokemonEnemy.ReceiveDamage(hit);
        Destroy(tempAnimAttack);
        if (pokemonEnemy.lifePoint <= 0)
        {
            idStage = 4;
        }
        else
        {
            idStage = 2;
        }
    }
    #endregion

    #region Command Initial
    public void CommandInitial()
    {
        dialogue = "";
        StartCoroutine("Dialogue", dialogue);
        idStage = 3;
    }
    #endregion

    #region Receive XP
    public IEnumerator ReceiveXP(int xpReceive)
    {
        dialogue = namePokemon + " received " + xpReceive + " xp!";
        StartCoroutine("Dialogue", dialogue);
        experience += xpReceive;

        percentual = experience / 100.0f;
        vector3 = barExperience.localScale;
        vector3.x = percentual;
        barExperience.localScale = vector3;

        idStage = 5;

        return null;
    }
    #endregion
}
