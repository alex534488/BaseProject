using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(CCC.Input.KeyBindingEntry.EntryEvent))]
    public class Generated_CCC_Input_KeyBindingEntry_KeyBindingEntryEvent_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_CCC_Input_KeyBindingEntry_KeyBindingEntryEvent_MonoBehaviourStorage, CCC.Input.KeyBindingEntry.EntryEvent> {
        public override bool CanEdit(Type type) {
            return typeof(CCC.Input.KeyBindingEntry.EntryEvent).IsAssignableFrom(type);
        }
    }
}
