using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveSide
{
    ��,
    ��
}

/// <summary>
/// ��UCCI��¼���
/// </summary>
public class GameController : MonoBehaviour
{
    // ��¼����״̬  �Ȳ���
    public static List<GameObject> lastStepRecord;

    public static MoveSide moveSide = MoveSide.��;
    public static int step = 0;

    private static GameController instance;
    
    public static GameObject textObj;
    public static GameController Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        lastStepRecord = new List<GameObject>();
        textObj = GameObject.Find("FlagText");
    }
    void Start()
    {

    }
    public static void Init()
    {
        step = 0;
        moveSide = MoveSide.��;
        ChessManager.UpdateChessCollider(moveSide);
        UpdateText();
    }
    /// <summary>
    /// ����һ��֮�����
    /// </summary>
    public static void Step()
    {
        moveSide = (moveSide == MoveSide.��) ? MoveSide.�� : MoveSide.��;
        ChessManager.UpdateChessCollider(moveSide);
        UpdateText();
    }
    public static void UpdateText()
    {
        Text tmpText = textObj.GetComponent<Text>();
        if (moveSide == MoveSide.��)
        {
            tmpText.text = "���巽����";
            tmpText.color = Color.red;
        }
        else
        {
            tmpText.text = "���巽����";
            tmpText.color = Color.black;
        }
    }
}
