using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

public class HeartBeatService
{
    public int timeout;
    Timer timer;
    DateTime lastTime;

    TcpNetConnect connect;
    public HeartBeatService( TcpNetConnect connect)
    {
        this.connect = connect;
    }

    public void ResetTimeout()
    {
        this.timeout = 0;
        lastTime = DateTime.Now;
    }

    public void SendHeartBeat(object source, ElapsedEventArgs e)
    {
        TimeSpan span = DateTime.Now - lastTime;
        timeout = (int)span.TotalMilliseconds;

        //check timeout
        if (timeout > ConstInfo.heartBeatInterval * 2)
        {
            connect.ChangeToCloseState();
         
            return;
        }

        //Send heart beat
        connect.Send(100, new byte[] { });
        //Console.WriteLine("send heart ...");
    }

    public void Start()
    {
      
        this.timer = new Timer();
        timer.Interval = ConstInfo.heartBeatInterval;
        timer.Elapsed += new ElapsedEventHandler(SendHeartBeat);
        timer.Enabled = true;

        //Set timeout
        timeout = 0;
        lastTime = DateTime.Now;
    }

    public void Stop()
    {
        if (this.timer != null)
        {
            this.timer.Enabled = false;
            this.timer.Dispose();
        }
    }
}

