using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.ktgame.message_bus.editor
{
    public class MessageBusEditor
    {
        private MessageBusSettings _settings;

        public MessageBusEditor(MessageBusSettings settings)
        {
            _settings = settings;
        }

        [Title("Message Bus Configuration", "Manage and Generate your Events", TitleAlignments.Centered)]
        [InfoBox("Define your global events here. Click 'Generate' to automatically create the C# structs.", InfoMessageType.Info)]
        
        [PropertySpace(SpaceAfter = 10)]
        [FolderPath]
        [LabelText("Generate Path")]
        [ShowInInspector]
        [OnValueChanged("MarkDirty")]
        public string GeneratePath
        {
            get => _settings.GeneratePath;
            set => _settings.GeneratePath = value;
        }

        [PropertySpace(SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, ListElementLabelName = "EventName", Expanded = true)]
        [LabelText("Event Definitions")]
        [ShowInInspector]
        [OnValueChanged("MarkDirty")]
        public System.Collections.Generic.List<MessageEventDefinition> Events
        {
            get => _settings.Events;
            set => _settings.Events = value;
        }

        [BoxGroup("Actions", CenterLabel = true)]
        [HorizontalGroup("Actions/Buttons")]
        [Button("Generate Event Classes", ButtonSizes.Large, Icon = SdfIconType.CodeSlash)]
        [GUIColor(0.2f, 0.8f, 0.2f)]
        public void GenerateCode()
        {
            if (_settings == null) return;
            
            var saveFolderPath = Path.Combine(Application.dataPath, _settings.GeneratePath.Replace("Assets/", ""));
            
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            foreach (var evt in _settings.Events)
            {
                if (string.IsNullOrWhiteSpace(evt.EventName)) continue;
                
                var filePath = Path.Combine(saveFolderPath, evt.EventName + ".cs");
                var builder = new StringBuilder();
                
                builder.AppendLine("using com.ktgame.message_bus;");
                builder.AppendLine();
                builder.AppendLine($"public struct {evt.EventName} : IMessage");
                builder.AppendLine("{");
                
                // Add fields
                foreach (var field in evt.Fields)
                {
                    if (string.IsNullOrWhiteSpace(field.Type) || string.IsNullOrWhiteSpace(field.Name)) continue;
                    builder.AppendLine($"    public {field.Type} {field.Name};");
                }
                
                if (evt.Fields.Count > 0) builder.AppendLine();
                
                // Add constructor
                if (evt.Fields.Count > 0)
                {
                    builder.Append($"    public {evt.EventName}(");
                    for (int i = 0; i < evt.Fields.Count; i++)
                    {
                        var field = evt.Fields[i];
                        builder.Append($"{field.Type} {field.Name.ToLower()}");
                        if (i < evt.Fields.Count - 1) builder.Append(", ");
                    }
                    builder.AppendLine(")");
                    builder.AppendLine("    {");
                    
                    foreach (var field in evt.Fields)
                    {
                        builder.AppendLine($"        this.{field.Name} = {field.Name.ToLower()};");
                    }
                    
                    builder.AppendLine("    }");
                }
                
                builder.AppendLine("}");
                
                File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
            }

            AssetDatabase.Refresh();
            Debug.Log($"[MessageBus] Generated {_settings.Events.Count} event classes successfully!");
        }

        [HorizontalGroup("Actions/Buttons")]
        [Button("Sync from Code", ButtonSizes.Large, Icon = SdfIconType.ArrowRepeat)]
        [GUIColor(0.2f, 0.6f, 1f)]
        public void SyncFromCode()
        {
            if (_settings == null) return;
            
            var messageTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => {
                    try { return a.GetTypes(); } catch { return new System.Type[0]; }
                })
                .Where(t => typeof(com.ktgame.message_bus.IMessage).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            int addedCount = 0;
            int updatedCount = 0;

            foreach (var type in messageTypes)
            {
                var existingEvent = _settings.Events.Find(e => e.EventName == type.Name);
                if (existingEvent == null)
                {
                    existingEvent = new MessageEventDefinition { EventName = type.Name };
                    _settings.Events.Add(existingEvent);
                    addedCount++;
                }
                else
                {
                    updatedCount++;
                }

                existingEvent.Fields.Clear();
                var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var f in fields)
                {
                    existingEvent.Fields.Add(new MessageEventField
                    {
                        Name = f.Name,
                        Type = GetFriendlyTypeName(f.FieldType)
                    });
                }
            }

            MarkDirty();
            Debug.Log($"[MessageBus] Sync complete! Added {addedCount}, Updated {updatedCount} events.");
        }

        private string GetFriendlyTypeName(System.Type type)
        {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(string)) return "string";
            if (type == typeof(long)) return "long";
            if (type == typeof(short)) return "short";
            if (type == typeof(byte)) return "byte";
            return type.Name;
        }

        private void MarkDirty()
        {
            if (_settings != null)
            {
                EditorUtility.SetDirty(_settings);
            }
        }
    }
}
