using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MainMenu/GameCartridge")]
public class TestScriptable : ScriptableObject
{
    [SerializeField] public string Title;
    [SerializeField] public string Type;
    [SerializeField] public int Players;
    [TextArea] public string Description;
}
