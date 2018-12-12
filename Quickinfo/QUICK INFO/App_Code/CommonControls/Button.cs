using System;

namespace MsgBox
{
    public class Button
    {
        private string _id;
        private string _text;
        private string _cssClass;
        private string _onClick;

        public Button(string text, string cssClass, string onClick)
        {
            this._text = text;
            this._cssClass = cssClass;
            this._onClick = onClick;
        }

        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }

        public string CssClass
        {
            get { return this._cssClass; }
            set { this._cssClass = value; }
        }

        public string OnClick
        {
            get { return this._onClick; }
            set { this._onClick = value; }
        }

        public string Render()
        {
            string control = "<input type=\"Button\" id=\"" + this._id + "\" class=\"" + this._cssClass + "\" value=\"" + this._text + "\"";

            if (this._onClick != "")
            {
                control += " onclick=\"" + this._onClick + "\"";
            }
            return control += " />";

          
        }

        protected void btnClick(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.Session["Test"] = "aaa";
        }
    }
}