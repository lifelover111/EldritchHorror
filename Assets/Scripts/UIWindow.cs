using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour 
{
    [SerializeField] public GameObject buttonPrefab;
    public static UIWindow UIwindow;
    private GameObject canvas;
    private List<GameObject> buttons;

    private void Awake()
    {
        UIwindow = this;
        canvas = GameObject.Find("Canvas");
        buttons = new List<GameObject>();
    }

    public void CreateWindow(Action[] ps)
    {
        int i = 0;
        foreach (Action t in ps)
        {

            GameObject btn = DefaultControls.CreateButton(new DefaultControls.Resources());
            btn.transform.SetParent(canvas.transform, false);
            btn.GetComponent<Button>().onClick.AddListener(t.action.Invoke);
            btn.GetComponent<Button>().onClick.AddListener(() => { btn.GetComponent<Button>().onClick.RemoveAllListeners(); });
            btn.GetComponentInChildren<Text>().text = t.name;
            btn.transform.localScale = new Vector3(10,3);
            btn.transform.position = new Vector3(btn.transform.position.x,1000 - i * 100);
            buttons.Add(btn);
            i++;
        }
    }

    public void HideWindow()
    {
        foreach (GameObject btn in buttons)
        {
            btn.SetActive(false);
        }

    }

    public void ShowWindow()
    {
        foreach (GameObject btn in buttons)
        {
            btn.SetActive(true);
        }

    }

    public void DeleteWindow()
    {
        foreach (GameObject btn in buttons)
        {
            Destroy(btn);
        }
        buttons.Clear();
    }
}
