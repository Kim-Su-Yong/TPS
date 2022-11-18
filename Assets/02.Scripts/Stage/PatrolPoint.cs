using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Color lineColor;
    [SerializeField]
    Transform[] lineTrs;
    public List<Transform> Nodes;
    private void OnDrawGizmos() //기즈모(좌표)에 색상이나 선을 그려주는 함수
    {                           //콜백 함수: 스스로 호출

        Gizmos.color = lineColor;

        //자기 자신 트랜폼부터 하위 자식들 트랜스폼을 
        //lineTrs 배열에 담는다. 
        lineTrs = GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>(); //동적 할당 
        for (int i = 0; i < lineTrs.Length; i++)
        {
            if (lineTrs[i] != this.transform)
            {
                Nodes.Add(lineTrs[i]);
            }
        }
        for(int i =0; i<Nodes.Count; i++)
        {         //현재 노드 
            Vector3 CurrentNode = Nodes[i].position;
            Vector3 PrevousNode = Vector3.zero; 
            //이전노드 
            if(i>0)
            {
                PrevousNode = Nodes[i - 1].position;
            }
            if(i==0 && Nodes.Count >1)
            {
                PrevousNode = Nodes[Nodes.Count - 1].position;
            }
            Gizmos.DrawSphere(CurrentNode, 0.5f);
            // point 오브젝트에 원형으로 표시 
            Gizmos.DrawLine(PrevousNode, CurrentNode);
            //이전 노드와 현재노드를 선으로 표시 
        }

    }

}
