using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    GameCore core;
    Canvas canvas;
    Slider slider;
    Text speedText;
    Text nameText;

    void Start() {
        core = FindObjectOfType<GameCore>();
        canvas = FindObjectOfType<Canvas>();

        slider = canvas.GetComponentInChildren<Slider>();  
        slider.onValueChanged.AddListener(ValueChangeCheck);

        Button[] buttons = canvas.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
            button.onClick.AddListener(delegate { OnButtonClick(button); });

        var texts = canvas.GetComponentsInChildren<Text>();
        speedText = texts.FirstOrDefault(a => a.name == "SpeedText");
        nameText  = texts.FirstOrDefault(a => a.name == "NameText");
       
    }

    public void OnButtonClick(Button b) {
        switch (b.name) { 
            case "LeftButton":
                core.SwitchToPreviosView();
            break;
            case "RightButton":
                core.SwitchToNextView();                
                break;
        }     
    }

    public void ValueChangeCheck(float value) {
        core.SetSpeed(value);       
    }

    private void Update() {
        MovingObjectDTO model  = core.GetModel();
        slider.value = model.GetSpeedRealValue();
        speedText.text = string.Format("Speed: {0:00.0}",model.Speed);
        nameText.text = string.Format("Name: {0}",model.Name);
    }

}
