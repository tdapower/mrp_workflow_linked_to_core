using System;
using System.Collections.Generic;

namespace MsgBox
{
    public class ButtonCollection
    {
        private List<Button> _collection = new List<Button>();

        public ButtonCollection(){}

        public void Add(Button button)
        {
            this._collection.Add(button);
        }

        public List<Button> GetCollection()
        {
            return this._collection;
        }
    }
}