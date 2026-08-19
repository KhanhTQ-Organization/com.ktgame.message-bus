using UnityEditor;
using com.ktgame.core.editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace com.ktgame.message_bus.editor
{
    [InitializeOnLoad]
    public class MessageBusEditorModule : IEditorDirtyHandler, IMenuTreeExtension
    {
        static MessageBusEditorModule()
        {
            var module = new MessageBusEditorModule();
            EditorDirtyRegistry.Register(module);
            MenuTreeExtensionRegistry.Register(module);
        }
        
        public void SetDirty()
        {
            var instance = MessageBusSettings.Instance;
            if (instance != null)
            {
                EditorUtility.SetDirty(instance);
            }
        }
        
        public void BuildMenu(OdinMenuTree tree)
        {
            tree.Add("Message Bus", new MessageBusEditor(MessageBusSettings.Instance), SdfIconType.EnvelopeFill);
        }
    }
}
