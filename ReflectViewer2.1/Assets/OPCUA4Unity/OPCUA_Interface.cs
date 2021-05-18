﻿// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Events;

 namespace game4automation
{
    //! OPCUA Interface Script which is importing OPC UA Nodes and acting as the Interface towards the OPCConnectiong.
#if GAME4AUTOMATION
    [HelpURL("https://game4automation.com/documentation/current/opcua.html")]
    public class OPCUA_Interface : Game4AutomationBehavior
#else
    public class OPCUA_Interface : MonoBehaviour
#endif
    
    {
        public string Server = "opc.tcp://localhost:4840"; //!< The address of the OPC Server (default is "opc.tcp://localhost:4840")
        public bool ConnectToServer = true; //!< If false no connection to the OPC server is made
#if GAME4AUTOMATION
        [ReadOnly]public bool IsConnected;
        #else
        public bool IsConnected;
        #endif
        public string ApplicationName = "game4automation"; //!<  The application name of the OPC Client
        public string ApplicationURI = "urn::game4automation"; //!<  The appliction URi of the OPC Client
        public int SessionTimeoutMs = 60000; //!< The time in ms before session timeout
        public int OperationTimeoutMs = 0; //!< Operation Timeout - if 0 then standard value
        public string SessionName = "game4automation"; //!< The session name of the OPC Session
        public bool AutoAcceptServerCertificates = true;  //!< True if all Server certificates should be accepted
        public bool ClientAutoGenerateCertificate = true; //!<  True if the Client should automatically generate its own certificate
        public string CertificatePath; //!< The Path of the Certificates. Home is always under StreamingAssets. You can define here a sub-path under streaming assets
        public string ClientCertificatePfx;   //!< The Path of the client certificate. If emtpy it is generated automatically
        public bool UseOnlySecureSendpoints = false;  //!<  True if only secure Endpoints should be used
        public bool UseHighLevelEndpointSecurity = false;  //!<  True if only the highest level of security provided by the Server should be used
        public string UserName = ""; //!< The username - if blank anonymous user will be used
        public string Password = ""; //!< The password for the User
        public string TopNodeId;  //!< The top Node ID under which the Nodes will be imported
        public int ReconnectTime = 2000;
        public int ReconnectTimeout = 1000;
        public int MaxNumberOfNodesPerSubscription = 0; //! 0 if number of nodes is not limited
        public bool ShowFullLog = true;
        
        #if GAME4AUTOMATION 
        public bool CreateSignals = true; //! Automatically creates Game4Automation Signals
        public bool AutomaticallyPLCInputOnWriteSignals = false;
        #endif
        public bool AutomaticallySubscribeOnImport = true;   //!<  Automatically subscribes when importing new nodes
        public int SubscriptionSamplingInterval = 50;
        
        public List<string> RegexWriteSignals;  //! Regex to defines Signals which needs to be automatically defined as Wrtie to OPCUA server
        public int WatchThreadMinCycleTimeMs = 10; //!<  The minimum Cycle time of the WatchNodes Thread
#if GAME4AUTOMATION
        [ReadOnly] public string Status; //!<  The Status of the opc connection
        [ReadOnly] public int WatchThreadCycleNr; //!<  The current Cycle number of the WatchNodes Thread
        [ReadOnly] public int WatchThreadCycleMs;  //!<  The current Cycle time of the  WatchNodes Thread (might be higher than MinCommCycleMs im Communication load is heavy)
        [ReadOnly] public int WatchMaxCommTimeMs; //!<  The current max communication Time in  WatchNodes Thread since simulation startup
        [ReadOnly] public bool IsConnecting=false; //!<  true during parallel connection thread;
#else
        public string Status; //!<  The Status of the opc connection
        public int WatchThreadCycleNr; //!<  The current Cycle number of the WatchNodes Thread
        public int WatchThreadCycleMs;  //!<  The current Cycle time of the  WatchNodes Thread (might be higher than MinCommCycleMs im Communication load is heavy)
        public int WatchMaxCommTimeMs; //!<  The current max communication Time in  WatchNodes Thread since simulation startup
        public bool IsConnecting=false; //!<  true during parallel connection thread;
#endif

        public UnityEvent EventOnConnected;
        public UnityEvent EventOnDisconnected;
        public UnityEvent EventOnReconnecting;

        // private
        private string laststatus;
        private bool tryconnect = false;
        private bool isconnectingbefore = false;
        private float lastconnecttime;
        public OPCUAConnection connection;
#if GAME4AUTOMATION     
        protected new bool hidename() { return true; }
 #endif       
        //! Gets an OPCUA_Node  with the NodeID in all the Childrens of the Interface 
        public OPCUA_Node GetOPCUANode(string nodeid)
        {
            OPCUA_Node[] children = transform.GetComponentsInChildren<OPCUA_Node>();
          
            foreach (var child in children)
            {
                if (child.NodeId == nodeid)
                {
                    return child;
                }
            }
            return null;
        }

        //! Reads an OPC_UA Node and returns a new full OPCUANode class with all OPCUA Information (might be blocking if used to much)
        public OPCUANode ReadNode(string nodeid)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return new OPCUANode();
            else
            {
                return connection.ReadNode(nodeid,null);
            }
        }
        
