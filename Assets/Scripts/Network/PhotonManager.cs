using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 포톤 서버 연결(접속)을 시도하는 메서드 : Name server 접속, 지역 서버 선택 후 마스터 서버와 연결하고 인증까지 되면 OnConnectedToMaster() 콜백을 호출한다.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버에 연결하였습니다.");
        
        // 랜덤 룸에 참거하거나 새로운 룸을 생성
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 참가에 실패하였습니다.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 접속하였습니다.");
        SpawnPlayer();
    }

    
    //방에 접속하면 랜덤 포지션에 플레이어를 스폰
    // 생성자 - Instansitatie
    // 파괴자 - Destory
    
    //포톤 네트워크를 통해 생성하는 경우 PhotonNetwork.Instantiate("경로", position, quaternion);
    //Unity의 instasntiate는 기본적으로 첫번째 매개변수에 GameObject를 넣어주는데, 포톤의 경우 Resources를 활용하기 때문에 string으로 Resources 하위의 폴더 경로를 넣어주어야한다. 해당 프리팹이 resources 폴더 내에 있어야 한다.
    void SpawnPlayer()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
        PhotonNetwork.Instantiate("Prefabs/PlayerPrefab", spawnPosition, Quaternion.identity);
        
        
        //이 두 줄이 함축된 내용이 위의 PhotonNetwork.Instantiate ~~ 부분이다.
        // GameObject obj = Resources.Load<GameObject>("PlayerPrefab");
        // Instantiate(obj, spawnPosition, quaternion.identity);
        
        

    }
}
