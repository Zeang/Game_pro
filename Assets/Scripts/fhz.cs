using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class fhz : MonoBehaviour
{
    public Material shieldMaterial;
    public int pointsCount = 20;
    public float pointRange = 0.5f;
    public float inTime = 0.5f;
    public float outTime = 0.5f;
    public Ease ease;
    public List<HitPoint> hitPoints = new List<HitPoint>();
    public List<Vector4> vecArray = new List<Vector4>();
    public Slider sl;

    public float leftTime = 20;
    private bool isUse = true;

    void Start()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            hitPoints.Add(new HitPoint());
            vecArray.Add(Vector4.zero);
        }
        shieldMaterial.SetFloat("_Opacity", 0.6f);
    }

    void Update()
    {
        if (leftTime < 0 && isUse)
        {
            transform.GetComponent<Collider>().enabled = false;
            Debug.Log("失效了");
            isUse = false;
        }
        else if (isUse)
        {
            leftTime -= Time.deltaTime;

        }
        if (leftTime < 1.0f)
        {
            float opa = (float)(leftTime * 0.6);
            if (opa < 0) opa = 0;
            shieldMaterial.SetFloat("_Opacity", opa);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit raycastHit;
        //    if (Physics.Raycast(ray, out raycastHit, 100))
        //    {
        //        var index = -1;
        //        foreach (var item in hitPoints)
        //        {
        //            if (item.complete)
        //            {
        //                index = hitPoints.IndexOf(item);
        //                break;
        //            }
        //        }
        //        if (index >= 0)
        //        {
        //            var hitPoint = new HitPoint();
        //            hitPoint.complete = false;
        //            hitPoint.position = new Vector4(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z, 0);
        //            hitPoints[index] = hitPoint;
        //            DOTween.To(() => hitPoint.range, x => hitPoint.range = x, pointRange, inTime).OnComplete(() => { DOTween.To(() => hitPoint.range, x => hitPoint.range = x, 0f, outTime).OnComplete(() => { hitPoint.complete = true; }).SetEase(ease); }).SetEase(ease);
        //        }
        //    }
        //}
        foreach (var item in hitPoints)
        {
            var p = item.position;
            item.position = new Vector4(p.x, p.y, p.z, item.range);
            vecArray[hitPoints.IndexOf(item)] = item.position;
        }
        shieldMaterial.SetVectorArray("_Array", vecArray);

    }

    public void ActiveFhz()
    {
        if (leftTime < 10)
        {
            shieldMaterial.SetFloat("_Opacity", 0.6f);
            transform.GetComponent<Collider>().enabled = true;
            leftTime += 10.0f;
            isUse = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("画了一个涟漪");
        var index = -1;
        foreach (var item in hitPoints)
        {
            if (item.complete)
            {
                index = hitPoints.IndexOf(item);
                break;
            }
        }
        if (index >= 0)
        {
            var hitPoint = new HitPoint();
            hitPoint.complete = false;
            hitPoint.position = new Vector4(collision.collider.transform.position.x, collision.collider.transform.position.y, collision.collider.transform.position.z, 0);
            hitPoints[index] = hitPoint;
            DOTween.To(() => hitPoint.range, x => hitPoint.range = x, pointRange, inTime).OnComplete(() => { DOTween.To(() => hitPoint.range, x => hitPoint.range = x, 0f, outTime).OnComplete(() => { hitPoint.complete = true; Debug.Log("画了一个涟漪"); }).SetEase(ease); }).SetEase(ease);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        var index = -1;
        foreach (var item in hitPoints)
        {
            if (item.complete)
            {
                index = hitPoints.IndexOf(item);
                break;
            }
        }
        if (index >= 0)
        {
            var hitPoint = new HitPoint();
            hitPoint.complete = false;
            hitPoint.position = new Vector4(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z, 0);
            hitPoints[index] = hitPoint;
            DOTween.To(() => hitPoint.range, x => hitPoint.range = x, pointRange, inTime).OnComplete(() => { DOTween.To(() => hitPoint.range, x => hitPoint.range = x, 0f, outTime).OnComplete(() => { hitPoint.complete = true; Debug.Log("画了一个涟漪"); }).SetEase(ease); }).SetEase(ease);
        }
    }


}

public class HitPoint
{
    public Vector4 position = Vector4.zero;
    public float time = 0;
    public float range = 0.1f;
    public bool complete = true;
}