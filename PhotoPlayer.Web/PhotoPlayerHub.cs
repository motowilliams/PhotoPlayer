using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PhotoPlayer.Web
{
    public class PhotoPlayerHub : Hub
    {
        public void Send(string photo)
        {
            Clients.All.send(photo);
        }
    }
}