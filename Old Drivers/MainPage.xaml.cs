using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Text;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;


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
                    res += '|';
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

        private void button_Encode_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                textBoxDecode.Text = ConvertFromMorse(textBox.Text);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textBox.Text);
                textBoxDecode.Text = Convert.ToBase64String(bytes);
            }
            else if (comboBox.SelectedIndex == 2)
            {
                if (textBox.Text.Length == 8)
                {
                    textBoxDecode.Text = "http://pan.baidu.com/s/" + textBox.Text;
                }
            }
            else if (comboBox.SelectedIndex == 3)
            {
                if (textBox.Text.Length == 40)
                {
                    textBoxDecode.Text = "magnet:?xt=urn:btih:" + textBox.Text;
                }
            }
        }

        private void button_Decode_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                textBoxDecode.Text = ConvertToMorse(textBox.Text);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                byte[] outputb = Convert.FromBase64String(textBox.Text);
                textBoxDecode.Text = Encoding.UTF8.GetString(outputb);
            }
        }

        private async void showAbout(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialogAbout();
            await contentDialog.ShowAsync();
        }

        private void comboBox_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 2)
            {
                buttonDecoding.IsEnabled = false;
            }
            else if (comboBox.SelectedIndex == 3)
            {
                buttonDecoding.IsEnabled = false;
            }
            else
            {
                buttonDecoding.IsEnabled = true;
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(textBoxDecode.Text);
            Clipboard.SetContent(dataPackage);
        }
    }
}