        //! Reads an OPC_UA Node and updates the OPCUANode class with all OPCUA Information (might be blocking if used to much)
        public OPCUANode ReadNode(string nodeid, OPCUANode opcNode)
        {
            if (connection == null)
                Connect();
            if (connection == null)
            {
                opcNode.StatusBad = true;
                opcNode.StatusGood = false;
                return opcNode;
            }
            else
            {
                return connection.ReadNode(nodeid, opcNode);
            }
        }
        
        //! Reads a Node Value, might be blocking, it is usually better to subscribe to the Node
        public object ReadNodeValue(string nodeid)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return null;
            else
            {
                return connection.ReadNodeValue(nodeid);
            }
        }
        
        //! Reads a Node Value, might be blocking, it is usually better to subscribe to the Node
        //! Reads with a reference to an OPCUANode for transfer of status of the read
        public object ReadNodeValue(ref OPCUANode node)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return null;
            else
            {
                return connection.ReadNodeValue(ref node);
            }
        }
        
        //! Writes a node value and returns true if successfull, function might be blocking it is better to use Watched Nodes which are using a parallel task
        public bool WriteNodeValue(string nodeid, object value)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return false;
            else
            {
                return connection.WriteNodeValue(nodeid, value);
            }
        }
        
        //! Writes a node value and returns true if successfull, function might be blocking it is better to use Watched Nodes which are using a parallel task
        //! Updates the node status properties when writinng
        public bool WriteNodeValue(ref OPCUANode node, object value)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return false;
            else
            {
                return connection.WriteNodeValue(ref node, value);
            }
        }
        
        //! Calls the Method with NodeId methodid in the node nodeid and passes parameters - method returns object array as result
        public object[] CallMethod(string nodeid, string methodid, params object[] parameters)
        {
            if (connection == null)
                Connect();
            if (connection == null)
                return null;
            else
            {
                return connection.CallMethod(nodeid,methodid, parameters);
            }
        }
        
        //! Adds a Node to the Watched Nodes List.Watched Nodes are sending in a parallel Task new Node Value data if Node.Value is changed
        public OPCUANode AddWatchedNode(string nodeid)
        {
            if (connection != null)
                return connection.AddWatchedNode(nodeid);
            else
            {
                return null;
            }
        }

        //! Subscribes to an OPCUA node, delegate function gets called when node value is updated on OPCUA server
        public OPCUANodeSubscription Subscribe(string nodeid, NodeUpdateDelegate del)
        {
            OPCUANodeSubscription sub;
            sub = connection.SubscribeNodeDataChange(nodeid,del,SubscriptionSamplingInterval);
           
            if (sub==null)
                Debug.LogError("NodeID " +  nodeid + "Subscription returns null");
            return sub;
        }

        //! Gets al Subnodes of topnode nodeid including all childrens
        public List<OPCUANode> GetAllSubnodes(string nodeid)
        {
            List<OPCUANode> res = new List<OPCUANode>();
           // if (connection == null)
                Connect();
            if (connection != null)
            {
                res = connection.GetAllSubNodes(TopNodeId);
            }

            return res;
        }
        
        //! Gets al Subnodes of topnode nodeid only for one level
        public List<OPCUANode> GetSubnodes(string nodeid)
        {
            List<OPCUANode> res = new List<OPCUANode>();
            if (connection == null)
                Connect();
            if (connection != null)
            {
                res = connection.GetSubNodes(TopNodeId);
            }

            return res;
        }
        
        //! Imports all OPCUANodes under TopNodeId and creates GameObjects.
        //! If GameObject with NodeID is already existing the GameObject will be updated.
        //! Does not deletes any Nodes. If Game4Automation Framework is existent (Compiler Switch GAME4AUTOMATION) also Game4Automation
        //! PLCInputs and PLCOutputs are created or updated or all nodes with suitable data types.
        public void ImportNodes()
        {
            var nodes = GetAllSubnodes(TopNodeId);        
            CreateNodes(nodes,gameObject);       
            OPCUA_Node[] opcuanodes = GetComponentsInChildren<OPCUA_Node>();
            
#if GAME4AUTOMATION
            foreach (var node in opcuanodes)
            {
                var IsWrite = false;
                // Check Regex
                if (RegexWriteSignals != null)
                {
                  
                    if (RegexWriteSignals.Count > 0)
                    {
                        foreach (var regexstring in RegexWriteSignals)
                        {
                            Regex regex = new Regex(regexstring);
                            if (regex.IsMatch(node.Name))
                            {
                                IsWrite = true;
                            }
                        }
                    }
                }

                if (AutomaticallyPLCInputOnWriteSignals)
                {
                    if (node.AccessLevel == "CurrentReadOrWrite")
                    {
                        IsWrite = true;
                    }
                }

                if (IsWrite)
                {
                    node.ReadValue = false;
                    node.WriteValue = true;
                    node.SubscribeValue = false;
                }
                if (CreateSignals)
                    node.UpdatePLCSignal();
                SetNodeSubscriptionParameters(node, true);
                node.Awake();
            }
#endif
            Debug.Log("All nodes imported from " + Server + " " + TopNodeId);
        
        }

        public void ManualImportNodes()
        {
            Connect();
            ImportNodes();
            Disconnect();
        }
        
        //! Sames as ImportNodes but it does not import the nodes from the OPCUA server, it just updates the nodes which are existing under the Interface based on the settings.
        //! If no Game4Automation Signals are existing then the signals are automatically created including needed subscriptions.
