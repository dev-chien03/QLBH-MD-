using System;
using System.Drawing;
using System.Windows.Forms;

namespace main
{
    /// <summary>
    /// UI Configuration and Styling Helper
    /// </summary>
    public static class UIConfiguration
    {
        // Color Scheme
        public static class Colors
        {
            public static Color Primary = Color.FromArgb(52, 152, 219);      // Blue
            public static Color Success = Color.FromArgb(46, 204, 113);      // Green
            public static Color Warning = Color.FromArgb(241, 196, 15);      // Orange
            public static Color Danger = Color.FromArgb(231, 76, 60);        // Red
            public static Color Secondary = Color.FromArgb(149, 165, 166);   // Gray
            public static Color Background = Color.FromArgb(240, 240, 240);  // Light Gray
            public static Color White = Color.White;
        }

        // Font Styles
        public static class Fonts
        {
            public static Font TitleFont = new Font("Arial", 14, FontStyle.Bold);
            public static Font HeaderFont = new Font("Arial", 12, FontStyle.Bold);
            public static Font NormalFont = new Font("Arial", 10, FontStyle.Regular);
            public static Font SmallFont = new Font("Arial", 9, FontStyle.Regular);
        }

        /// <summary>
        /// Apply consistent styling to a button
        /// </summary>
        public static Button CreateStyledButton(string text, int x, int y, Color bgColor)
        {
            Button btn = new Button()
            {
                Text = text,
                Left = x,
                Top = y,
                Width = 80,
                Height = 30,
                BackColor = bgColor,
                ForeColor = Color.White,
                Font = Fonts.NormalFont,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        /// <summary>
        /// Apply consistent styling to a label
        /// </summary>
        public static Label CreateStyledLabel(string text, int x, int y, int width = 70)
        {
            return new Label()
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Font = Fonts.NormalFont,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        /// <summary>
        /// Apply consistent styling to a textbox
        /// </summary>
        public static TextBox CreateStyledTextBox(int x, int y, int width = 150)
        {
            return new TextBox()
            {
                Left = x,
                Top = y,
                Width = width,
                Font = Fonts.NormalFont
            };
        }

        /// <summary>
        /// Create a styled data grid view
        /// </summary>
        public static DataGridView CreateStyledDataGrid()
        {
            DataGridView dgv = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = Fonts.NormalFont,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(245, 245, 245)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Colors.Primary,
                    ForeColor = Color.White,
                    Font = Fonts.HeaderFont
                }
            };
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }

        /// <summary>
        /// Show success message
        /// </summary>
        public static DialogResult ShowSuccess(string message, string title = "Thành công")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show error message
        /// </summary>
        public static DialogResult ShowError(string message, string title = "Lỗi")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show warning message
        /// </summary>
        public static DialogResult ShowWarning(string message, string title = "Cảnh báo")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show confirmation dialog
        /// </summary>
        public static DialogResult ShowConfirm(string message, string title = "Xác nhận")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }

    /// <summary>
    /// Input Validation Helper
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Check if string is empty or whitespace
        /// </summary>
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Check if string is valid decimal number
        /// </summary>
        public static bool IsValidDecimal(string value)
        {
            return decimal.TryParse(value, out _);
        }

        /// <summary>
        /// Check if string is valid integer
        /// </summary>
        public static bool IsValidInteger(string value)
        {
            return int.TryParse(value, out _);
        }

        /// <summary>
        /// Check if price is valid (non-negative)
        /// </summary>
        public static bool IsValidPrice(string value)
        {
            if (!decimal.TryParse(value, out decimal price))
                return false;
            return price >= 0;
        }

        /// <summary>
        /// Check if quantity is valid (non-negative integer)
        /// </summary>
        public static bool IsValidQuantity(string value)
        {
            if (!int.TryParse(value, out int qty))
                return false;
            return qty >= 0;
        }

        /// <summary>
        /// Check if phone number format is valid
        /// </summary>
        public static bool IsValidPhoneNumber(string value)
        {
            if (IsEmpty(value))
                return false;
            // Simple validation: 10-11 digits
            return value.Length >= 10 && value.Length <= 11;
        }

        /// <summary>
        /// Validate multiple required fields
        /// </summary>
        public static bool ValidateRequired(params string[] fields)
        {
            foreach (var field in fields)
            {
                if (IsEmpty(field))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Format Helper for display
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// Format currency for VND
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Format date to Vietnamese format
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Format datetime to Vietnamese format
        /// </summary>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
