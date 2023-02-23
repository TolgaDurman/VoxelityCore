using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Timer
{
    public class TimerManager : Singleton<TimerManager>
    {
        private List<Timer> timers = new List<Timer>();
        private List<Timer> removedTimers = new List<Timer>();

        public void Assign(Timer timer) => timers.Add(timer);
        public void Remove(Timer timer)
        {
            if (timers.Contains(timer) && !removedTimers.Contains(timer)) removedTimers.Add(timer);
        }
        public void KillAll() => timers.ForEach(x => x.Kill());
        public void CompleteAll(bool useOnComplete = true) => timers.ForEach(x => x.Complete(useOnComplete));
        public void Kill(object id) => timers.Where(x => x.IsEquals(id)).ToList().ForEach(x => x.Kill());
        
        public void Kill(Object reference) => timers.Where(x => x.IsEquals(reference)).ToList().ForEach(x => x.Kill());
        
        public void Complete(object id, bool useOnComplete = true) 
        => timers.Where(x => x.IsEquals(id)).ToList().ForEach(x => x.Complete(useOnComplete));
        
        public void Complete(Object reference, bool useOnComplete = true) 
        => timers.Where(x => x.IsEquals(reference)).ToList().ForEach(x => x.Complete(useOnComplete));

        private void Update()
        {
            timers.Where(x => !removedTimers.Contains(x)).ToList().ForEach(x => x.Tick(Time.unscaledDeltaTime));
            removedTimers.Where(x => timers.Contains(x)).ToList().ForEach(x => timers.Remove(x));
            removedTimers.Clear();
        }

    }
}
