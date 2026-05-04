using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public int maxHp = 100;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;

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
        spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(maxHp, Hittable.Team.PLAYER, gameObject);
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
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        if (GameManager.Instance.state == GameManager.GameState.INWAVE)
        {
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;
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
        hp = new Hittable(maxHp, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
    }

}
