namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    public delegate void EventAggregateCallback(object source);

    public class EventAggregate
    {
        private static EventAggregate instance;

        private readonly IDictionary<string, IList<EventAggregateCallback>> subscriptions;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public EventAggregate()
        {
            this.subscriptions = new Dictionary<string, IList<EventAggregateCallback>>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static EventAggregate Instance
        {
            get
            {
                return instance ?? (instance = new EventAggregate());
            }
        }

        public void Subscribe(string key, EventAggregateCallback callback)
        {
            if (!this.subscriptions.ContainsKey(key))
            {
                this.subscriptions.Add(key, new List<EventAggregateCallback>());
            }

            System.Diagnostics.Trace.Assert(!this.subscriptions[key].Contains(callback));

            this.subscriptions[key].Add(callback);
        }

        public void Unsubscribe(string key, EventAggregateCallback callback)
        {
            if(!this.subscriptions.ContainsKey(key))
            {
                Debug.LogWarning("Unsubscribe called with missing key!");
                return;
            }

            this.subscriptions[key].Remove(callback);
        }

        public void Notify(string key, object source = null)
        {
            if (!this.subscriptions.ContainsKey(key))
            {
                return;
            }

            foreach (EventAggregateCallback callback in this.subscriptions[key])
            {
                callback(source);
            }
        }
    }
}
