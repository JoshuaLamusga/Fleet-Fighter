using Microsoft.Xna.Framework.Input;
using System;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Like the KeyboardState class, but
    /// includes button releasing.
    /// </summary>
    class CtrlKeyboard
    {
        public KeyboardState keyStateOld, keyStateNew;

        /// <summary>
        /// Creates two KeyboardStates to compare past and present values.
        /// </summary>
        public CtrlKeyboard(KeyboardState keyStateOld)
        {
            this.keyStateOld = keyStateOld;
            this.keyStateNew = Keyboard.GetState();
        }

        /// <summary>
        /// Creates two KeyboardStates to compare past and present values.
        /// </summary>
        public CtrlKeyboard(KeyboardState keyStateOld, KeyboardState keyStateNew)
        {
            this.keyStateOld = keyStateOld;
            this.keyStateNew = keyStateNew;
        }

        /// <summary>
        /// Returns true if the key was released, false otherwise.
        /// </summary>
        public bool IsReleased(Keys key)
        {
            if (keyStateOld.IsKeyDown(key) &&
                keyStateNew.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the key is pressed right now (doesn't return true if held), false otherwise.
        /// </summary>
        public bool IsPressedFirst(Keys key)
        {
            if (keyStateOld.IsKeyUp(key) &&
                keyStateNew.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the key state changed, false otherwise.
        /// </summary>
        public bool HasChanged(Keys key)
        {
            if (keyStateOld.IsKeyDown(key) ==
                keyStateNew.IsKeyDown(key))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns whether or not the specified key is a letter.
        /// </summary>
        public static bool IsLetter(Keys key)
        {
            if (key == Keys.A ||
                key == Keys.B ||
                key == Keys.C ||
                key == Keys.D ||
                key == Keys.E ||
                key == Keys.F ||
                key == Keys.G ||
                key == Keys.H ||
                key == Keys.I ||
                key == Keys.J ||
                key == Keys.K ||
                key == Keys.L ||
                key == Keys.M ||
                key == Keys.N ||
                key == Keys.O ||
                key == Keys.P ||
                key == Keys.Q ||
                key == Keys.R ||
                key == Keys.S ||
                key == Keys.T ||
                key == Keys.U ||
                key == Keys.V ||
                key == Keys.W ||
                key == Keys.X ||
                key == Keys.Y ||
                key == Keys.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether or not the specified key is a letter.
        /// </summary>
        public static bool IsDigit(Keys key)
        {
            if (key == Keys.D0 ||
                key == Keys.D1 ||
                key == Keys.D2 ||
                key == Keys.D3 ||
                key == Keys.D4 ||
                key == Keys.D5 ||
                key == Keys.D6 ||
                key == Keys.D7 ||
                key == Keys.D8 ||
                key == Keys.D9 ||
                key == Keys.NumPad0 ||
                key == Keys.NumPad1 ||
                key == Keys.NumPad2 ||
                key == Keys.NumPad3 ||
                key == Keys.NumPad4 ||
                key == Keys.NumPad5 ||
                key == Keys.NumPad6 ||
                key == Keys.NumPad7 ||
                key == Keys.NumPad8 ||
                key == Keys.NumPad9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the proper string representation of visible keys.
        /// </summary>
        /// <param name="isCaps">Whether or not shift is active (not capslock).</param>
        public static string KeyToString(Keys key, bool shiftPressed)
        {
            //Checks for letters.
            if (IsLetter(key))
            {
                if (Console.CapsLock == true || shiftPressed)
                {
                    return key.ToString();
                }
                else
                {
                    return key.ToString().ToLower();
                }
            }

            //Checks for keys that have shift-based variants.
            if (shiftPressed)
            {
                //Digits
                if (key == Keys.D0 || key == Keys.NumPad0) { return ")"; }
                else if (key == Keys.D1 || key == Keys.NumPad1) { return "!"; }
                else if (key == Keys.D2 || key == Keys.NumPad2) { return "@"; }
                else if (key == Keys.D3 || key == Keys.NumPad3) { return "#"; }
                else if (key == Keys.D4 || key == Keys.NumPad4) { return "$"; }
                else if (key == Keys.D5 || key == Keys.NumPad5) { return "%"; }
                else if (key == Keys.D6 || key == Keys.NumPad6) { return "^"; }
                else if (key == Keys.D7 || key == Keys.NumPad7) { return "&"; }
                else if (key == Keys.D8 || key == Keys.NumPad8) { return "*"; }
                else if (key == Keys.D9 || key == Keys.NumPad9) { return "("; }
                //Punctuation
                else if (key == Keys.OemTilde) { return "~"; }
                else if (key == Keys.OemOpenBrackets) { return "{"; }
                else if (key == Keys.OemCloseBrackets) { return "}"; }
                else if (key == Keys.OemSemicolon) { return ":"; }
                else if (key == Keys.OemQuotes) { return "\""; }
                else if (key == Keys.OemComma) { return "<"; }
                else if (key == Keys.OemPeriod || key == Keys.Decimal) { return ">"; }
                else if (key == Keys.OemQuestion) { return "?"; }
                else if (key == Keys.OemMinus) { return "_"; }
                else if (key == Keys.OemPlus) { return "+"; }
                else if (key == Keys.OemPipe) { return "|"; }
            }
            else
            {
                //Digits
                if (key == Keys.D0 || key == Keys.NumPad0) { return "0"; }
                else if (key == Keys.D1 || key == Keys.NumPad1) { return "1"; }
                else if (key == Keys.D2 || key == Keys.NumPad2) { return "2"; }
                else if (key == Keys.D3 || key == Keys.NumPad3) { return "3"; }
                else if (key == Keys.D4 || key == Keys.NumPad4) { return "4"; }
                else if (key == Keys.D5 || key == Keys.NumPad5) { return "5"; }
                else if (key == Keys.D6 || key == Keys.NumPad6) { return "6"; }
                else if (key == Keys.D7 || key == Keys.NumPad7) { return "7"; }
                else if (key == Keys.D8 || key == Keys.NumPad8) { return "8"; }
                else if (key == Keys.D9 || key == Keys.NumPad9) { return "9"; }
                //Punctuation
                if (key == Keys.OemTilde) { return "`"; }
                else if (key == Keys.OemOpenBrackets) { return "["; }
                else if (key == Keys.OemCloseBrackets) { return "]"; }
                else if (key == Keys.OemSemicolon) { return ";"; }
                else if (key == Keys.OemQuotes) { return "'"; }
                else if (key == Keys.OemComma) { return ","; }
                else if (key == Keys.OemPeriod || key == Keys.Decimal) { return "."; }
                else if (key == Keys.OemQuestion) { return "/"; }
                else if (key == Keys.OemMinus) { return "-"; }
                else if (key == Keys.OemPlus) { return "="; }                
                else if (key == Keys.OemPipe) { return "\\"; }
            }

            //Miscellaneous keys and punctuation
            if (key == Keys.Space) { return " "; }
            else if (key == Keys.Divide) { return "/"; }
            else if (key == Keys.Add) { return "+"; }
            else if (key == Keys.Subtract) { return "-"; }
            else if (key == Keys.Multiply) { return "*"; }

            //If the key pressed was something else (like an unprintable character)
            return "";
        }
    }
}