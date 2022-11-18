using System.Collections.Generic;
using System.Collections;
namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int killCount = 0; //사망 캐릭터수
        public float hp = 120f; //초기 생명치
        public float damage = 25.0f; //총알 데미지
        public float speed = 6.0f; //이동 속도
        public List<Item> equipItem = new List<Item>();
    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP,SPEED,GRENADE,DAMAGE } //아이템 종류 선언
        public enum ItemCalc { INC_VALUE,PERCENT } //계산 방식
        public ItemType itemType; //아이템 종류
        public ItemCalc itemCalc; //아이템 적용시 계산 방식
        public string name; //아이템 이름
        public string desc; //아이템 소개
        public float value; //계산
    }
}
