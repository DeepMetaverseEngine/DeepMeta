using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud_Image_TmpExample : MonoBehaviour
{
    public GameObject hudgo;
    public int count = 0;
    public Transform cameraTrans;
    public Slider slider;

    private void Awake()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Instantiate(hudgo);
            go.transform.SetParent(transform, false);
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            dir = dir.normalized;
            float len = Random.Range(12, 50);
            go.transform.position = dir * len;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, Time.time * 10, 0);
        Vector3 pos = new Vector3(0, 0, 50 - 150 * slider.value);
        cameraTrans.transform.position = pos;
    }
}
