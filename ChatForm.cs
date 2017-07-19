using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using NetworkLibrary;

namespace MultiChatServer {
    public partial class ChatForm : Form {
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;
        Socket mainSock;
        IPAddress thisAddress;

        private int _receiveSize = 0;
        private int _packetSize = 0;

        public ChatForm() {
            InitializeComponent();

            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _textAppender = new AppendTextDelegate(AppendText);
        }

        void AppendText(Control ctrl, string s) {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else {
                string source = ctrl.Text;
                ctrl.Text = s;//source + Environment.NewLine + s;
            }
        }

        void OnFormLoaded(object sender, EventArgs e) {
            IPHostEntry he = Dns.GetHostEntry(Dns.GetHostName());

            // 처음으로 발견되는 ipv4 주소를 사용한다.
            foreach (IPAddress addr in he.AddressList) {
                if (addr.AddressFamily == AddressFamily.InterNetwork) {
                    thisAddress = addr;
                    break;
                }    
            }

            // 주소가 없다면..
            if (thisAddress == null)
                // 로컬호스트 주소를 사용한다.
                thisAddress = IPAddress.Loopback;

            txtAddress.Text = thisAddress.ToString();
        }
        void BeginStartServer(object sender, EventArgs e) {
            int port;
            if (!int.TryParse(txtPort.Text, out port)) {
                MsgBoxHelper.Error("포트 번호가 잘못 입력되었거나 입력되지 않았습니다.");
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }

            // 서버에서 클라이언트의 연결 요청을 대기하기 위해
            // 소켓을 열어둔다.
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port);
            mainSock.Bind(serverEP);
            mainSock.Listen(10);

            // 비동기적으로 클라이언트의 연결 요청을 받는다.
            mainSock.BeginAccept(AcceptCallback, null);

            AppendText(txtHistory, "서버 시작 됐다");
        }

        List<Socket> connectedClients = new List<Socket>();
        void AcceptCallback(IAsyncResult ar) {
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = mainSock.EndAccept(ar);

            // 또 다른 클라이언트의 연결을 대기한다.
            mainSock.BeginAccept(AcceptCallback, null);

            AsyncObject obj = new AsyncObject(20);
            obj.WorkingSocket = client;

            // 연결된 클라이언트 리스트에 추가해준다.
            connectedClients.Add(client);

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            AppendText(txtHistory, string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));

            // 클라이언트의 데이터를 받는다.

            while (true)
            {
                if (client.BeginReceive(obj.Buffer, 0, 20, 0, DataReceived, obj).IsCompleted)
                {
                    
                }
            }
            //bool result = client.BeginReceive(obj.Buffer, 0, 20, 0, DataReceived, obj).IsCompleted;

