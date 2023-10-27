using UnityEngine;
using System;
using System.Collections.Generic;

namespace QualisysRealTime.Unity
{
    public class Connection : MonoBehaviour
    {
        /// <summary>Hostname of server</summary>
        public string HostName = "Localhost";
        bool stream6d = true;
        bool stream3d = true;
        bool stream3dNoLabels = false;
        bool streamGaze = true;
        bool streamAnalog = false;
        bool streamSkeleton = true;
        public string Protocol;
        public string ConnectionStatus;
        public int frameNumber;

        private QTMRealTimeSDK.DiscoveryResponse server;

        void Awake()
        {
            List<QTMRealTimeSDK.DiscoveryResponse> discoveryResponses = RTClient.GetInstance().GetServers();
            server = Array.Find(discoveryResponses.ToArray(), r => r.HostName == HostName);
            RTClient.GetInstance().StartConnecting(server.IpAddress, -1, stream6d, stream3d, stream3dNoLabels, streamGaze, streamAnalog, streamSkeleton);
        }

        void OnApplicationQuit()
        {
            RTClient.GetInstance().Disconnect();
        }

        void FixedUpdate()
        {
            Protocol = RTClient.GetInstance().RtProtocolVersion.ToString();
            ConnectionStatus = RTClient.GetInstance().ConnectionState.ToString();
            frameNumber = RTClient.GetInstance().GetFrame();
        }
    }
}
