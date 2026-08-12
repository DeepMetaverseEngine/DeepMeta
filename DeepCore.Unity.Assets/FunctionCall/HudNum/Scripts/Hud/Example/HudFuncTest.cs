using System.Collections.Generic;
using NFCore.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class HudFuncTest : MonoBehaviour
{

    public GameObject hudgo;
    public Transform cameraTrans;
    public int count = 300;

    private List<HudNum> testlist = new List<HudNum>(5000);
    private List<Transform> testTrans = new List<Transform>(5000);

    private bool _Start = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_Start) return;

        transform.rotation = Quaternion.Euler(0, Time.time * 5, 0);
    }

    [Button("开始测试")]
    private void StartTest()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Instantiate(hudgo);
            go.transform.SetParent(transform, false);
            HudCanvasRenderer canvasRenderer = go.GetComponent<HudCanvasRenderer>();
            if (canvasRenderer != null)
            {
                HudNum humNum = canvasRenderer.GetHudComponet<HudNum>("Num");
                testlist.Add(humNum);
                if (humNum != null)
                {
                    //string numstr = Random.Range(-99999999, 99999999).ToString();
                    //numstr = i.ToString();//
                    //humNum.num = numstr;
                    humNum.Num = Random.Range(-99999999, 99999999);
                    humNum.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
                }
            }
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            dir = dir.normalized;
            float len = Random.Range(12, 50);
            go.transform.position = cameraTrans.position  + dir * len;
            //go.transform.position = new Vector3(10, 0, -i);//
            testTrans.Add(go.transform);//
        }


        gameObject.AddComponent<HudRun>();
        _Start = true;
    }
}
