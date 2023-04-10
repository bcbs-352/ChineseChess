using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveSide
{
    红,
    黑
}

/// <summary>
/// 以UCCI记录棋局
/// </summary>
public class GameController : MonoBehaviour
{
    // 记录棋盘状态  先不做
    public static List<GameObject> lastStepRecord;

    public static MoveSide moveSide = MoveSide.红;
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
        moveSide = MoveSide.红;
        ChessManager.UpdateChessCollider(moveSide);
        UpdateText();
    }
    /// <summary>
    /// 下完一步之后调用
    /// </summary>
    public static void Step()
    {
        moveSide = (moveSide == MoveSide.红) ? MoveSide.黑 : MoveSide.红;
        ChessManager.UpdateChessCollider(moveSide);
        UpdateText();
    }
    public static void UpdateText()
    {
        Text tmpText = textObj.GetComponent<Text>();
        if (moveSide == MoveSide.红)
        {
            tmpText.text = "行棋方：红";
            tmpText.color = Color.red;
        }
        else
        {
            tmpText.text = "行棋方：黑";
            tmpText.color = Color.black;
        }
    }
}
