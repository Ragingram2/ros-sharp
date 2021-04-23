/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace RosSharp.RosBridgeClient.Protocols
{
    public class WebSocketSharpProtocol: IProtocol
    {
        public event EventHandler OnReceive;
        public event EventHandler OnConnected;
        public event EventHandler OnClosed;

        private WebSocket WebSocket;
        private List<string> console = new List<string>();

        public WebSocketSharpProtocol(string url)
        {
            WebSocket = new WebSocket(url);
            WebSocket.OnMessage += Receive;

            WebSocket.OnClose += Closed;
            WebSocket.OnOpen += Connected;
        }
                
        public void Connect()
        {
            console.Add("[WebSocketSharpProtocol] Connected");
            WebSocket.ConnectAsync();            
        }

        public void Close()
        {
            console.Add("[WebSocketSharpProtocol] Disconnected");
            WebSocket.CloseAsync();
        }

        public bool IsAlive()
        {
            return WebSocket.IsAlive;
        }

        public void Send(byte[] data)
        { 
            console.Add("[WebSocketSharpProtocol] Sending: "+BitConverter.ToString(data));
            WebSocket.SendAsync(data, null);
        }

        public List<string> getConsole()
        {
            return console;
        }
        
        private void Receive(object sender, WebSocketSharp.MessageEventArgs e)
        {
            console.Add("[WebSocketSharpProtocol] Recived in Socket");
            OnReceive?.Invoke(sender, new MessageEventArgs(e.RawData));
        }

        private void Closed(object sender, EventArgs e)
        {
            OnClosed?.Invoke(sender, e);
        }

        private void Connected(object sender, EventArgs e)
        {
            OnConnected?.Invoke(sender, e);
        }
    }
}
