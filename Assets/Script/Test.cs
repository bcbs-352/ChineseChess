using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestEnum
{
    Aa,
    Bb,
    Dd
}
public enum TestEnum2
{
    aA,
    bB,
    cC
}
public class Test : MonoBehaviour
{
    //[SerializeField]
    public TestEnum testEnum=TestEnum.Aa;
    public TestEnum2 testEnum2 = TestEnum2.aA;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Test Enum");
        if ((int)testEnum==(int)testEnum2)
            Debug.Log("equal!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
