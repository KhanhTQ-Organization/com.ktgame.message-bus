using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.ktgame.message_bus.editor
{
    [Serializable]
    [HideReferenceObjectPicker]
    public class MessageEventField
    {
        [TableColumnWidth(150, Resizable = false)]
        [HideLabel]
        public string Type = "string";

        [HideLabel]
        public string Name = "value";
    }

    [Serializable]
    [HideReferenceObjectPicker]
    public class MessageEventDefinition
    {
        [LabelText("Event Class Name")]
        public string EventName;

        [Space(5)]
        [TableList(AlwaysExpanded = true)]
        public List<MessageEventField> Fields = new List<MessageEventField>();
    }

    public class MessageBusSettings : ScriptableObject
    {
        private static MessageBusSettings _instance;
        public static MessageBusSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    var path = "Packages/com.ktgame.message-bus/Editor/MessageBusSettings.asset";
                    _instance = AssetDatabase.LoadAssetAtPath<MessageBusSettings>(path);
                    if (_instance == null)
                    {
                        _instance = CreateInstance<MessageBusSettings>();
                        AssetDatabase.CreateAsset(_instance, path);
                        AssetDatabase.SaveAssets();
                    }
                }
#endif
                return _instance;
            }
        }

        [HideInInspector] public string GeneratePath = "Assets/Scripts/MessageBus";
        [HideInInspector] public List<MessageEventDefinition> Events = new List<MessageEventDefinition>();
    }
}
