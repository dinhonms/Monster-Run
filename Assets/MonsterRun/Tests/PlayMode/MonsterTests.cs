using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Behaviour;
using UnityEditor;

public class MonsterTests
{
    private const float FINISH_LINE_POS = 10f;
    private const float EXECUTION_TIME = 5f;

    //The basic naming of a test comprises of three main parts: 
    //[UnitOfWork_StateUnderTest_ExpectedBehavior]
    [UnityTest]
    public IEnumerator CheckThat_CrossedFinishLine_DeactivateAfterTime()
    {
        var countTime = 0f;
        var monster = AssetDatabase.LoadAssetAtPath<MonsterBehaviour>("Assets/MonsterRun/Prefabs/MonsterPrefab.prefab");
        var mBehaviour = Object.Instantiate(monster);
        
        mBehaviour.Initialize(FINISH_LINE_POS);

        while (countTime < EXECUTION_TIME)
        {
            yield return new WaitForSeconds(1f);
            
            countTime++;
        }

        Assert.That(mBehaviour.transform.position.x, Is.GreaterThanOrEqualTo(FINISH_LINE_POS));
    }
}
