using DeepCore;
using DeepCore.Concurrent;
using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneServer;
using DeepCore.Game3D.Host.ZoneServer.Interface;
using DeepCore.Game3D.ZoneServer;
using DeepCore.GameData;
using DeepCore.GameData.Zone;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.IO;
using DeepCore.Log;
using DeepCore.Protocol;
using DeepCore.Reflection;
using DeepCrystal.Server;
using DeepCrystal.SSocket.SuperSocket;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeepEditor.Plugin3D.BattleServer.Host
{
    //-------------------------------------------------------------------------------------------------

    public class ServerNode : Disposable, IServerListener, IZoneNodeServer
    {
        protected readonly Logger log = LoggerFactory.GetLogger("ServerNode");
        private readonly ServerFactory mServerFactory;

        private readonly EditorTemplates mDataRoot;
        private readonly ZoneNodeConfig mConfig;
        private readonly ZoneNode mNode;
        private readonly SocketAcceptor mServer;
        private readonly int mSceneID;
        private readonly ServerCodec mCodec;
        private readonly string m_Port;
        public ServerNode(EditorTemplates dataroot, ZoneNodeConfig cfg, int sceneID, string port = null)
        {
            this.m_Port = port;

            this.mDataRoot = dataroot;
            this.mConfig = cfg;
            this.mSceneID = sceneID;

            this.mCodec = new ServerCodec(dataroot.Templates);

            this.mServerFactory = new ServerFactory();
            this.mServer = mServerFactory.CreateServer(mCodec) as SocketAcceptor;

            this.mNode = ZoneHostFactory.Factory.CreateServerZoneNode(this, dataroot, cfg);
            this.mNode.OnZoneStart += MNode_OnZoneStart;
            this.mNode.OnZoneStop += MNode_OnZoneStop;
        }

        public Task<InstanceZone> StartAsync()
        {
            var data = mNode.DataRoot.LoadScene(mSceneID, true, false);
            return this.mNode.StartAsync(data);
        }
        public Task StopAsync()
        {
            mServer.Dispose();
            return mNode.StopAsync();
        }

        protected override void Disposing()
        {
            mServer.Dispose();
            mNode.Stop();
        }

        //-------------------------------------------------------------------------------------------------

        public ZoneNodeConfig Config
        {
            get { return mConfig; }
        }
        public ZoneNode Node
        {
            get { return mNode; }
        }
        public SocketAcceptor Server
        {
            get { return mServer; }
        }
        public EditorTemplates DataRoot
        {
            get { return mDataRoot; }
        }
        public bool Running
        {
            get { return mNode.IsRunning; }
        }
        public ServerCodec Codec
        {
            get { return mCodec; }
        }

        //-------------------------------------------------------------------------------------------------

        private void InternalOpen()
        {
            try
            {
                if (!string.IsNullOrEmpty(m_Port) && int.TryParse(m_Port, out var port))
                {
                    this.mServer.Open("127.0.0.1", port, this);
                }
                else
                {
                    Random random = new Random();
                    port = random.Next(10000, 60000);
                    this.mServer.Open("127.0.0.1", port, this);
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }

        private void MNode_OnZoneStop(InstanceZone zone)
        {
        }
        private void MNode_OnZoneStart(InstanceZone zone)
        {
            new Thread(InternalOpen).Start();
        }

        //-------------------------------------------------------------------------------------------------
        #region IServerListener

        void IServerListener.OnInit(DeepCrystal.Server.IServer server)
        {
        }
        void IServerListener.OnDestory()
        {
        }
        ISessionListener IServerListener.OnSessionConnected(ISession session)
        {
            return new SessionPlayer(session as Session, mNode);
        }

        #endregion
        //-------------------------------------------------------------------------------------------------
        #region IZoneNodeServer

        void IZoneNodeServer.GameServerRpcInvoke(object msg)
        {
        }
        void IZoneNodeServer.GameServerRpcCall(object msg, Action<object, Exception> callback)
        {
        }
        void IZoneNodeServer.ListenGameServerRpcInvoke(Action<object> handler)
        {
        }
        void IZoneNodeServer.ListenGameServerRpcCall(Action<object, Action<object, Exception>> handler)
        {
        }

        #endregion
        //-------------------------------------------------------------------------------------------------




    }

    //-------------------------------------------------------------------------------------------------

    public class ServerCodec : BattleCodec
    {
        private InputStream mInputStream;
        private OutputStream mOutputStream;
        private DeepCore.IO.MemoryStream mBinBufferIn;
        private DeepCore.IO.MemoryStream mBinBufferOut;

        private HashMap<Type, Record> mTypeBytes = new HashMap<Type, Record>();

        public ServerCodec(TemplateManager templates)
            : base(templates)
        {
            this.mInputStream = new InputStream(null, Factory);
            this.mOutputStream = new OutputStream(null, Factory);
            this.mBinBufferIn = new DeepCore.IO.MemoryStream(1024);
            this.mBinBufferOut = new DeepCore.IO.MemoryStream(1024);
        }

        public override bool doDecode(Stream input, out object message)
        {
            lock (mInputStream)
            {
                mInputStream.SetStream(input);
                return base.doDecode(mInputStream, out message);
            }
        }

        public override bool doEncode(Stream output, object message)
        {
            bool ret;
            long len;
            lock (mOutputStream)
            {
                long pos = output.Position;
                mOutputStream.SetStream(output);
                ret = base.doEncode(mOutputStream, message);
                len = output.Position - pos;
            }
            if (ret)
            {
                var type = message.GetType();
                if (mTypeBytes.TryGetValue(type, out var rec))
                {
                    rec.Bytes += len;
                    rec.Count++;
                    mTypeBytes.Put(type, rec);
                }
                else
                {
                    rec.Bytes = len;
                    rec.Count = 1;
                    mTypeBytes.Put(type, rec);
                }
            }
            return ret;
        }

        public HashMap<Type, Record> GetSentTypeBytes()
        {
            lock (mOutputStream)
            {
                return new HashMap<Type, Record>(mTypeBytes);
            }
        }

        public struct Record
        {
            public long Bytes;
            public int Count;
        }
    }

    //-------------------------------------------------------------------------------------------------

    internal class SessionPlayer : IZoneNodeSession, ISessionListener
    {
        private static Logger log = LoggerFactory.GetLogger("SessionPlayer");

        private readonly ZoneNode node;
        private readonly Session session;
        private readonly BattleCodec codec;

        private readonly HashMap<string, object> mAttributes = new HashMap<string, object>();
        private Action<IMessage> mRecvHandler;

        private EnterRoomRequestC2B login_data;
        private string player_uuid;
        private string display_name;
        private int unit_template_id;
        private byte force;



        public SessionPlayer(Session session, ZoneNode node)
        {
            this.node = node;
            this.session = session;
            this.codec = new BattleCodec(node.Templates);
        }

        //----------------------------------------------------------------------------
        #region IZoneNodePlayer

        public string PlayerUUID { get { return player_uuid; } }
        public string DisplayName { get { return display_name; } }
        public ZoneNode.PlayerClient BindingPlayer { get; set; }
        private AtomicReference<PackNotify> send_queue = new AtomicReference<PackNotify>(null);
        bool IZoneNodeSession.IsAttribute(string key)
        {
            return mAttributes.ContainsKey(key);
        }
        void IZoneNodeSession.SetAttribute(string key, object value)
        {
            mAttributes.Put(key, value);
        }
        object IZoneNodeSession.GetAttribute(string key)
        {
            return mAttributes.Get(key);
        }
        void IZoneNodeSession.ClientSend(PlayerMessageEntry msg, bool immediately)
        {
            if (immediately)
            {
                session.Send(msg.message);
            }
            else
            {
                var queue = send_queue.GetOrCreate(() => { return new PackNotify(); });
                if (msg.buffer != null)
                    queue.events.Add(msg.buffer);
                else if (msg.message != null)
                    queue.events.Add(msg.message);
            }
        }
        void IZoneNodeSession.ClientFlush(BattleCodec codec)
        {
            var queue = send_queue.GetAndSet(null);
            if (queue != null)
            {
                session.Send(queue);
            }
        }
        void IZoneNodeSession.ListenClient(Action<IMessage> handler)
        {
            mRecvHandler = handler;
        }
        void IZoneNodeSession.OnPlayerConnected(ZoneNode.PlayerClient binding)
        {
        }
        void IZoneNodeSession.OnPlayerDisconnect(ZoneNode.PlayerClient binding)
        {
            mRecvHandler = null;
        }
        void IZoneNodeSession.OnPlayerDisposed()
        {
        }
        void IZoneNodeSession.GameServerRpcInvoke(object msg)
        {
        }
        void IZoneNodeSession.GameServerRpcCall(object msg, Action<object, Exception> callback)
        {
        }
        void IZoneNodeSession.ListenGameServerRpcInvoke(Action<object> handler)
        {
        }
        void IZoneNodeSession.ListenGameServerRpcCall(Action<object, Action<object, Exception>> handler)
        {
        }
        #endregion
        //----------------------------------------------------------------------------
        #region ISessionListener

        void ISessionListener.OnConnected(ISession session)
        {
            log.Info("OnConnected : " + session);
        }
        void ISessionListener.OnDisconnected(ISession session, bool force, string reason)
        {
            log.Info("OnDisconnected : " + session);
            if (node != null)
            {
                if (login_data != null)
                {
                    login_data = null;
                    node.PlayerLeave(this, (c, e) => { }, true);
                }
            }
        }
        void ISessionListener.OnError(ISession session, Exception err)
        {
        }
        void ISessionListener.OnSentMessage(ISession session, object message)
        {
        }
        void ISessionListener.OnReceivedMessage(ISession session, object message)
        {
            try
            {
                if (message is EnterRoomRequestC2B)
                {
                    do_EnterRoomResponseB2C(message as EnterRoomRequestC2B);
                }
                else if (message is LeaveRoomRequestC2B)
                {
                    do_LeaveRoomRequestC2B(message as LeaveRoomRequestC2B);
                }
                else if (mRecvHandler != null)
                {
                    mRecvHandler(message as IMessage);
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }

        #endregion


        private void do_EnterRoomResponseB2C(EnterRoomRequestC2B req)
        {
            lock (this)
            {
                this.login_data = req;
                this.player_uuid = req.PlayerUUID;
                this.display_name = req.TestToken.PlayerDisplayName;
                this.unit_template_id = req.TestToken.Data.UnitTemplateID;
                this.force = req.TestToken.Data.Force;
                var temp = node.Templates.GetUnit(unit_template_id);
                var add = new AddUnit()
                {
                    info = temp,
                    editor_name = "",
                    player_uuid = req.PlayerUUID,
                    force = force,
                    level = 0,
                    pos = null,
                    direction = 0
                };
                node.PlayerEnter(this, add, (c, e) => { }, true);
            }
        }

        private void do_LeaveRoomRequestC2B(LeaveRoomRequestC2B req)
        {
            lock (this)
            {
                if (login_data != null)
                {
                    login_data = null;
                    node.PlayerLeave(this, (c, e) => { }, false);
                    this.session.Disconnect(false);
                }
            }
        }

    }

}
