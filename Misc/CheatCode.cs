using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Revolution.IO
{
    public class CheatCode
    {
        public event EventHandler CodeEntered;
        public readonly Key[] CharacterSequence;
        private int currentKey = 0;

        public CheatCode(Key[] charSequence)
        {
            CharacterSequence = charSequence;
        }

        public void ProcessKey(Key key)
        {
            if (key == CharacterSequence[currentKey])
            {
                currentKey++;
            }
            else
            {
                currentKey = 0;
            }

            if (currentKey == CharacterSequence.Length)
            {
                currentKey = 0;
                CodeEntered?.Invoke(this, null);
            }
        }
    }
}