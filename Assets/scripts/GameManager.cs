using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text textField;
    public InputField nameField;

    public int counter = 0;

    public void myClick() {
        counter++;
        textField.text = counter.ToString();
    }

    public void myClickName()
    {
        textField.text = "Привет " + nameField.text;
    }

}
