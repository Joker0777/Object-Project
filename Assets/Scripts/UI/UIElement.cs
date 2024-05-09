using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    private TextMeshProUGUI _textUI;
    private string _updatedUIText = "";

    [SerializeField] private UIElementType elementType;


    public UIElementType ElementType
    {
        get { return elementType; }
    }

    public string UpdatedUIText
    {
        set 
        { 
            _updatedUIText = value;
            _textUI.text = $"{elementType} : {UpdatedUIText}";
            Debug.Log("In updated ui text " + _updatedUIText);
        }
        get { return _updatedUIText; }
    }


    private void Awake()
    {
        _textUI = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        _textUI.text = $"{elementType} : {UpdatedUIText}";
    }
}
