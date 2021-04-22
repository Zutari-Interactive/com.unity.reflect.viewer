// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using UnityEngine;

namespace game4automation
{
    public class DemoCallMethod : MonoBehaviour
    {
        public OPCUA_Interface Interface;
        public string NodeId;
        public string MethodId;
        public double MethodParameterA;
        public double MethodParameterB;
        public double Result;
        private double _a, _b;

        void Update()
        {
            // if parameter changed
            if (_a != MethodParameterA || _b != MethodParameterB)
            {
                var res = Interface.CallMethod(NodeId, MethodId, MethodParameterA,
                    MethodParameterB); // calls a method with NodeID MethodID in Node NodeId with Parameters MethodParameterA and MethodParameterB
                if (res!=null) // Method call was successfull
                       Result = (double) res[0]; // The result is returned as an object array
            }

            _a = MethodParameterA;
            _b = MethodParameterB;
        }
    }
}
