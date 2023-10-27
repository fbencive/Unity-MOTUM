// Unity SDK for Qualisys Track Manager. Copyright 2015-2018 Qualisys AB
//
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace QualisysRealTime.Unity
{
    public class AvatarAnimation : MonoBehaviour
    {
        [Header("QTM settings")]
        public string SkeletonName = "Put QTM skeleton name here";
        public uint CalibrationCount = 180;
        public Glove5dt.GloveComponent glove;
        public float fingerSmoothing = 100.0f;

        private string RigSegmentPrefix = "VF:";
        private Dictionary<uint, GameObject> mQtmSegmentIdToGameObject = new Dictionary<uint, GameObject>();
        private Dictionary<string, GameObject> mSegmentNameToGameObject = new Dictionary<string, GameObject>();
        private Dictionary<uint, TransformAverage> mQtmSegmentIdToTransform = new Dictionary<uint, TransformAverage>();
        private Skeleton mQtmSkeletonCache = null;
        private bool calibrating = false;
        private uint calibrationIndex = 0;
        private int numFrame = 0;

        private Transform[] boneTransform = new Transform[15];
        private Quaternion[] boneQuaternion = new Quaternion[15];


        private void Awake()
        {
            SetupGlove();

            Stack<Transform> s = new Stack<Transform>();
            s.Push(transform);
            while (s.Count > 0)
            {
                var t = s.Pop();
                string name = t.gameObject.name;

                try
                {
                    if (string.IsNullOrEmpty(RigSegmentPrefix))
                    {
                        mSegmentNameToGameObject.Add(name, t.gameObject);
                    }
                    else if (name.StartsWith(RigSegmentPrefix))
                    {
                        mSegmentNameToGameObject.Add(name.Replace(RigSegmentPrefix, ""), t.gameObject); //remove the prefix from the GO names
                    }
                }
                catch (System.ArgumentException e)
                {
                    Debug.LogWarning("Failed to add " + name + " exception " + e.ToString());
                }

                foreach (Transform child in t)
                {
                    s.Push(child);
                }
            }
        }

        void Update()
        {
            AnimateAvatarFromQtm();
            if (glove != null)
                AnimateFingersFromGlove();
        }

        private void AnimateAvatarFromQtm()
        {
            int N = RTClient.GetInstance().GetFrame(); //if frame = 0, it means that QTM is connected (opened), but is not streaming RT data (there is no measurement ongoing - no play with RT output)
            if (N == numFrame)
                return;
            numFrame = N;

            // Get the skeleton from QTm
            var skeleton = RTClient.GetInstance().GetSkeleton(SkeletonName);
            if (mQtmSkeletonCache != skeleton)
                StartNewSkeleton(skeleton);

            if (mQtmSkeletonCache == null)
                return;
            if (calibrating)
                Calibrate();
            else
            {
                // Update all the game objects
                GameObject gameObject;
                TransformAverage t;
                foreach (var segment in mQtmSkeletonCache.Segments)
                {
                    if (mQtmSegmentIdToGameObject.TryGetValue(segment.Key, out gameObject))
                    {
                        if (mQtmSegmentIdToTransform.TryGetValue(segment.Key, out t))
                        {
                            //Consider to use negative values on the right side of the equation if the world is upside down
                            segment.Value.Position.y = segment.Value.Position.y;
                            segment.Value.Rotation.y = segment.Value.Rotation.y;  //added by FB
                            segment.Value.Position.x = segment.Value.Position.x; //added by FB
                            gameObject.transform.localPosition = t.position + segment.Value.Position; //leave this like that even if the world is upside down
                            segment.Value.Rotation.x = segment.Value.Rotation.x; //added by FB
                            gameObject.transform.localRotation = t.rotation * segment.Value.Rotation; //leave this like that even if the world is upside down
                        }
                    }
                }
            }
        }

        private void StartNewSkeleton(Skeleton skeleton)
        {
            Debug.Log("starting new skeleton");
            mQtmSkeletonCache = skeleton;
            if (mQtmSkeletonCache == null)
                return;

            mQtmSegmentIdToGameObject.Clear();
            mQtmSegmentIdToTransform.Clear();
            foreach (var segment in mQtmSkeletonCache.Segments)
            {
                GameObject go;
                if (mSegmentNameToGameObject.TryGetValue(segment.Value.Name, out go))
                {
                    mQtmSegmentIdToGameObject[segment.Value.Id] = go;
                    mQtmSegmentIdToTransform[segment.Value.Id] = new TransformAverage(go.transform.localPosition, go.transform.localRotation);
                }
                else
                    Debug.Log("Didn't Find " + RigSegmentPrefix + segment.Value.Name);
            }
            calibrating = true;
            calibrationIndex = 0;
        }

        private void Calibrate()
        {
            // Update all the game objects. Calibration is necessary to reset hand position in QTM space to our Unity scene
            TransformAverage t;
            foreach (var segment in mQtmSkeletonCache.Segments)
            {
                if (mQtmSegmentIdToTransform.TryGetValue(segment.Key, out t))
                {
                    Debug.Log(segment.Value.Name + " " + segment.Value.Position + " " + segment.Value.Rotation);
                    //Calls the "Add" method in TransformAverage.cs. Consider to use negative values on the right side of the equation if the world is upside down
                    segment.Value.Position.y = segment.Value.Position.y;
                    segment.Value.Rotation.y = segment.Value.Rotation.y;  //added by FB
                    segment.Value.Position.x = segment.Value.Position.x; //added by FB
                    segment.Value.Rotation.x = segment.Value.Rotation.x; //added by FB
                    t.Add(segment.Value.Position, segment.Value.Rotation);
                }
            }
            calibrationIndex++;

            if (calibrationIndex == CalibrationCount)
            {
                foreach (var segment in mQtmSkeletonCache.Segments)
                {
                    if (mQtmSegmentIdToTransform.TryGetValue(segment.Key, out t))
                    {
                        Debug.Log(segment.Value.Name + " " + t.position + " " + t.rotation);
                    }
                }
                calibrating = false;
            }
        }

        private void SetupGlove()
        {
            if (glove != null)
            {
                string[] fingers = { "Thumb", "Index", "Middle", "Ring", "Little" };
                string[] bones = { "Proximal", "Intermediate", "Distal" };

                string[] DestFingers = { "Thumb", "Index", "Middle", "Ring", "Pinky" };
                string[] DestBones = { "1", "2", "3" };
                string side = glove.Hand == Glove5dt.Hand.LEFT ? "Left" : "Right";

                int i = 0;
                foreach (var finger in fingers)
                {
                    foreach (var bone in bones)
                    {
                        string DestFinger = DestFingers[Array.IndexOf(fingers, finger)];
                        string DestBone = DestBones[Array.IndexOf(bones, bone)];
                        string fullName = RigSegmentPrefix + side + "Hand" + DestFinger + DestBone;

                        //HumanBone foundBone = Array.Find(DestinationAvatar.humanDescription.human, b => b.humanName == side + " " + finger + " " + bone);
                        //boneTransform[i] = FindDeepChild(GetComponent<Transform>(), foundBone.boneName);
                        boneTransform[i] = FindDeepChild(GetComponent<Transform>(), fullName);
                        boneQuaternion[i] = boneTransform[i].localRotation;
                        i++;
                    }
                }
            }
        }

        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;
                var result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void AnimateFingersFromGlove()
        {
            SlerpFinger(0, Quaternion.Euler(0, glove.values[Glove5dt.Sensors.THUMB_INDEX], 0));
            SlerpFinger(1, Quaternion.AngleAxis(-glove.values[Glove5dt.Sensors.THUMB_NEAR], new Vector3(0, 1, 1)));
            SlerpFinger(2, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.THUMB_FAR] / 1.6f, 0));
            SlerpFinger(3, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.INDEX_NEAR], 0) * Quaternion.Euler(0, 0, -glove.values[Glove5dt.Sensors.INDEX_MIDDLE]));
            SlerpFinger(4, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.INDEX_FAR], 0));
            SlerpFinger(5, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.INDEX_FAR] / 1.5f, 0));
            SlerpFinger(6, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.MIDDLE_NEAR], 0));
            SlerpFinger(7, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.MIDDLE_FAR], 0));
            SlerpFinger(8, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.MIDDLE_FAR] / 1.5f, 0));
            SlerpFinger(9, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.RING_NEAR], 0) * Quaternion.Euler(0, 0, glove.values[Glove5dt.Sensors.MIDDLE_RING]));
            SlerpFinger(10, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.RING_FAR], 0));
            SlerpFinger(11, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.RING_FAR] / 1.5f, 0));
            SlerpFinger(12, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.LITTLE_NEAR], 0) * Quaternion.Euler(0, 0, glove.values[Glove5dt.Sensors.RING_LITTLE] + glove.values[Glove5dt.Sensors.MIDDLE_RING]));
            SlerpFinger(13, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.LITTLE_FAR], 0));
            SlerpFinger(14, Quaternion.Euler(0, -glove.values[Glove5dt.Sensors.LITTLE_FAR] / 1.5f, 0));
        }

        private void SlerpFinger(int boneIndex, Quaternion target)
        {
            boneTransform[boneIndex].localRotation = boneQuaternion[boneIndex] * Quaternion.Slerp(boneTransform[boneIndex].localRotation, target, Time.fixedDeltaTime * fingerSmoothing);
        }
    }
}
