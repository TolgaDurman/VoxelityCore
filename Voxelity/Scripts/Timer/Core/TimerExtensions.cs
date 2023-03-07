using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Voxelity.Timers
{
    public static class TimerExtensions
    {
        public static Timer CountTime(this Transform self, float time) => new Timer(time).SetReference(self);
        public static Timer CountTime(this Transform self, float time,UnityAction onComplete) => new Timer(time).SetReference(self).OnComplete(onComplete);
        public static Timer CountTime(this GameObject self, float time) => new Timer(time).SetReference(self);
        public static Timer CountTime(this GameObject self, float time,UnityAction onComplete) => new Timer(time).SetReference(self).OnComplete(onComplete);
        public static Timer CountTime(this MonoBehaviour self,float time) => new Timer(time).SetReference(self);
        public static Timer SetActive(this GameObject self, bool value,float time) => new Timer(time).OnComplete(()=>self.SetActive(value));
    }
}
