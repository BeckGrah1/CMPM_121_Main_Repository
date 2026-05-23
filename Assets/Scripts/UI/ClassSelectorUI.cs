using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ClassSelectorUI : MonoBehaviour
{
    PlayerClass[] classes;
    public GameObject button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameManager.OnPREGAME += Show;
        GameManager.OnLevelStart += Hide;
    }

    void OnDisable()
    {
        GameManager.OnPREGAME -= Show;
        GameManager.OnLevelStart -= Hide;
    }

    void Show() => gameObject.SetActive(true);
    void Hide() => gameObject.SetActive(false);

    void Start()
    {
        // LevelDataManager.Awake() will ensure Instance exists before this Start() runs- all Awake() must run before any Start()
        classes = PlayerClassManager.Instance.getAllClasses();

        // loop over the list of levels as read in from the json
        // add a button for each level, 60px down from the last
        for (int i = 0; i < classes.Length; i++)
        {
            GameObject selector = Instantiate(button, this.transform);
            selector.transform.localPosition = new Vector3(0, 130 - (i * 60));
            selector.GetComponent<MenuSelectorController>().SetClass(classes[i].name);
        }
    }
}
