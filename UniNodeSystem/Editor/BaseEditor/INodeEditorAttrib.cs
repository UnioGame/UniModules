using System;

namespace UniNodeSystemEditor
{
    public interface INodeEditorAttribute {
        Type GetInspectedType();
    }
}