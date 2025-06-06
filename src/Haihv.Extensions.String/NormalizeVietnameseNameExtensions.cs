using System.Globalization;
using System.Text;

namespace Haihv.Extensions.String;

public static class NormalizeVietnameseNameExtensions
{
    /// <summary>
    /// Chuẩn hóa tên dân tộc để so sánh.
    /// <para>
    /// - Chuyển về chữ thường
    /// - Loại bỏ dấu tiếng Việt
    /// - Loại bỏ dấu cách và ký tự đặc biệt
    /// </para>
    /// </summary>
    /// <param name="name">Tên cần chuẩn hóa</param>
    /// <returns>Tên đã được chuẩn hóa</returns>
    public static string NormalizeVietnameseName(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        // Chuyển về chữ thường
        var normalized = name.ToLowerInvariant();

        // Loại bỏ dấu tiếng Việt
        normalized = RemoveVietnameseDiacritics(normalized);

        // Thay thế các ký tự đặc biệt, dấu nháy, dấu gạch ngang, dấu cách bằng ""
        var sb = new StringBuilder();
        foreach (var c in normalized.Where(char.IsLetterOrDigit))
        {
            sb.Append(c);
        }

        // Một số trường hợp đặc biệt: thay thế các chữ cái viết liền không dấu
        // Ví dụ: "h'mong" -> "hmong", "e de" -> "ede", "o du" -> "odu", "ba na" -> "bana"
        return sb.ToString();
    }

    /// <summary>
    /// Loại bỏ dấu tiếng Việt khỏi chuỗi.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào</param>
    /// <returns>Chuỗi đã loại bỏ dấu</returns>
    private static string RemoveVietnameseDiacritics(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Quy tắc thay thế: e -> ê, d -> đ (chỉ khi là từ đầu tiên hoặc sau khoảng trắng)
        var chars = input.Trim().ToLowerInvariant().ToCharArray();
        var sb = new StringBuilder();
        var isWordStart = true;
        foreach (var c in chars)
        {
            switch (isWordStart)
            {
                case true when c == 'e':
                    sb.Append('ê');
                    break;
                case true when c == 'd':
                    sb.Append('đ');
                    break;
                default:
                    sb.Append(c);
                    break;
            }

            isWordStart = c is ' ' or '\t' or '\n';
        }
        var preprocessed = sb.ToString();

        // Loại bỏ dấu tiếng Việt
        var normalizedString = preprocessed.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in from c in normalizedString
                 let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c)
                 where unicodeCategory != UnicodeCategory.NonSpacingMark
                 select c)
        {
            stringBuilder.Append(c);
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}