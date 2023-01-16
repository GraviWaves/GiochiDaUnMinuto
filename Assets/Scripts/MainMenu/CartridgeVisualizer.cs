using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartridgeVisualizer : MonoBehaviour
{
    public ScriptableCartridge Info { get; private set; }
    [SerializeField] private Image cover;


    public void SetInfo(ScriptableCartridge cartridgeInfo)
    {
        Info = cartridgeInfo;
        cover.sprite = cartridgeInfo.Cover;
    }
}
