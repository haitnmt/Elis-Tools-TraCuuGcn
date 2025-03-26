namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

/// <summary>
/// Tiện ích mở rộng cho chuỗi, hỗ trợ chuẩn hóa chuỗi đầu vào.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Chuẩn hóa chuỗi bằng cách cắt khoảng trắng thừa và chuyển đổi thành chữ hoa.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào cần chuẩn hóa</param>
    /// <returns>Chuỗi đã được chuẩn hóa hoặc null nếu đầu vào là null</returns>
    public static string? ChuanHoa(this string? input) => input?.Trim().ToUpper() ?? null;

    /// <summary>
    /// Chuẩn hóa chuỗi bằng cách cắt khoảng trắng thừa và chuyển đổi thành chữ thường.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào cần chuẩn hóa</param>
    /// <returns>Chuỗi đã được chuẩn hóa hoặc null nếu đầu vào là null</returns>
    public static string? ChuanHoaLowerCase(this string? input) => input?.Trim().ToLower() ?? null;

    /// <summary>
    /// Chuẩn hóa tên đơn vị hành chính bằng cách chuyển đổi chữ cái đầu tiên thành chữ hoa và các chữ cái còn lại thành chữ thường.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào cần chuẩn hóa</param>
    /// <param name="isLower">Nếu true, chữ cái đầu tiên sẽ được chuyển thành chữ thường.</param>
    /// <returns>Chuỗi đã được chuẩn hóa hoặc null nếu đầu vào là null</returns>
    /// <remarks>
    /// Nếu chuỗi đầu vào có nhiều từ, chỉ chữ cái đầu tiên của từ đầu tiên sẽ được chuyển thành chữ hoa hoặc chữ thường, các từ còn lại sẽ giữ nguyên.
    /// </remarks>
    public static string ChuanHoaTenDonViHanhChinh(this string input, bool isLower = true)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length switch
        {
            0 => string.Empty,
            1 => isLower ? char.ToLower(input[0]) + input[1..] : input,
            _ =>
                $"{(isLower ? char.ToLower(words[0][0]) : char.ToUpper(words[0][0])) + words[0][1..]} {string.Join(' ', words.Skip(1))}"
        };
    }
    
    /// <summary>
    /// Chuyển đổi chữ cái đầu tiên của chuỗi thành chữ hoa.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào cần chuyển đổi</param>
    /// <remarks>
    /// Nếu chuỗi đầu vào có nhiều từ, chỉ chữ cái đầu tiên của từ đầu tiên sẽ được chuyển thành chữ hoa.
    /// </remarks>
    /// <returns></returns>
    public static string VietHoaDauChuoi(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return char.ToUpper(input[0]) + input[1..];
    }
}