using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonEnemy : MonoBehaviour
{
    #region Variables
    private PokemonAlly pokemonAlly;
    private BattleController battleController;

    // Status Enemy
    public string namePokemon;
    public int level;
    public int experienceReceive;
    public float lifePointMax, lifePoint, percentualLife;
    public string[] actionsPokemon;
    public int[] damageActions;
    public GameObject[] animationsAttack;
    private string dialogue;
    private Transform barLife;
    private Vector3 vector3;

    private int idCommand;
    private int hit;

    private int idStage;

    private Text nameTextUi;
    private Text levelTextUi;
    #endregion

    #region Mono
    private void Start()
    {
        pokemonAlly = FindObjectOfType(typeof(PokemonAlly)) as PokemonAlly;
        battleController = FindObjectOfType(typeof(BattleController)) as BattleController;

        nameTextUi = GameObject.Find("NamePokemonEnemyText").GetComponent<Text>();
        levelTextUi = GameObject.Find("LevelPokemonEnemyText").GetComponent<Text>();
        nameTextUi.text = namePokemon;
        levelTextUi.text = level.ToString();
    
        // Life Controller
        lifePoint = lifePointMax;
        barLife = GameObject.Find("HP_Enemy").transform;
        percentualLife = lifePoint /lifePointMax;
        vector3 = barLife.localScale;
        vector3.x = percentualLife;
        barLife.localScale = vector3;
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
        percentualLife = lifePoint / lifePointMax;
        vector3 = barLife.localScale;
        vector3.x = percentualLife;
        barLife.localScale = vector3;
    }
    #endregion

    #region Action Initial
    public IEnumerator ActionInitial()
    {
        idCommand = Random.Range(0, actionsPokemon.Length);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine("ActionsCommand", idCommand);
    }
    #endregion

    #region Actions Command
    public IEnumerator ActionsCommand(int idAction)
    {
        switch (idAction)
        {
            case 0:
                StartCoroutine("ApplyDamage");
                break;
            case 1:
                StartCoroutine("ApplyDamage");
                break;
        }

        yield return new WaitForSeconds(1.0f);
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
                pokemonAlly.CommandInitial();
                break;
        }
    }
    #endregion

    #region Apply Damage
    public IEnumerator ApplyDamage()
    {
        // Animation Attack
        GameObject tempAnimAttack = Instantiate(animationsAttack[idCommand]) as GameObject;
        tempAnimAttack.transform.position = pokemonAlly.transform.position;

        hit = Random.Range(1, damageActions[idCommand]);
        dialogue = namePokemon + " use " + actionsPokemon[idCommand] + "!";
        StartCoroutine("Dialogue", dialogue);
        yield return new WaitForSeconds(1.0f);
        Destroy(tempAnimAttack);
        pokemonAlly.ReceiveDamage(hit);
        idStage = 2;
    }
    #endregion
}
