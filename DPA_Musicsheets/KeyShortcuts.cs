using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets
{
    class KeyShortcuts
    {
        private ChainOfResponsibility _actionChain = new ChainOfResponsibility();
        private List<Key> _keysDown = new List<Key>();

        private void textBox_keyDown(object sender, KeyEventArgs e)
        {
            _keysDown.Add(e.Key);
            if (_actionChain.Handle(_keysDown))
            {
                e.Handled = true;
                _keysDown.Clear();
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            _keysDown.Remove(e.Key);
        }
    }
}
