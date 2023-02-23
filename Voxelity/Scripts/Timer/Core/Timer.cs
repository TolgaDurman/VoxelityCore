using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Voxelity.Timer
{
    public class Timer
    {
        private float _completeTime;
        private float _countedTime;
        private bool _enabled = true;
        private bool _autoKill = true;
        private bool _unscaledTime = false;
        private UnityAction _onComplete;
        private UnityAction<float> _onUpdate;
        private UnityAction<float> _onUpdate01;
        private Object _referenceObject;
        private object _id;
        public float RemainingTime
        {
            get => _completeTime - _countedTime;
        }

        public Timer(float completeTime, bool enabled = true)
        {
            this._completeTime = completeTime;
            this._enabled = enabled;
            TimerManager.Instance.Assign(this);
        }
        public Timer SetAutoKill(bool value)
        {
            _autoKill = value;
            return this;
        }
        public Timer OnUpdate(UnityAction<float> onUpdate)
        {
            this._onUpdate = onUpdate;
            return this;
        }
        public Timer OnUpdate01(UnityAction<float> onUpdate01)
        {
            this._onUpdate01 = onUpdate01;
            return this;
        }
        public Timer OnComplete(UnityAction onComplete)
        {
            this._onComplete = onComplete;
            return this;
        }
        public Timer SetUnscaledTime(bool value)
        {
            _unscaledTime = !value;
            return this;
        }
        public Timer SetReference(Object referenceObject)
        {
            _referenceObject = referenceObject;
            return this;
        }
        public Timer SetId(object id)
        {
            this._id = id;
            return this;
        }
        public bool IsEquals(object reference)
        {
            if (_id.Equals(reference))
                return true;
            return false;
        }
        public bool IsEquals(Object reference)
        {
            if (_referenceObject == reference)
                return true;
            return false;
        }
        public void Play() => _enabled = true;
        public void Pause() => _enabled = false;
        public void Reset() => _countedTime = 0;
        public void Restart()
        {
            _countedTime = 0f;
            Play();
        }
        public void AddTime(float value) => _countedTime -= value;

        public void Tick(float delta)
        {
            if (!_enabled)
                return;

            _onUpdate?.Invoke(_countedTime);
            _onUpdate01?.Invoke(Mathf.Clamp01(_countedTime / _completeTime));

            _countedTime += delta * (_unscaledTime ? 1f : Time.timeScale);

            if (RemainingTime <= 0)
                Complete(_onComplete != null);
        }
        public void Kill()
        {
            TimerManager.Instance.Remove(this);
        }
        public void Complete(bool executeOnComplete = true)
        {
            _enabled = false;
            _countedTime = _completeTime;
            
            _onUpdate?.Invoke(_completeTime);
            _onUpdate01?.Invoke(1f);
            
            if (executeOnComplete)
                _onComplete?.Invoke();

            if (!_autoKill) return;
            Kill();
        }
    }
}
