using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float Volume = 1.0f; //볼륨
    public bool isSoundMute = false; //사운드 뮤트 기능 음소거
    public static SoundManager soundmanager;
    //싱글턴 디자인 기법
    private void Awake()
    {
        //soundmanager = this;
        if (soundmanager == null)
            soundmanager = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        //다음 씬으로 넘어가더라도 사라지지 않는다.
    }
    //사운드 공용함수
    public void PlaySoundFunc(Vector3 pos, AudioClip audioClip)
    {
        if (isSoundMute) return;
        //음소거 옵션이 설정되면 바로 빠져나감
        //게임 오브젝트를 동적으로 생성
        GameObject soundObj = new GameObject("Sfx");
        //사운드 발생 위치 지정
        soundObj.transform.position = pos;
        //생성한 게임 오브젝트에 오디오소스 컴퍼넌트 추가
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = audioClip;
        source.minDistance = 10f;
        source.maxDistance = 30f;
        source.volume = Volume;
        source.Play();
        Destroy(soundObj, audioClip.length);
    }
}
