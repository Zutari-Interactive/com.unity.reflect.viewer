// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  
using UnityEngine;

namespace game4automation
{
    public class DemoWriteNodeToServer : MonoBehaviour
    {
        public float Speed;
        public OPCUA_Interface Interface;
        public string NodeId;
        public float Position;
        private OPCUANode node;

        // Start is called before the first frame update
        void Start()
        {
            node = Interface.AddWatchedNode(NodeId); // Register node as watched node
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.left, Speed * Time.deltaTime);
            Position = transform.rotation.eulerAngles.x; // Just for displaying it
            node.Value =
                (int) transform.rotation.eulerAngles
                    .x; // sets the new node value - because it is watched value will be automatically transfered to OPC Server
        }
    }
}
