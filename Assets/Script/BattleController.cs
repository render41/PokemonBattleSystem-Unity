using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    #region Variables
    private PokemonAlly pokemonAlly;
    private PokemonEnemy pokemonEnemy;

    [Header("Dialogue System")]
    public string dialogue;
    public Text textBaseSystem;

    [Header("Stage Atual")]
    public int idStage;

    // Animation Initial Configs
    private Transform trainer;
    private Transform pokemon;
    private Transform positionA, positionB;

    [Header("Menus Game Objects")]
    public GameObject menuA;
    public GameObject menuB;
    #endregion

    #region Mono
    private void Start()
    {
        GetObjects();

        // Menu Configs
        menuA.SetActive(false);
        menuB.SetActive(false);

        // Start Dialogue
        idStage = 0;
        dialogue = "An " + pokemonEnemy.namePokemon + " appeared";
        StartCoroutine("Dialogue", dialogue);
    }

    private void Update()
    {
        // Animation trainer and move pokemon
        if (idStage == 1)
        {
            trainer.GetComponent<Animator>().SetBool("GoPokemon", true);
            float step = 3 * Time.deltaTime;
            trainer.position = Vector3.MoveTowards(trainer.position, positionB.position, step);
            pokemon.position = Vector3.MoveTowards(pokemon.position, positionA.position, step);
        }
    }
    #endregion

    #region Get Objects
    private void GetObjects()
    {
        pokemonAlly = FindObjectOfType(typeof(PokemonAlly)) as PokemonAlly;
        pokemonEnemy = FindObjectOfType(typeof(PokemonEnemy)) as PokemonEnemy;

        trainer = GameObject.Find("Trainer").transform;
        positionA = GameObject.Find("PositionA").transform;
        pokemon = pokemonAlly.transform;
        positionB = GameObject.Find("PositionB").transform;
    }
    #endregion

    #region Dialogue
    public IEnumerator Dialogue(string txt)
    {
        // Animation Text
        int letter = 0;
        textBaseSystem.text = "";
        while (letter <= txt.Length - 1)
        {
            textBaseSystem.text += txt[letter];
            letter += 1;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1);
        idStage += 1;
        
        switch(idStage)
        {
            case 1:
                dialogue = "I choose you " + pokemonAlly.namePokemon + "!";
                StartCoroutine("Dialogue", dialogue);
                break;
            
            case 2:
                pokemonAlly.CommandInitial();
                break;
        }
    }
    #endregion

    #region Fight
    public void Fight()
    {
        menuA.SetActive(false);
        menuB.SetActive(true);

        pokemonAlly.RenameButtons();
    }
    #endregion

    #region Command
    public void Command(int idCommand)
    {
        menuB.SetActive(false);
        pokemonAlly.StartCoroutine("ActionsCommand", idCommand);
    }
    #endregion

    #region Back Fight
    public void BackFight()
    {
        menuA.SetActive(true);
        menuB.SetActive(false);
    }
    #endregion

    #region Exit Battle
    public void ExitBattle()
    {
        Application.Quit();
    }
    #endregion
}
