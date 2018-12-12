using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace MsgBox
{
    public class MessageBox : Control
    {
        private string _template;
        private string _title;
        private string _message;

        private ButtonCollection _buttonCollection = new ButtonCollection();
        private VariableCollection _variableCollection = new VariableCollection();

        public MessageBox() { }

        public MessageBox(string template) 
        {
            this._template = template;
        }

        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }

        public string Template
        {
            get { return this._template; }
            set { this._template = value; }
        }

        public ButtonCollection Buttons
        {
            get { return this._buttonCollection; }
        }

        public VariableCollection Variable
        {
            get { return this._variableCollection; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (!File.Exists(this._template))
            {
                // Throw an exception if the template file was not found.
                if (this._template.Length > 30)
                {
                    this._template = "..." + this._template.Substring(this._template.Length - 30);
                }
                throw new MsgBox.TemplateNotFoundException(String.Format("MessageBox template \"{0}\" file not found", this._template));
            }

            using (StreamReader reader = new StreamReader(this._template))
            {
                string template = reader.ReadToEnd();
                string buttonContainer = "";

                foreach (Button button in this._buttonCollection.GetCollection())
                {
                    buttonContainer += button.Render() + " ";
                }

                template = template.Replace("{@TITLE}", this._title);
                template = template.Replace("{@MESSAGE}", this._message);
                template = template.Replace("{@BUTTONS}", buttonContainer);

                foreach (KeyValuePair<string, string> variable in this._variableCollection.GetCollection())
                {
                    template = template.Replace("{@" + variable.Key.ToUpper() + "}", variable.Value);
                }

                reader.Close();
                output.Write(template);
            }
        }
    }
}