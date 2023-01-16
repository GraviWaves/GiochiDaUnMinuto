using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MainMenu/GameCartridge")]
public class ScriptableCartridge : ScriptableObject
{
    public string Title;
    public string Genre;
    public int Players;
    [TextArea] public string Description;
    public Sprite Cover;
    public Enums.SceneName GameScene;
}
