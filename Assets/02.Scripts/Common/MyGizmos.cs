using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL, WAYPOINT}
    private const string wayPointFile = "ENEMY";
    public Type type = Type.NORMAL;
    public Color _color = Color.red;
    public float _radius = 0.5f;
    private void OnDrawGizmos() //좌표 기즈모에 색상이나 선을 그려주는 함수
    {
        if(type == Type.NORMAL)
        {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
                            //위치                                    ,파일명    ,스케일 적용여부
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}