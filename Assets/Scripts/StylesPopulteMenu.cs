using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylesPopulteMenu : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject Container;

    public void AddCard(string name)
    {
        GameObject Card = Instantiate(CardPrefab);
        Card.transform.SetParent(Container.transform, false);
        string[] Split = name.Split(":");
        Card.GetComponent<Button_StyleMenu>().Name = Split[0];
        Card.GetComponent<Button_StyleMenu>().Description = Split[1];
        Card.GetComponent<Button_StyleMenu>().size = Split[2];
        Debug.Log(name);
    }


}