#if GAME4AUTOMATION
        public void UpdateNodes()
        {
            OPCUA_Node[] opcuanodes = GetComponentsInChildren<OPCUA_Node>();
            foreach (var node in opcuanodes)
            {        
                var IsWrite = false;
                // Check Regex
                if (RegexWriteSignals != null)
                {
                  
                    if (RegexWriteSignals.Count > 0)
                    {
                        foreach (var regexstring in RegexWriteSignals)
                        {
                            Regex regex = new Regex(regexstring);
                            if (regex.IsMatch(node.Name))
                            {
                                IsWrite = true;
                            }
                        }
                    }
                }

                if (IsWrite)
                {
                    node.ReadValue = false;
                    node.WriteValue = true;
                }
                
                node.UpdatePLCSignal();
                SetNodeSubscriptionParameters(node,false);
            }
            Debug.Log("All nodes updated!");
        }
#endif
        
        private string GetString(string data)
        {
            var guidData = System.Convert.FromBase64String(data);

            var guid = new System.Guid(guidData);
            return guid.ToString("B").ToUpper();
        }
        
        
        //! Connects to the OPCUA server
        public void Connect()
        {
            lastconnecttime = Time.time;
            // If already connected first disconnect
            if (connection == null)
            {
                connection = new OPCUAConnection();
                         connection.Server = Server;
            connection.ApplicationName = ApplicationName;
            connection.ApplicationURI = ApplicationURI;
            connection.SessionTimeoutMs = SessionTimeoutMs;
            connection.SessionName = SessionName;
            connection.AutoAcceptServerCertificates = AutoAcceptServerCertificates;
            connection.ClientAutoGenerateCertificate = ClientAutoGenerateCertificate;
            connection.UseOnlySecureSendpoints = UseOnlySecureSendpoints;
            connection.UseHighLevelEndpointSecurity = UseHighLevelEndpointSecurity;
            connection.UserName = UserName;
            connection.Password = Password;
            connection.MinCommCycleMs = WatchThreadMinCycleTimeMs;
            connection.ClientCertificatePfx = ClientCertificatePfx;
            connection.SubscriptionSamplingInterval = SubscriptionSamplingInterval;
            connection.ReconnectTimeout = ReconnectTimeout;
            connection.OperationTimeout = OperationTimeoutMs;
            connection.MaxNumberOfNodesPerSubscription = MaxNumberOfNodesPerSubscription;
            connection.ShowFullLog = ShowFullLog;
            #if !UNITY_IOS && !UNITY_ANDROID
                  connection.CertifcatePath = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath,  CertificatePath));
                  if (connection.ClientCertificatePfx!="")
                     connection.ClientCertificatePfx = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, ClientCertificatePfx));
                 
            #endif
            #if UNITY_IOS || UNITY_ANDROID
                    connection.CertifcatePath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, CertificatePath));

                if (connection.ClientCertificatePfx!="")
                     connection.ClientCertificatePfx = Path.GetFullPath(Path.Combine(Application.persistentDataPath, ClientCertificatePfx));
            #endif
            IsConnecting = true;
            isconnectingbefore = true;

            connection.OnConnecting += OnConnecting;
            connection.OnConnected += OnConnnected;
            connection.OnReconnecting += OnReconnecting;
            connection.OnReconnected += OnReconnected;
            connection.OnDisconnecting += OnDisconnecting;
            connection.OnDisconnected += OnDisconnected;
      
