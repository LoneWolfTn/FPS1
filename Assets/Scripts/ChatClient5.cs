// attached to client game object

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class ChatClient5 : MonoBehaviour
{

    private string _ip = "127.0.0.1";
    private int _port = 9001;
    private NetworkClient _client;

    [SerializeField]
    private Text _chatText;
    [SerializeField]
    private InputField _sendTextInput;
    [SerializeField]
    private InputField _nameInput;
    void Start()
    {
        _chatText.text = string.Empty;
        _sendTextInput.text = string.Empty;
        _nameInput.text = "Client " + UnityEngine.Random.Range(0, int.MaxValue);

        Application.runInBackground = true;

        var config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);

        _client = new NetworkClient();
        _client.Configure(config, 1);

            RegisterHandlers();
        
    }


    void OnGUI()
    {


        _ip = GUI.TextField(new Rect(10, 110, 250, 30), _ip, 25);
        _port = Convert.ToInt32(GUI.TextField(new Rect(10, 140, 250, 30), _port.ToString(), 25));
        

        if (GUI.Button(new Rect(25, 170, 200, 50), " restart "))
        {
            _client.Disconnect();
            _client.Connect(_ip, _port);
        }

    }

    private void RegisterHandlers()
    {
        _client.RegisterHandler((short)MessageType.Message, OnMessageReceived);
    }

    private void OnMessageReceived(NetworkMessage message)
    {
        var mes = message.ReadMessage<ChatMessage5>().Message;
        _chatText.text = mes + "\n" + _chatText.text;
    }

    public void SendChatMessage()
    {
        var mes = new ChatMessage5();
        mes.Message = _nameInput.text + ": " + _sendTextInput.text;
        _sendTextInput.text = "";
        _client.Send((short)MessageType.Message, mes);
    }

    void OnApplicationQuit()
    {
        if (_client != null)
        {
            _client.Disconnect();
            _client.Shutdown();
            _client = null;
        }
    }
}
