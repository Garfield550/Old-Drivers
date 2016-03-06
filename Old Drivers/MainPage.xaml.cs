using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Text;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using System.Text.RegularExpressions;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Old_Drivers
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the dictionary of all characters in morse code (true being a dash, false being a dot).
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        public static Dictionary<char, string> MorseCharacterValues
        {
            get
            {
                return new Dictionary<char, string>()
                {
                    { 'A', ".-" },
                    { 'B', "-..." },
                    { 'C', "-.-." },
                    { 'D', "-.." },
                    { 'E', "." },
                    { 'F', "..-." },
                    { 'G', "--." },
                    { 'H', "...." },
                    { 'I', ".." },
                    { 'J', ".---" },
                    { 'K', "-.-" },
                    { 'L', ".-.." },
                    { 'M', "--" },
                    { 'N', "-." },
                    { 'O', "---" },
                    { 'P', ".--." },
                    { 'Q', "--.-" },
                    { 'R', ".-." },
                    { 'S', "..." },
                    { 'T', "-" },
                    { 'U', "..-" },
                    { 'V', "...-" },
                    { 'W', ".--" },
                    { 'X', "-..-" },
                    { 'Y', "-.--" },
                    { 'Z', "--.." },
                    { '0', "-----" },
                    { '1', ".----" },
                    { '2', "..---" },
                    { '3', "...--" },
                    { '4', "....-" },
                    { '5', "....." },
                    { '6', "-...." },
                    { '7', "--..." },
                    { '8', "---.." },
                    { '9', "----." },
                    { '.', ".-.-.-" },
                    { ',', "--..--" },
                    { '?', "..--.." },
                    { '!', "-.-.--" },
                    { '\'', ".----." },
                    { '"', ".-..-." },
                    { '(', "-.--." },
                    { ')', "-.--.-" },
                    { '&', ".-..." },
                    { ':', "---..." },
                    { ';', "-.-.-." },
                    { '=', "-...-" },
                    { '+', ".-.-." },
                    { '-', "-....-" },
                    { '/', "-..-." },
                    { '_', "..--.-" },
                    { '$', "...-..-" },
                    { '@', ".--.-." },
                    { '%', ".-.-.- -..-. .-.-.-" }
                };
            }
        }

        /// <summary>
        /// Converts a message from morse to english.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <returns> The english of the message. </returns>
        public static string ConvertFromMorse(string message)
        {
            // Copy each character into the result string
            string res = string.Empty;
            foreach (string word in message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string letter in word.Split(' '))
                {
                    res += FindLetter(letter);
                }

                res += ' ';
            }

            return res;
        }

        /// <summary>
        /// Finds the character value of a morse code character.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="morseVal"> The morse code of the character. </param>
        /// <returns> The english character. </returns>
        public static char FindLetter(string morseVal)
        {
            foreach (char c in MorseCharacterValues.Keys)
            {
                if (MorseCharacterValues[c] == morseVal)
                {
                    return c;
                }
            }

            // Make unknown characters questions marks
            return '?';
        }

        /// <summary>
        /// Converts plain text to morse code.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="message"> The plain text. </param>
        /// <returns> The morse code. Null if it could not convert it. </returns>
        public static string ConvertToMorse(string message)
        {
            // Make the message upper case so it has recognisable characters
            message = message.ToUpper();

            // Return null if the message is not valid
            if (!ValidEnglish(message))
            {
                return null;
            }
            else
            {
                // Otherwise convert it
                string res = string.Empty;

                // Split by word
                foreach (string word in message.Split(' '))
                {
                    // Say a space does not need to be added
                    bool addSpace = false;

                    // Split by characters in the word
                    foreach (char c in word)
                    {
                        // Add the character gap signifier if it needs to be added
                        if (!addSpace)
                        {
                            addSpace = !addSpace;
                        }
                        else
                        {
                            res += ' ';
                        }

                        res += MorseCharacterValues[c];
                    }

                    // Add the word gap signifier
                    // res += '|';
                }

                return res;
            }
        }

        /// <summary>
        /// Checks if a message is valid and won't throw errors when being played.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="message"> The message to check. </param>
        /// <param name="isMorseForm"> Whether the message is in morse form. </param>
        /// <returns> Whether the message is valid. </returns>
        public static bool ValidMessage(string message, bool isMorseForm = false)
        {
            if (isMorseForm)
            {
                return ValidMorse(message);
            }
            else
            {
                return ValidEnglish(message);
            }
        }

        /// <summary>
        /// Validates english to see if it can be converted to morse code.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <returns> Whether the message is valid. </returns>
        public static bool ValidEnglish(string message)
        {
            // Make the message upper case
            message = message.ToUpper();

            // Check each character is valid
            foreach (string word in message.Split(' '))
            {
                foreach (char c in word)
                {
                    if (!MorseCharacterValues.ContainsKey(c))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates a morse message.
        /// <copyright url="https://github.com/wrightg42/morse-code"> This code is protected under the MIT License. </copyright>
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <returns> Whether the message is valid. </returns>
        public static bool ValidMorse(string message)
        {
            // Make the message upper case
            message = message.ToUpper();

            // Check each letter is valid
            foreach (string word in message.Split(new char[] { '\\', '|', '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string letter in word.Split(' '))
                {
                    if (!MorseCharacterValues.ContainsValue(letter))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Trim White Space
        /// Code copy from http://www.codeproject.com/Articles/1014073/Fastest-method-to-remove-all-whitespace-from-Strin
        /// </summary>
        static Regex whitespace = new Regex("\\s+", RegexOptions.Compiled);

        public static string TrimWithRegex(string str)
        {
            return whitespace.Replace(str, string.Empty);
        }

        /// <summary>
        /// Trim All
        /// </summary>
        public static string TrimAll(string str)
        {
            str = str.Trim();
            return TrimWithRegex(str);
        }

        private async void showAbout(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialogAbout();
            await contentDialog.ShowAsync();
        }

        private void mainPivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pivotIndex = mainPivot.SelectedIndex;

            switch (pivotIndex)
            {
                case 0:
                    appBarButtonDecode.Visibility = Visibility.Visible;
                    break;
                case 1:
                    appBarButtonDecode.Visibility = Visibility.Visible;
                    break;
                case 2:
                    appBarButtonDecode.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    appBarButtonDecode.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void appBarButtonEncodeClick(object sender, RoutedEventArgs e)
        {
            if (mainPivot.SelectedIndex == 0)
            {
                morseTextBoxTranslate.Text = ConvertToMorse(morseTextBoxSource.Text);
            }
            else if (mainPivot.SelectedIndex == 1)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(base64TextBoxSource.Text);
                try
                {
                    base64TextBoxTranslate.Text = Convert.ToBase64String(bytes);
                }
                catch
                {
                    base64TextBoxTranslate.Text = base64TextBoxSource.Text;
                }
            }
            else if (mainPivot.SelectedIndex == 2)
            {
                string text = TrimAll(baiduTextBoxSource.Text);

                if (baiduTextBoxSource.Text.Length == 8)
                {
                    baiduTextBoxTranslate.Text = "http://pan.baidu.com/s/" + text;
                }
                else if (baiduTextBoxSource.Text.Length == 9)
                {
                    baiduTextBoxTranslate.Text = "http://pan.baidu.com/s" + text;
                }
                else if (baiduTextBoxSource.Text.Length == 10)
                {
                    baiduTextBoxTranslate.Text = "http://pan.baidu.com/" + text;
                }
                else if (baiduTextBoxSource.Text.Length == 11)
                {
                    baiduTextBoxTranslate.Text = "http://pan.baidu.com" + text;
                }
                else
                {
                    baiduTextBoxTranslate.Text = "?";
                }
            }
            else if (mainPivot.SelectedIndex == 3)
            {
                string text = TrimAll(magnetTextBoxSource.Text);

                if (magnetTextBoxSource.Text.Length == 40)
                {
                    magnetTextBoxTranslate.Text = "magnet:?xt=urn:btih:" + text;
                }
                else
                {
                    magnetTextBoxTranslate.Text = "?";
                }
            }
        }

        private void appBarButtonDecodeClick(object sender, RoutedEventArgs e)
        {
            if (mainPivot.SelectedIndex == 0)
            {
                morseTextBoxTranslate.Text = ConvertFromMorse(morseTextBoxSource.Text);
            }
            else if (mainPivot.SelectedIndex == 1)
            {
                byte[] outputb = Convert.FromBase64String(base64TextBoxSource.Text);
                try
                {
                    base64TextBoxTranslate.Text = Encoding.UTF8.GetString(outputb);
                }
                catch
                {
                    base64TextBoxTranslate.Text = base64TextBoxSource.Text;
                }
            }
        }

        private void appBarButtonCopyClick(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            if (mainPivot.SelectedIndex == 0)
            {
                dataPackage.SetText(morseTextBoxTranslate.Text);
            }
            else if (mainPivot.SelectedIndex == 1)
            {
                dataPackage.SetText(base64TextBoxTranslate.Text);
            }
            else if (mainPivot.SelectedIndex == 2)
            {
                dataPackage.SetText(baiduTextBoxTranslate.Text);
            }
            else if (mainPivot.SelectedIndex == 3)
            {
                dataPackage.SetText(magnetTextBoxTranslate.Text);
            }
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            Clipboard.SetContent(dataPackage);
        }

        private void appBarButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            morseTextBoxSource.Text = string.Empty;
            base64TextBoxSource.Text = string.Empty;
            baiduTextBoxSource.Text = string.Empty;
            magnetTextBoxSource.Text = string.Empty;
            morseTextBoxTranslate.Text = string.Empty;
            base64TextBoxTranslate.Text = string.Empty;
            baiduTextBoxTranslate.Text = string.Empty;
            magnetTextBoxTranslate.Text = string.Empty;
        }
    }
}
