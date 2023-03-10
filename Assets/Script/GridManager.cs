using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ص�Grid�����£��������Ӷ����������ŵ㣬���ŵ����
// �������£���������,�Ϻ��º� ��10��9�У�grid�±��ʾΪi(0~9) j(0~8)
public class GridManager : MonoBehaviour
{
    public static Dictionary<Vector2, Transform> grid2point;    // ����Grid��ά����-�ŵ�Point����
    public static Dictionary<Vector2, GameObject> grid2chess;   // ����Grid��ά����-����Chess����
    public static Dictionary<GameObject, Vector2> chess2grid;   // ����Chess����-����Grid��ά����

    public static List<Vector2> accessiblePoints;

    [SerializeField]
    public float offset = 0.41f;

    public static GridManager instance;
    public static GridManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = new GridManager();
        grid2chess = new Dictionary<Vector2, GameObject>();
        grid2point = new Dictionary<Vector2, Transform>();
        chess2grid = new Dictionary<GameObject, Vector2>();
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                // ��ȡPoint��Transform    ����ʾ��ɫ(Sphere),��������
                Transform point = transform.GetChild(9 * i + j);
                grid2point[new Vector2(i, j)] = point;
                point.GetChild(0).gameObject.SetActive(false);
                point.GetComponent<PointClick>().gridPos = new Vector2(i, j);
            }
        }
    }

    /// <summary>
    ///  �������ӳأ�������������������
    ///  GameControllerÿһ������
    /// </summary>
    public void UpdateGridData()
    {
        chess2grid.Clear();
        grid2chess.Clear();

        for (int i = 0; i < ChessManager.livePool.Count; ++i)
        {
            GameObject chess = ChessManager.livePool[i];
            chess2grid.Add(chess, chess.GetComponent<Chess_Base>().currentPos);
            grid2chess.Add(chess.GetComponent<Chess_Base>().currentPos, chess);
        }
    }

    /// <summary>
    /// ������ŵ��б���ȫ�����ú󽫿��ŵ�������ɫ
    /// </summary>
    /// <param name="points"></param>
    public static void PointTurnGreen(List<Vector2> points)
    {
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                grid2point[new Vector2(i, j)].GetChild(0).gameObject.SetActive(false);
            }
        }
        if (points == null)
            return;
        accessiblePoints = points;
        
        for (int i = 0; i < points.Count; ++i)
        {
            grid2point[points[i]].GetChild(0).gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// ���ݵ�λ�������ӣ����ô˺������轫���ӽ���livePool����
    /// </summary>
    /// <param name="chess"></param>
    /// <param name="gridPos"></param>
    public void PlaceChess(GameObject chess, Vector2 gridPos)
    {
        chess.transform.position = grid2point[gridPos].position + new Vector3(0, offset, 0);
        grid2chess[gridPos] = chess;
        chess2grid[chess] = gridPos;
        UpdateGridData();
    }
}
