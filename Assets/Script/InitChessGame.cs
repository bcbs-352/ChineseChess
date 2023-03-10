using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ֳ�ʼ��������Ԥ����ʵ�����������ŵ�grid��ʼ��
// ��GameController���ã���ֳ�ʼ���󽻸�ChessManager �� GridManager����
public class InitChessGame : MonoBehaviour
{
    #region Ԥ����
    public GameObject ���;
    public GameObject ��ʿ;
    public GameObject ��˧;
    public GameObject ����;
    public GameObject ����;
    public GameObject �쳵;
    public GameObject ����;
    public GameObject ����;
    public GameObject ��ʿ;
    public GameObject �ڽ�;
    public GameObject ����;
    public GameObject ����;
    public GameObject �ڳ�;
    public GameObject ����;
    #endregion
    public static GameObject[] AllChess;
    public static Vector2[] OriginalPos;
    void Awake()
    {
        AllChess = new GameObject[] {
            Instantiate(�ڳ�),Instantiate(����),Instantiate(����),Instantiate(��ʿ),Instantiate(�ڽ�),Instantiate(��ʿ),Instantiate(����),Instantiate(����),Instantiate(�ڳ�),
            Instantiate(����),Instantiate(����),Instantiate(����),Instantiate(����),Instantiate(����),Instantiate(����),Instantiate(����),
            Instantiate(���),Instantiate(���),Instantiate(���),Instantiate(���),Instantiate(���),Instantiate(����),Instantiate(����),
            Instantiate(�쳵),Instantiate(����),Instantiate(����),Instantiate(��ʿ),Instantiate(��˧),Instantiate(��ʿ),Instantiate(����),Instantiate(����),Instantiate(�쳵)
        };
        OriginalPos = new Vector2[]
        {
            new Vector2(0,0), new Vector2(0,1), new Vector2(0,2), new Vector2(0,3), new Vector2(0,4), new Vector2(0,5), new Vector2(0,6), new Vector2(0,7), new Vector2(0,8),
            new Vector2(2,1), new Vector2(2,7), new Vector2(3,0), new Vector2(3,2), new Vector2(3,4), new Vector2(3,6), new Vector2(3,8),
            new Vector2(6,0), new Vector2(6,2), new Vector2(6,4), new Vector2(6,6), new Vector2(6,8), new Vector2(7,1), new Vector2(7,7),
            new Vector2(9,0), new Vector2(9,1), new Vector2(9,2), new Vector2(9,3), new Vector2(9,4), new Vector2(9,5), new Vector2(9,6), new Vector2(9,7), new Vector2(9,8)
        };
        
    }

    public void BtnDown()
    {
        InitAllChess();
        GameController.Init();
    }

    /// <summary>
    /// ��ʼ��/��λ����λ��
    /// </summary>
    public void InitAllChess()
    {
        ChessManager.livePool.Clear();
        ChessManager.diedPool.Clear();
        
        for (int i = 0; i < AllChess.Length; ++i)
        {
            GridManager.Instance.PlaceChess(AllChess[i], OriginalPos[i]);           // ��������
            Chess_Base tmpClass = AllChess[i].GetComponent<Chess_Base>();

            // �����������,��ҪRespawn���ã�����Ϊ�ǳ�ʼ������������������Ҫ�ֶ�ȡ�������¼�,��ͳһSpawn
            // �˴����ܳ�Bug
            if (tmpClass.chessState == ChessState.Died)
                tmpClass.CancelSubscribeEvents(AllChess[i]);                        
            ChessManager.Spawn(AllChess[i]);        // ���������¼�

            tmpClass.currentPos = OriginalPos[i];   // ��������
            tmpClass.chessState=ChessState.Free;    // ����״̬
        }
        GridManager.Instance.UpdateGridData();  // �е��ң�֮��ͳһ��̬��
    }
}
