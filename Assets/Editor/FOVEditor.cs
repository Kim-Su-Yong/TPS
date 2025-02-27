﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        //EnemyFOV 클래스를 참조
        EnemyFOV fov = (EnemyFOV)target;
        //원주위의 시작점의 좌표를 계산(주어진 각도의 1/2)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        //원의 색상을 흰색으로 지정
        Handles.color = Color.white;        //원점 좌표, 노멀 벡터, 원의 반지름
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);
        //부채꼴 색상을 그림
        Handles.color = new Color(1, 1, 1, 0.2f);
        //채워진 부채꼴을 그림
                                            //원점 좌표, 노멀 벡터, 부채꼴의 시작 좌표, 부채꼴의 각도, 부채꼴의 반지름
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);
        //시야 각을 텍스트로 표시
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f),
            fov.viewAngle.ToString());
    }
}
