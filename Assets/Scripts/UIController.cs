using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Selection
{
    public string name;
    public delegate void SelectionDelegate();
    public SelectionDelegate selection;
    public Selection(SelectionDelegate selection, string name)
    {
        this.name = name;
        this.selection = selection;
    }
}

public class UIController
{
    public delegate void SelectionDelegate();
    static public void CreateSelectionBox(params Selection[] selections)
    {
        GameObject canvas = GameObject.Find("Canvas");
        List<GameObject> buttons = new List<GameObject>();
        int i = 0;
        foreach (Selection s in selections)
        {
            GameObject btn = DefaultControls.CreateButton(new DefaultControls.Resources());
            btn.transform.SetParent(canvas.transform, false);
            btn.GetComponent<Button>().onClick.AddListener(s.selection.Invoke);
            btn.GetComponentInChildren<Text>().text = s.name;
            btn.transform.localScale = new Vector3(10, 3);
            btn.transform.position = new Vector3(btn.transform.position.x, 1000 - i * 100);
            buttons.Add(btn);
            i++;
        }
        foreach (GameObject btn in buttons)
        {
            btn.GetComponent<Button>().onClick.AddListener(() => { foreach (GameObject go in buttons) { Object.Destroy(go); } });
        }
    }


    static public void CreateSelectionBox(List<Selection> selections)
    {
        GameObject canvas = GameObject.Find("Canvas");
        List<GameObject> buttons = new List<GameObject>();
        int i = 0;
        foreach (Selection s in selections)
        {
            GameObject btn = DefaultControls.CreateButton(new DefaultControls.Resources());
            btn.transform.SetParent(canvas.transform, false);
            btn.GetComponent<Button>().onClick.AddListener(s.selection.Invoke);
            btn.GetComponentInChildren<Text>().text = s.name;
            btn.transform.localScale = new Vector3(10, 3);
            btn.transform.position = new Vector3(btn.transform.position.x, 1000 - i * 100);
            buttons.Add(btn);
            i++;
        }
        foreach (GameObject btn in buttons)
        {
            btn.GetComponent<Button>().onClick.AddListener(() => { foreach (GameObject go in buttons) { Object.Destroy(go); } });
        }
    }

}
