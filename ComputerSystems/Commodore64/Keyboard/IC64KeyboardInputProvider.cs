using System.Windows.Forms;

namespace Commodore64.Keyboard
{
    public interface IC64KeyboardInputProvider
    {
        public bool IsKeyDown(Keys key);
    }
}
