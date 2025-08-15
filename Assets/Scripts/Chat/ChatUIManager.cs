using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    //------ 싱글톤 -------//
    private static ChatUIManager _instance;
    public static ChatUIManager Instance => _instance;

    
    public RectTransform content;
    public ScrollRect scrollRect;
    public InputField chatInputField;
    public Text chatLogText;
    public int MaxMessages;
    private List<string> chatMessages = new List<string>();
   
    
    #region Initializae
    private void Awake() => Init();
    
    private void Init()
    {
        SingletonInit();
       
    }

    private void SingletonInit()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
    #endregion
    
    
    private void Update()
    {
        
        //선택된 UI가 채팅 입력 창인 경우에만 엔터키를 눌렀을 때 메시지를 보내도록 처리
        if(EventSystem.current.currentSelectedGameObject == chatInputField.gameObject &&
           Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return = Enter
        {
            SendChatMessage();
        }
    }

    
    //input field의 메시지를 ChatManager를 통해 채팅 서버에 보내기
    private void SendChatMessage()
    {
        string message = chatInputField.text;
        if(!string.IsNullOrEmpty(message))
        {
            ChatManager.Instance.SendMessageToChat(message);

            chatInputField.text = "";

            chatInputField.ActivateInputField();  //input field에 focus를 맞추도록!
        }
    }
    

    public void DisplayMessage(string Message)
    {
        chatMessages.Add(Message);

        
        //최대 메시지 개수가 넘어가면 앞에서부터 삭제
        if(chatMessages.Count > MaxMessages)
        {
            chatMessages.RemoveAt(0);
        }

        scrollRect.verticalNormalizedPosition = 0.0f; // 스크롤을 항상 아래로 맞추도록 처리  
        UpdateChatLog(); // UI 반영
    }

    
    private void UpdateChatLog()
    {
        chatLogText.text = string.Join("\n", chatMessages);
        content.sizeDelta = new Vector2(content.sizeDelta.x, chatLogText.GetComponent<RectTransform>().sizeDelta.y + 100.0f);
    }
}
