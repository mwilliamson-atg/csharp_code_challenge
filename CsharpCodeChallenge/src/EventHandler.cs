using System;
using System.Collections.Generic;
using System.Reflection;

namespace code.challenge
{
    /**
     * The EventHandler is, unsurprisingly, responsible for dispatching events between disparate objects.
     * The basic usage is simple: Register an object and anything annotated with [Subscribe(SomeEvent)] will be called when that event is dispatched
     */
    public static class EventHandler
    {
        private static readonly Dictionary<EventType, Dictionary<object, List<RegisteredMethod>>> eventMap = new Dictionary<EventType, Dictionary<object, List<RegisteredMethod>>>();
        public static void register(object obj)
        {
            var methods = obj.GetType().GetMethods();
            foreach (var method in methods)
            {
                foreach (var attribute in method.GetCustomAttributes(typeof(SubscribeAttribute), true))
                {
                    var subscribeAttribute = (SubscribeAttribute)attribute;
                    registerMethod(obj, method, subscribeAttribute.eventType);
                }
            }
        }

        public static void unregister(object obj)
        {
            foreach (var eventType in eventMap.Keys)
            {
                var dictionary = eventMap[eventType];
                if (dictionary.ContainsKey(obj))
                    dictionary.Remove(obj);
            }
        }

        public static void postEvent(EventType eventType, Dictionary<string, object> data = null)
        {
            var dictionary = eventMap[eventType]; 
            foreach (var obj in dictionary.Keys)
            {
                foreach (var registeredMethod in dictionary[obj])
                {
                    registeredMethod.method.Invoke(registeredMethod.obj, registeredMethod.usesExtraData ? new object[] {data} : null);
                }
            }
        }

        private static void registerMethod(object obj, MethodInfo method, EventType eventType)
        {
            if (!eventMap.ContainsKey(eventType))
            {
                eventMap.Add(eventType, new Dictionary<object, List<RegisteredMethod>>());
            }

            var dictionary = eventMap[eventType];
            List<RegisteredMethod> list;
            if (dictionary.ContainsKey(obj))
                list = dictionary[obj];
            else
            {
                list = new List<RegisteredMethod>();
                dictionary.Add(obj, list);
            }
            list.Add(new RegisteredMethod(obj, method));
        }
        
    }

    internal class RegisteredMethod
    {
        internal readonly object obj;
        internal readonly MethodInfo method;
        internal readonly bool usesExtraData;

        public RegisteredMethod(object obj, MethodInfo method)
        {
            this.obj = obj;
            this.method = method;
            usesExtraData = method.GetParameters().Length > 0;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
        public readonly EventType eventType;

        public SubscribeAttribute(EventType eventType)
        {
            this.eventType = eventType;
        }
    }
}