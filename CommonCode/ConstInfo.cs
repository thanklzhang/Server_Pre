using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConstInfo
{
    public const string LS_IP = "127.0.0.1";
    public const int LS_Client_Port = 2345;
    public const int LS_GS_Port = 2346;


    public const string GS_IP = "127.0.0.1";
    public const int GS_Client_Port = 2347;


    public const string CS_IP = "127.0.0.1";
    public const int CS_GS_Port = 2348;

    public const string SS_IP = "127.0.0.1";
    public const int SS_Tcp_Port = 2400;

    public const int SS_Udp_Port = 2500;
    //public const int SS_Receive_Port = 2349;
    //public const int SS_Send_Port = 2350;



    public const int heartBeatInterval = 2000;

    public const int sendHeartBeatMsgId = 100;
    public const int receiveHeartBeatMsgId = 101;

}

