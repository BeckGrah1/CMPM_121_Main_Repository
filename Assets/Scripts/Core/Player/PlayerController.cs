using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;
    public PlayerClass playerClass;
    public RPNReturns rpnValues;
    public int speed;

    public Unit unit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        // defaulting to mage currently, in assignment 4 we will modify this to use what the player chooses
        playerClass = PlayerClassManager.Instance.getPlayerClassByName("mage");
        rpnValues = CalculateSpawnExpressions.CalculatePlayerStats(playerClass);
        spellcaster = new SpellCaster(rpnValues.mana, rpnValues.mana_regen, rpnValues.spellpower, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(rpnValues.hp, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spell);
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*rpnValues.speed;
        EventBus.Instance.DoPlayerMoved();
    }

    void Die()
    {
        if (GameManager.Instance.state == GameManager.GameState.INWAVE)
        {
            GameManager.Instance.SetState(GameManager.GameState.GAMEOVER);
        }
    }

    // OnEnable(): Unity calls this when this GameObject becomes active
    // while the object is active, run StartLevel when GameManager.OnLevelStart is invoked
    // this allows other things to tell the game manager to start, and the game manager to tell the player to start
    void OnEnable()
    {
        GameManager.OnLevelStart += StartLevel;
        GameManager.OnGameReset += ResetPlayer;
    }

    // stop listening when the player GameObject is no longer active
    void OnDisable()
    {
        GameManager.OnLevelStart -= StartLevel;
        GameManager.OnGameReset -= ResetPlayer;
    }

    public void ResetPlayer()
    {
        hp = new Hittable(rpnValues.hp, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
    }

    public void UpdateRPNValues()
    {
        rpnValues = CalculateSpawnExpressions.CalculatePlayerStats(playerClass);
    }

    public void HealPlayer(int healAmount)
    {
        hp.hp = Mathf.Min(hp.hp + healAmount, hp.max_hp);
        if (hp.hp == hp.max_hp)
        {
            EventBus.Instance.DoFullHP();
        }
    }

}
