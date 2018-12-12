using System;

namespace MsgBox
{
    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException(string message) : base(message){}
    }
}