            //Console.Write(result);


        }

        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            

            //byte[] buff = obj.Buffer;
            if(_packetSize == 0) // 아직 받야아할 패킷 사이즈가 판단이 안된 경우
            {
                try
                {
                    int len = obj.WorkingSocket.EndReceive(ar);



                    if(len == 0)
                    {
                        obj.WorkingSocket.Close();
                        return;
                    }
                    else if(len != 53)
                    {
                        Console.Write(len);
                    }
                    _receiveSize += len;
                }
                catch(Exception ex)
                {
                    
                }
                if (_receiveSize == 0)
                {
                    obj.WorkingSocket.Close();
                    return;
                }
                else if (_receiveSize < 2)
                {
                    try
                    {
                        //obj.WorkingSocket.BeginReceive(obj.Buffer, _receiveSize, (obj.Buffer.Length - _receiveSize), SocketFlags.None, new AsyncCallback(DataReceived), obj);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    _packetSize = (int)Util.ByteArrToShort(obj.Buffer, 0); // 패킷 사이즈를 구하는 코드
                    if (_packetSize <= 0)
                    {
                        obj.WorkingSocket.Close();
                        return;
                    }

                    if(obj.Buffer.Length >= _packetSize) // 이미 한번에 다 받았다면
                    {
                        ProcessPacket(obj);
                    }
                    else
                    {
                        _receiveSize = 0;
                        AsyncObject newObj = new AsyncObject(_packetSize);
                        newObj.WorkingSocket = obj.WorkingSocket;
                        try
                        {
                            //newObj.WorkingSocket.BeginReceive(newObj.Buffer, 0, _packetSize, SocketFlags.None, new AsyncCallback(DataReceived), newObj);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            else
            {
                try
                {
                    int len = obj.WorkingSocket.EndReceive(ar);
                    if(len == 0)
                    {
                        obj.WorkingSocket.Close();
                        return;
                    }
                    _receiveSize += len;
                }
                catch(Exception ex)
                {

                }
                if (_receiveSize < _packetSize)
                {
                    try
                    {
                        //obj.WorkingSocket.BeginReceive(obj.Buffer, _receiveSize, (obj.Buffer.Length - _receiveSize), SocketFlags.None, new AsyncCallback(DataReceived), obj);
                    }
                    catch(Exception ex)
                    {

                    }
                }
                else
                {
                    ProcessPacket(obj);
                }
            }
        }

        private void ProcessPacket(AsyncObject obj)
        {
            PacketUserInfo user = new PacketUserInfo(1000);
            user.ToType(obj.Buffer);
            string text = user.ToString();

            AppendText(txtHistory, text);  //string.Format("[받음]{0}: {1}", ip, msg));

            // for을 통해 "역순"으로 클라이언트에게 데이터를 보낸다.
            for (int i = connectedClients.Count - 1; i >= 0; i--)
            {
                Socket socket = connectedClients[i];
                //if (socket != obj.WorkingSocket) //@TODO: 일단 같은 소켓이라도 보내도록 하자
                //{
                try { socket.Send(obj.Buffer); }
                catch
                {
                    // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                    try { socket.Dispose(); } catch { }
                    connectedClients.RemoveAt(i);
                }
                //}
            }

            // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
            obj.ClearBuffer();

            // 수신 대기
            obj.WorkingSocket.BeginReceive(obj.Buffer, 0, obj.Buffer.Length, 0, DataReceived, obj);
            _packetSize = 0;
        }

        void OnSendData(object sender, EventArgs e) {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound) {
                MsgBoxHelper.Warn("서버가 실행되고 있지 않습니다!");
                return;
            }
            
            // 보낼 텍스트
            string tts = txtTTS.Text.Trim();
            if (string.IsNullOrEmpty(tts)) {
                MsgBoxHelper.Warn("텍스트가 입력되지 않았습니다!");
                txtTTS.Focus();
                return;
            }
            
            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(thisAddress.ToString() + '\x01' + tts);

            // 연결된 모든 클라이언트에게 전송한다.
            for (int i = connectedClients.Count - 1; i >= 0; i--) {
                Socket socket = connectedClients[i];
                try { socket.Send(bDts); } catch {
                    // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                    try { socket.Dispose(); } catch { }
                    connectedClients.RemoveAt(i);
                }
            }

            // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
            AppendText(txtHistory, string.Format("[보냄]{0}: {1}", thisAddress.ToString(), tts));
            txtTTS.Clear();
        }


        private bool IsGetPacketSize(byte[] buff)
        {
            if (buff.Length < 2)
            {
                return false;
            }

            return true;
        }

        private int GetPacketTotalSize(byte[] buff)
        {
            int result = 0;

            if (IsGetPacketSize(buff))
            {
                byte[] size = new byte[2];
                size[0] = buff[0];
                size[1] = buff[1];
                result = (int)Util.ByteArrToShort(size, 0);
            }
            return result;
        }

        private int GetPacketType(byte[] buff)
        {
            int result = 0;
            int offset = 2; //앞에 바이트길이 헤더 2바이트

            result = Util.ByteArrToInt(buff, offset);
            
            return result;
        }
    }
}