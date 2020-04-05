using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class CallAnt
{
    //蟻の参照用オブジェクト
    private GameObject m_AntRef;
    //蟻が出てくる穴参照用オブジェクト
    private GameObject m_AntHoleRef;
    //蟻が出てくる穴
    private GameObject m_AntHole;

    private const string ANT_PATH = "Prefabs/Ant";
    private const string ANTHOLE_PATH = "Prefabs/AntHole";
    private Character m_Owner;
    //現在生成した蟻の数
    private int m_CreateAntCount;
    //1フレームで生成する蟻の数
    private const int CREATE_COUNT_1FRAME = 10;
    //生成する蟻の合計
    private const int CREATE_MAX_COUNT = 100000;
    //蟻を生成する最大半径
    private const float CREATE_ANT_RADIUS = 1f;
    //蟻の進行方向決定用基準点
    private Vector3 m_AntDirDecidePos;

    public void OnAwake(Character owner)
    {
        m_Owner = owner;
        m_AntRef = Resources.Load(ANT_PATH) as GameObject;
        m_AntHoleRef = Resources.Load(ANTHOLE_PATH) as GameObject;
    }

    public void StartCallAnt()
    {
        CreateAntHole(() => 
        {
            m_Owner.StartCoroutine(CreateAntCor());
        });
    }

    private IEnumerator CreateAntCor()
    {
        while(true)
        {
            for(int i = 0; i < CREATE_COUNT_1FRAME; i++)
            {
                CreateAnt();
            }

            m_CreateAntCount += CREATE_COUNT_1FRAME;
            if(m_CreateAntCount >= CREATE_MAX_COUNT)
            {
                yield break;
            }
            yield return null;
        }
    }

    private void CreateAnt()
    {
        var randPos = Random.insideUnitCircle * CREATE_ANT_RADIUS;
        var createPos = m_AntHole.transform.position + new Vector3(randPos.x, - 0.1f, randPos.y);
        //後で上に上げるためあらかじめ少し下に生成しておく
        GameObject antObj = GameObject.Instantiate(m_AntRef, createPos, Quaternion.identity);
        antObj.transform.DOLocalMoveY(0.1f, 1.0f).OnComplete(() => 
        {
            var ant = antObj.GetComponent<Ant>();
            var diff = (antObj.transform.position - m_AntDirDecidePos);
            diff.y = 0f;
            var dir = diff.normalized;
            ant.Initialize(dir);
        });
    }

    private void CreateAntHole(System.Action onComplete)
    {
        m_AntHole = GameObject.Instantiate(m_AntHoleRef, m_Owner.transform);
        var ren = m_AntHole.gameObject.GetComponent<MeshRenderer>();
        var mat = ren.sharedMaterial;
        DOVirtual.Float(0f, 1f, 2f, value =>
        {
            mat.SetFloat("_Alpha", value);
        })
        .OnComplete(() => onComplete());
        m_AntDirDecidePos = m_AntHole.transform.Find("DirDecidePos").position;
    }
}
