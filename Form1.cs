using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Lab_4_Основи_програмної_інженерії_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.HideSelection = false;
            richTextBox1.Dock = DockStyle.Fill;
            BindMenuEvents();
        }

        private void BindMenuEvents()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl is MenuStrip ms)
                {
                    AttachHandlers(ms.Items);
                }
            }
        }

        private void AttachHandlers(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    string txt = menuItem.Text.ToLower();
                    string name = menuItem.Name.ToLower();

                    if (name == "opentoolstripmenuitem1" || (txt.Contains("open") && !name.Equals("opentoolstripmenuitem")))
                    {
                        menuItem.Click -= openToolStripMenuItem1_Click;
                        menuItem.Click += openToolStripMenuItem1_Click;
                    }
                    else if (txt.Contains("save") || name.Contains("save"))
                    {
                        menuItem.Click += SaveFile_Click;
                    }
                    else if (txt.Contains("exit") || name.Contains("exit"))
                    {
                        menuItem.Click += (s, e) => this.Close();
                    }
                    else if (txt.Contains("3.10") || txt.Contains("пробіл") || name.Contains("clearextra"))
                    {
                        menuItem.Click += CleanExtraSpaces_Click;
                    }
                    else if (txt.Contains("2.1") || txt.Contains("прізвищ") || name.Contains("namesurname"))
                    {
                        menuItem.Click += FindNames_Click;
                    }
                    else if (txt.Contains("2.9") || txt.Contains("email") || name.Contains("email"))
                    {
                        menuItem.Click += FindEmails_Click;
                    }
                    else if (txt.Contains("2.10") || txt.Contains("номер") || name.Contains("phone"))
                    {
                        menuItem.Click += FindPhoneNumbers_Click;
                    }
                    else if (txt.Contains("загальн") || (txt.Contains("statistic") && !menuItem.HasDropDownItems) || name.Contains("general"))
                    {
                        menuItem.Click += GeneralStats_Click;
                    }
                    else if (txt.Contains("3.2") || txt.Contains("частот") || name.Contains("frequency"))
                    {
                        menuItem.Click += WordFrequency_Click;
                    }
                    else if (txt.Contains("3.3") || txt.Contains("довш") || name.Contains("longest"))
                    {
                        menuItem.Click += Top10Longest_Click;
                    }
                    else if ((txt.Contains("about") || name.Contains("about")) && !menuItem.HasDropDownItems)
                    {
                        menuItem.Click += About_Click;
                    }

                    if (menuItem.HasDropDownItems)
                    {
                        AttachHandlers(menuItem.DropDownItems);
                    }
                }
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Files|*.txt;*.html|All Files|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.Text = File.ReadAllText(ofd.FileName);
                }
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Files|*.txt|All Files|*.*" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, richTextBox1.Text);
                    MessageBox.Show("Файл збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void GeneralStats_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text;
            int charsCount = text.Length;
            int charsNoSpaces = Regex.Replace(text, @"\s+", "").Length;
            int wordsCount = Regex.Matches(text, @"[\p{L}\p{N}]+").Count;
            int linesCount = richTextBox1.Lines.Length;

            string info = $"Символів з пробілами: {charsCount}\n" +
                          $"Символів без пробілів: {charsNoSpaces}\n" +
                          $"Кількість слів: {wordsCount}\n" +
                          $"Кількість рядків: {linesCount}";

            MessageBox.Show(info, "Загальна статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Лабораторна робота №4\nТема: Регулярні вирази в C#", "Про програму");
        }

        private void FindNames_Click(object sender, EventArgs e)
        {
            string pattern = @"\b\p{Lu}\p{Ll}+ \p{Lu}\p{Ll}+\b";
            MatchCollection matches = Regex.Matches(richTextBox1.Text, pattern);

            if (matches.Count > 0)
            {
                var foundNames = matches.Cast<Match>().Select(m => m.Value).Distinct();
                string resultList = string.Join("\n", foundNames);
                MessageBox.Show($"Знайдено осіб ({matches.Count}):\n\n{resultList}", "Результати пошуку");
            }
            else
            {
                MessageBox.Show("Імен та прізвищ не знайдено.", "Пошук");
            }
        }

        private void FindEmails_Click(object sender, EventArgs e)
        {
            string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";
            MatchCollection matches = Regex.Matches(richTextBox1.Text, pattern);

            if (matches.Count > 0)
            {
                var emails = matches.Cast<Match>().Select(m => m.Value).Distinct();
                MessageBox.Show(string.Join("\n", emails), $"Знайдено email ({matches.Count})");
            }
            else
            {
                MessageBox.Show("Email-адрес не знайдено.", "Пошук");
            }
        }

        private void FindPhoneNumbers_Click(object sender, EventArgs e)
        {
            string pattern = @"(?:\+?38)?(?:\(0\d{2}\)|0\d{2})[- ]?\d{3}[- ]?\d{2}[- ]?\d{2}";
            MatchCollection matches = Regex.Matches(richTextBox1.Text, pattern);

            if (matches.Count > 0)
            {
                var phones = matches.Cast<Match>().Select(m => m.Value).Distinct();
                MessageBox.Show(string.Join("\n", phones), $"Знайдено номерів ({matches.Count})");
            }
            else
            {
                MessageBox.Show("Номерів не знайдено.", "Пошук");
            }
        }

        private void WordFrequency_Click(object sender, EventArgs e)
        {
            MatchCollection matches = Regex.Matches(richTextBox1.Text.ToLower(), @"[\p{L}\p{N}]+");

            if (matches.Count == 0)
            {
                MessageBox.Show("Текст порожній.", "Увага");
                return;
            }

            var freq = matches.Cast<Match>()
                              .GroupBy(m => m.Value)
                              .OrderByDescending(g => g.Count())
                              .Take(25)
                              .Select(g => $"{g.Key}: {g.Count()}");

            MessageBox.Show(string.Join("\n", freq), "Частота слів (Топ-25)");
        }

        private void Top10Longest_Click(object sender, EventArgs e)
        {
            MatchCollection matches = Regex.Matches(richTextBox1.Text, @"[\p{L}]+");

            if (matches.Count == 0)
            {
                MessageBox.Show("Слів не знайдено.", "Увага");
                return;
            }

            var top10 = matches.Cast<Match>()
                               .Select(m => m.Value)
                               .Distinct()
                               .OrderByDescending(w => w.Length)
                               .Take(10)
                               .Select((w, i) => $"{i + 1}. {w} ({w.Length} симв.)");

            MessageBox.Show(string.Join("\n", top10), "10 найдовших слів");
        }

        private void CleanExtraSpaces_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();
            foreach (var line in richTextBox1.Lines)
            {
                string cleaned = Regex.Replace(line, @"[ \t]+", " ").Trim();
                if (!string.IsNullOrWhiteSpace(cleaned))
                {
                    result.Add(cleaned);
                }
            }
            richTextBox1.Text = string.Join(Environment.NewLine, result);
            MessageBox.Show("Зайві пробіли та порожні рядки видалено.", "Готово");
        }
    }
}