#if !UNITY_IOS && !UNITY_ANDROID
            Debug.Log(
                "OPCUA4Unity - System pathes are Common Application Data: [" +
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) +
                "] Application Data: [" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)+"]");
           #endif
            }
            connection.Connect();
        }

        private void Connected()
        {
            var nodes = GetComponentsInChildren<OPCUA_Node>();
                foreach (var node in nodes)
                {
                    node.OnConnected();
                }
#if GAME4AUTOMATION        
                if (Game4AutomationController!=null)
                    Game4AutomationController.OnConnectionOpened(this.gameObject);
#endif
             
                tryconnect = false;
          /*
                Debug.Log($"Not Connected to OPC Server {Server} with Status {connection.GetStatus()} trying to connect again in {ConnectTimeout/1000}s.");
                tryconnect = true; */
            
        } 

        
        private void OnConnecting()
        {
            IsConnected = false;
            Debug.Log("OPCUA - Connecting to " + Server);
            Status = "Connecting";
        }
        
        private void OnConnnected()
        {
            IsConnected = true;
            Debug.Log("OPCUA - Connected to " + Server);
            Status = "Connected";
        }

        
        private void OnReconnecting()
        {
            IsConnected = false;
            Debug.Log("OPCUA - Reconnecting to " + Server);
            Status = "Reconnecting";

        }

        private void OnReconnected()
        {
            IsConnected = true;
            if (ShowFullLog) Debug.Log("OPCUA - Reconnected to " + Server);
            Status = "Connected";
        
        }
        
        private void OnDisconnecting()
        {
            IsConnected = false;
            Debug.Log("OPCUA - Disconnecting from " + Server);
            Status = "Disconnecting";
        }
        
        private void OnDisconnected()
        {
            IsConnected = false;
            Debug.Log("OPCUA - Disconnected from " + Server);
            Status = "Disconnected";
        }

        
        private void UpdateStatus()
        {
            
            if (connection.threadconnected==false)
                if (Status != "Connected")
                {
                    if (connection.ConnectException != null)
                    {
                        Debug.Log("OPCUA - " + connection.ConnectException.Message);
                        connection.ConnectException = null;
                        tryconnect = true;
                        Status = "Waiting for new connect";
                    }
                }
            
            if (Status == laststatus)
                return;
            
            if (Status == "Connected")
            {
                IsConnected = true;
                #if GAME4AUTOMATION
                var signals = GetComponentsInChildren<Signal>();
                foreach (var signal in signals)
                {
                    signal.SetStatusConnected(true);
                }
                #endif
                if (ShowFullLog) Debug.Log("OPCUA - Status Connected " + Server);
                Connected();
                EventOnConnected.Invoke();
            }

      

            if (Status != "Connected" && laststatus == "Connected")
            {
#if GAME4AUTOMATION        
                if (Game4AutomationController!=null)
                    Game4AutomationController.OnConnectionClosed(this.gameObject);
#endif
                if (ShowFullLog) Debug.Log("OPCUA - Status Disconnected " + Server);
                EventOnDisconnected.Invoke();
            }

            if (Status == "Reconnecting")
            {
                if (ShowFullLog) Debug.Log("OPCUA - Status Reconnecting " + Server);
                EventOnReconnecting.Invoke();
            }

            laststatus = Status;
        }
        
        //! Disconnects from the  OPCUA server
        public void Disconnect()
        {
#if GAME4AUTOMATION
            if (Game4AutomationController!=null)
               Game4AutomationController.OnConnectionClosed(this.gameObject);
#endif
            var nodes = GetComponentsInChildren<OPCUA_Node>();
            foreach (var node in nodes)
            {
                    node.OnDisconnected();
            }

            if (connection != null)
            {
                connection.Disconnect();
                connection = null;
                Debug.Log("Disconnected from OPC Client" + Server);
            }
  
            EventOnDisconnected.Invoke();
        }

        private void UpdateInfo(OPCUA_Node info, OPCUANode opcNode)
        {
           info.UpdateNode(opcNode);
           
           
        }

        private void SetNodeSubscriptionParameters(OPCUA_Node node, Boolean newcreated)
        {
        
            if (node.UserAccessLevel == "CurrentRead" || node.UserAccessLevel == "CurrentReadOrWrite")
            {
                if (newcreated)
                {
                    node.ReadValue = true;
                    node.WriteValue = false;
                }

                node.SubscribeValue = AutomaticallySubscribeOnImport;
            }
            if (node.UserAccessLevel == "CurrentWrite")
            {
                node.ReadValue = false;
                node.WriteValue = true;
                node.SubscribeValue = false;
            }
        }
        
        private void CreateNodes(List<OPCUANode> nodes, GameObject topobject)
        {
            GameObject newnode;
            foreach (OPCUANode node in nodes)
            {
                OPCUA_Node info = GetOPCUANode(node.NodeId);
                // If not create node
                if (info == null)
                {
                    newnode = new GameObject(node.Name);
                    newnode.transform.parent = topobject.transform;
                    info = newnode.AddComponent<OPCUA_Node>();
                    UpdateInfo(info,node);
                    info.NodeId = node.NodeId;
                    info.Interface = this;
                    SetNodeSubscriptionParameters(info,true);
                }
                else
                {
                    newnode = info.gameObject;
                    UpdateInfo(info,node);
                    SetNodeSubscriptionParameters(info,false);
                }

 
                // if subnodes then create them
                if (node.SubNodes.Count > 0)
                {
                  CreateNodes(node.SubNodes,newnode);
                }
            }
        }
        
        private  void OnEnable()
        {
            if (ConnectToServer)
                 Connect();
        }

        private void OnDisable()
        {
            Disconnect();
   
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }
        
        private void Update()
        {
            if (connection != null)
            {
                UpdateStatus(); // Check Status in Mainthread and call events
                WatchThreadCycleMs = connection.CommCycleMs;
                WatchThreadCycleNr = connection.CommCycleNr;
                WatchMaxCommTimeMs = connection.MaxCommTimeMs;
                if (connection.ConnectException != null)
                {
                    Debug.Log("OPCUA - " + connection.ConnectException.Message);
                }
            }

            
        
                
            if (tryconnect)
            {
                var deltatime = Time.time - lastconnecttime;
                if (deltatime>ReconnectTime/1000)
                    Connect();
            }

        }
    }
}