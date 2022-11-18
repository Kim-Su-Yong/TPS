using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public void OnclickStartBtn()
    {
        SceneManager.LoadScene("SceneLoader");
        //SceneManager.LoadScene("Level_1");
        //SceneManager.LoadScene("BattleFieldScene",LoadSceneMode.Additive);
        //SceneManager.LoadScene("BattelFieldScene", LoadSceneMode.Single);
        //기존에 있는 씬을 삭제하고 새로운 씬을 로드한다.
        //SceneManager.CreateScene(); //새로운 빈 씬을 생성한다.
        //SceneManager.LoadSceneAsync(); //씬을 비동기 방식으로 로드한다.
        //SceneManager.MergeScenes(); //소스 씬을 다른씬으로 통합한다.
        //소스씬은 모든 게임 오브젝트가 통합된 이후 삭제된다.
        //SceneManager.MoveGameObjectToScene();
        //현재 씬에 있는 특정 오브젝트를 다른 씬으로 이동한다.
        //SceneManager.UnloadScene(); //현재씬에 있는 모든 게임 오브젝트를 삭제한다.
    }
}
