using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChatManager : MonoBehaviour, IChatClientListener
{
  
    //------ 싱글톤 -------//
    private static ChatManager _instance;
    public static ChatManager Instance => _instance;
    
    
    private ChatClient _chatClient;
    private string _chatChannel = "GlobalChannel";   //채팅 채널


    #region Initializae
    private void Awake() => Init();
    
    private void Init()
    {
        SingletonInit();
        ChatClientInit();
    }

    private void SingletonInit()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    private void ChatClientInit()
    {
        //chat client 생성 및 초기화
        _chatClient = new(this);
        
        //chat client와 server를 연결
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.AppVersion,
            new AuthenticationValues(PhotonNetwork.NickName));
    }
    

    #endregion


    private void Update()
    {
        _chatClient?.Service();
    }


    public void SendMessageToChat(string message)
    {
        //입력한 message가 null이 아닌 경우에만
        if (!string.IsNullOrEmpty(message))
        {
            _chatClient.PublishMessage(_chatChannel, $"{PhotonNetwork.NickName} : {message}"); 
            
        }
    }

    #region ChatClient_Interface

    //PHoton.Chat 클라이언트에서 발생하는 디버깅 메시지를 처리한다.
    //level : error, warning, info
    //message : 디버깅 메세지
    public void DebugReturn(DebugLevel level, string message)
    {
        switch(level)
        {
            case DebugLevel.ERROR:
                Debug.LogError($"Photon Chat Error: {message}");
                break;
            case DebugLevel.WARNING:
                Debug.LogWarning($"Photon Chat Warning: {message}");
                break;
            default:
                Debug.Log($"Photon Chat : {message}");
                break;
        }
    }

    
    //Photon.Chat 서버와 연결이 끊어졌을 때 호출된다.
    public void OnDisconnected()
    {
        Debug.Log("Photon Disconnected!");
    }
    
    // PHoton.Chat 서버와 연결이 되었을 때 호출된다.
    public void OnConnected()
    {
        Debug.Log("Photon Connected!");

        _chatClient.Subscribe(new string[] { _chatChannel });
    }

    // Photon.Chat 클라이언트의 상태가 변경될 때 호출된다.
    // 특정 유저가 채팅을 보냈을 때 그 상태를 모니터링하고 상태에 따라서 UI를 업데이트 한더던지, 특정 캐릭터 위로 말풍선을 띄우는 작업을 할 때
    // 본인의 캐릭터가 아니더라도 타 유저가 채팅을 입력했을 때 타 유저의 머리 위쪽으로 채팅 UI를 띄워주는 그런 식의 작업이 가능하다.
    // 매개변수 : state(ChatState 열거형 값(Enum), 클라이언트의 현재 상태(Connected, Connecting, Disconnected 등) 즉, 현재 클라이언트의 상태가 connected 일 때만, 클라이언트가 내 화면에 노출되어 있으면, 머리 위쪽으로 UI를 올려주거나 메시지를 송수진 할 때 사용한다.
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Chat State Changed: {state}");
    
        // ConnectedToNameServer : 네임 서버와의 연결이 완료된 상태
        // Authenticated : 인증이 완료되어 채팅 서버와 연결이 준비가 된 상태
        // Disconnected : 연결이 끊긴 상태
        // ConnectedToFrontEnd : Front-End 서버와 연결된 상태
    
        switch(state)
        {
            case ChatState.ConnectedToNameServer:
                Debug.Log("Connected to Name Server");
                break;
            case ChatState.Authenticated:
                Debug.Log("Authenticated successfully.");
                break;
            case ChatState.Disconnected:
                Debug.LogWarning("Disconnected from Chat Server");
                break;
            case ChatState.ConnectingToFrontEnd:
                Debug.Log("Connected to Front End Server");
                break;
            default:
                Debug.Log($"Unhandled Chat State: {state}");
                break;
        }
    }

    
    // 특정 채널에서 메시지를 수신했을 때 호출된다.
    //월드 채널, 파티 채널 등.. 여러 개의 채팅 채널이 존재할 때, 각각의 채널에서 어떤 메세지가 수신됐을 때 호출된다.
    // channelName : 메시지가 수신된 채널 이름
    // senders : 메시지를 보낸 사용자 이름 배열
    // messages : 수신된 메시지 배열
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    // 다른 플레이어가 보낸 개인 메시지를 수신했을 때 호출된다.
    // 귓속말 같은 시스템, 개인 채팅
    // sender : 메시지를 보낸 사용자 이름
    // message : 메시지 내용
    // channelName : 메시지가 속한 채널 이름 (포톤에서는 개인 메시지도 채널 네임이 포함되서 전달된다)
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }


    // 채널 구독 요청이 성공적으로 처리되었을 때 호출된다.
    // channels : 구독ㄷ한 채널 이름 배열
    //results : 각 채널의 구독 성공 여부(true, false)
    // 특정 캐릭터가 길드 시스템에 가입하면 그 길드 채널을 사용할 수 있게 된다. 길드 채널에 들어오면 길드 채널 내부에서 채팅을 할 수 있어야한다. 이때 유저가 이 길드 채널을 구독시키는 방법으로 사용할 수 있다.
    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    //채널 구독 해제 요청이 처리되었을 때 호출된다.
    // 길드에서 탈퇴했을 때 해당 길드 채널에 대한 구독 해지 등에 사용
    // channels : 구독 해제된 채널 이름 배열
    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 특정 사용사의 상태가 변경되었을 때 호출된다.
    /// </summary>
    /// <param name="user">상태가 변경된 사용자</param>
    /// <param name="status">새로운 상태 코드 (온라인, 오프라인, 자리 비움 등)</param>
    /// <param name="gotMessage">상태 변경시 추가 메시지 여부</param>
    /// <param name="message">상태 변경과 함께 전달된 메시지</param>
    /// 친구 목록에서 친구가 온라인인지 오프라인인지를 확인할 수 있는 기능을 만들 수 있는 함수?
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    
    //특정 사용자가 채널에 구독했을 때 호출된다.
    // channel : 사용자가 구독한 채널 이름
    // user : 구독한 사용자 이름
    // 위의 OnSubscrbe와 다르게 '유저 정보'를 받아올 수 있다.
    //길드 가입시 길드원이 가입했다고 메시지를 알려야한다. 이 때 이 메서드를 사용할 수 있다.
    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    //특정 사용자가 채널 구독을 해제했을 때 호출된다.
    // channel : 사용자가 구독 해제한 채널 이름
    // user : 구독 해제한 사용자 이름
    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
 
    #endregion
 
}
