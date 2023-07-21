using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void selectScene(){
        switch (this.gameObject.name)
        {
            case "MaqMode":
                SceneManager.LoadScene("MannequinMode");
                break;
            case "RackMode":
                SceneManager.LoadScene("RackMode");
                break;          
        }
    }
}