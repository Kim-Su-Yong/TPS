using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
//어튜리뷰트를 사용해서 자동 생성 
[CreateAssetMenu(fileName = "GameDataSO",
    menuName = "Create GameData", order=1)]
                                    //생성할 메뉴의 순서를 결정
public class GameDataObject : ScriptableObject
{
    //ScriptableObject의 활용 분야는 매우 다양하다.
    //가장 흔히 볼 수 있는 것으로 인벤토리의 데이터 저장이나
    //아이템의 설정 및 발생 빈도 등에 있다.
    //이러한 데이터를 유니티 에디터에서 손쉽게 설정하고
    //확인할 수 있다.

    public int killCount = 0;
    public float hp = 120f;
    public float damage = 25f;
    public float speed = 6.0f;
    public List<Item>equipItem = new List<Item>();
}
