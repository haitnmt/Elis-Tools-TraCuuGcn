using System.Globalization;
using System.Text;
using Haihv.Extensions.String;

namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

/// <summary>
/// Danh mục các dân tộc Việt Nam sử dụng trong hệ thống.
/// <para>
/// - Cung cấp các hằng số đại diện cho từng dân tộc.
/// - Hỗ trợ lấy mã theo tên và ngược lại.
/// </para>
/// <para>
/// <b>Danh sách các dân tộc theo Tổng cục Thống kê Việt Nam:</b>
/// <list type="bullet">
/// <item><term>1</term><description>Kinh</description></item>
/// <item><term>2</term><description>Tày</description></item>
/// <item><term>3</term><description>Thái</description></item>
/// <item><term>4</term><description>Hoa</description></item>
/// <item><term>5</term><description>Khmer</description></item>
/// <item><term>6</term><description>Mường</description></item>
/// <item><term>7</term><description>Nùng</description></item>
/// <item><term>8</term><description>H'Mông</description></item>
/// <item><term>9</term><description>Dao</description></item>
/// <item><term>10</term><description>Gia Rai</description></item>
/// <item><term>11</term><description>Ê Đê</description></item>
/// <item><term>12</term><description>Ba Na</description></item>
/// <item><term>13</term><description>Xơ Đăng</description></item>
/// <item><term>14</term><description>Sán Chay</description></item>
/// <item><term>15</term><description>Cơ Ho</description></item>
/// <item><term>16</term><description>Chăm</description></item>
/// <item><term>17</term><description>Sán Dìu</description></item>
/// <item><term>18</term><description>Hrê</description></item>
/// <item><term>19</term><description>Mnông</description></item>
/// <item><term>20</term><description>Ra Glai</description></item>
/// <item><term>21</term><description>Xtiêng</description></item>
/// <item><term>22</term><description>Bru - Vân Kiều</description></item>
/// <item><term>23</term><description>Thổ</description></item>
/// <item><term>24</term><description>Giáy</description></item>
/// <item><term>25</term><description>Cơ Tu</description></item>
/// <item><term>26</term><description>Gié Triêng</description></item>
/// <item><term>27</term><description>Mạ</description></item>
/// <item><term>28</term><description>Khơ Mú</description></item>
/// <item><term>29</term><description>Co</description></item>
/// <item><term>30</term><description>Tà Ôi</description></item>
/// <item><term>31</term><description>Chơ Ro</description></item>
/// <item><term>32</term><description>Kháng</description></item>
/// <item><term>33</term><description>Xinh Mun</description></item>
/// <item><term>34</term><description>Hà Nhì</description></item>
/// <item><term>35</term><description>Chu Ru</description></item>
/// <item><term>36</term><description>Lào</description></item>
/// <item><term>37</term><description>La Chí</description></item>
/// <item><term>38</term><description>La Ha</description></item>
/// <item><term>39</term><description>Phù Lá</description></item>
/// <item><term>40</term><description>La Hủ</description></item>
/// <item><term>41</term><description>Lự</description></item>
/// <item><term>42</term><description>Ngái</description></item>
/// <item><term>43</term><description>Chứt</description></item>
/// <item><term>44</term><description>Lô Lô</description></item>
/// <item><term>45</term><description>Mảng</description></item>
/// <item><term>46</term><description>Pà Thẻn</description></item>
/// <item><term>47</term><description>Cơ Lao</description></item>
/// <item><term>48</term><description>Cống</description></item>
/// <item><term>49</term><description>Bố Y</description></item>
/// <item><term>50</term><description>Si La</description></item>
/// <item><term>51</term><description>Pu Péo</description></item>
/// <item><term>52</term><description>Brâu</description></item>
/// <item><term>53</term><description>Ơ Đu</description></item>
/// <item><term>54</term><description>Rơ măm</description></item>
/// </list>
/// </para>
/// </summary>
public static class DanhMucLoaiDanToc
{
    /// <summary>
    /// Kinh (Mã: 1)
    /// </summary>
    public static readonly LoaiDanToc Kinh = new(1, "Kinh");

    /// <summary>
    /// Tày (Mã: 2)
    /// </summary>
    public static readonly LoaiDanToc Tay = new(2, "Tày");

    /// <summary>
    /// Thái (Mã: 3)
    /// </summary>
    public static readonly LoaiDanToc Thai = new(3, "Thái");

    /// <summary>
    /// Hoa (Mã: 4)
    /// </summary>
    public static readonly LoaiDanToc Hoa = new(4, "Hoa");

    /// <summary>
    /// Khmer (Mã: 5)
    /// </summary>
    public static readonly LoaiDanToc Khmer = new(5, "Khmer");

    /// <summary>
    /// Mường (Mã: 6)
    /// </summary>
    public static readonly LoaiDanToc Muong = new(6, "Mường");

    /// <summary>
    /// Nùng (Mã: 7)
    /// </summary>
    public static readonly LoaiDanToc Nung = new(7, "Nùng");

    /// <summary>
    /// H'Mông (Mã: 8)
    /// </summary>
    public static readonly LoaiDanToc HMong = new(8, "H'Mông");

    /// <summary>
    /// Dao (Mã: 9)
    /// </summary>
    public static readonly LoaiDanToc Dao = new(9, "Dao");

    /// <summary>
    /// Gia Rai (Mã: 10)
    /// </summary>
    public static readonly LoaiDanToc GiaRai = new(10, "Gia Rai");

    /// <summary>
    /// Ê Đê (Mã: 11)
    /// </summary>
    public static readonly LoaiDanToc EDe = new(11, "Ê Đê");

    /// <summary>
    /// Ba Na (Mã: 12)
    /// </summary>
    public static readonly LoaiDanToc BaNa = new(12, "Ba Na");

    /// <summary>
    /// Xơ Đăng (Mã: 13)
    /// </summary>
    public static readonly LoaiDanToc XoDang = new(13, "Xơ Đăng");

    /// <summary>
    /// Sán Chay (Mã: 14)
    /// </summary>
    public static readonly LoaiDanToc SanChay = new(14, "Sán Chay");

    /// <summary>
    /// Cơ Ho (Mã: 15)
    /// </summary>
    public static readonly LoaiDanToc CoHo = new(15, "Cơ Ho");

    /// <summary>
    /// Chăm (Mã: 16)
    /// </summary>
    public static readonly LoaiDanToc Cham = new(16, "Chăm");

    /// <summary>
    /// Sán Dìu (Mã: 17)
    /// </summary>
    public static readonly LoaiDanToc SanDiu = new(17, "Sán Dìu");

    /// <summary>
    /// Hrê (Mã: 18)
    /// </summary>
    public static readonly LoaiDanToc Hre = new(18, "Hrê");

    /// <summary>
    /// Mnông (Mã: 19)
    /// </summary>
    public static readonly LoaiDanToc Mnong = new(19, "Mnông");

    /// <summary>
    /// Ra Glai (Mã: 20)
    /// </summary>
    public static readonly LoaiDanToc RaGlai = new(20, "Ra Glai");

    /// <summary>
    /// Xtiêng (Mã: 21)
    /// </summary>
    public static readonly LoaiDanToc Xtieng = new(21, "Xtiêng");

    /// <summary>
    /// Bru - Vân Kiều (Mã: 22)
    /// </summary>
    public static readonly LoaiDanToc BruVanKieu = new(22, "Bru - Vân Kiều");

    /// <summary>
    /// Thổ (Mã: 23)
    /// </summary>
    public static readonly LoaiDanToc Tho = new(23, "Thổ");

    /// <summary>
    /// Giáy (Mã: 24)
    /// </summary>
    public static readonly LoaiDanToc Giay = new(24, "Giáy");

    /// <summary>
    /// Cơ Tu (Mã: 25)
    /// </summary>
    public static readonly LoaiDanToc CoTu = new(25, "Cơ Tu");

    /// <summary>
    /// Gié Triêng (Mã: 26)
    /// </summary>
    public static readonly LoaiDanToc GieTrieng = new(26, "Gié Triêng");

    /// <summary>
    /// Mạ (Mã: 27)
    /// </summary>
    public static readonly LoaiDanToc Ma = new(27, "Mạ");

    /// <summary>
    /// Khơ Mú (Mã: 28)
    /// </summary>
    public static readonly LoaiDanToc KhoMu = new(28, "Khơ Mú");

    /// <summary>
    /// Co (Mã: 29)
    /// </summary>
    public static readonly LoaiDanToc Co = new(29, "Co");

    /// <summary>
    /// Tà Ôi (Mã: 30)
    /// </summary>
    public static readonly LoaiDanToc TaOi = new(30, "Tà Ôi");

    /// <summary>
    /// Chơ Ro (Mã: 31)
    /// </summary>
    public static readonly LoaiDanToc ChoRo = new(31, "Chơ Ro");

    /// <summary>
    /// Kháng (Mã: 32)
    /// </summary>
    public static readonly LoaiDanToc Khang = new(32, "Kháng");

    /// <summary>
    /// Xinh Mun (Mã: 33)
    /// </summary>
    public static readonly LoaiDanToc XinhMun = new(33, "Xinh Mun");

    /// <summary>
    /// Hà Nhì (Mã: 34)
    /// </summary>
    public static readonly LoaiDanToc HaNhi = new(34, "Hà Nhì");

    /// <summary>
    /// Chu Ru (Mã: 35)
    /// </summary>
    public static readonly LoaiDanToc ChuRu = new(35, "Chu Ru");

    /// <summary>
    /// Lào (Mã: 36)
    /// </summary>
    public static readonly LoaiDanToc Lao = new(36, "Lào");

    /// <summary>
    /// La Chí (Mã: 37)
    /// </summary>
    public static readonly LoaiDanToc LaChi = new(37, "La Chí");

    /// <summary>
    /// La Ha (Mã: 38)
    /// </summary>
    public static readonly LoaiDanToc LaHa = new(38, "La Ha");

    /// <summary>
    /// Phù Lá (Mã: 39)
    /// </summary>
    public static readonly LoaiDanToc PhuLa = new(39, "Phù Lá");

    /// <summary>
    /// La Hủ (Mã: 40)
    /// </summary>
    public static readonly LoaiDanToc LaHu = new(40, "La Hủ");

    /// <summary>
    /// Lự (Mã: 41)
    /// </summary>
    public static readonly LoaiDanToc Lu = new(41, "Lự");

    /// <summary>
    /// Ngái (Mã: 42)
    /// </summary>
    public static readonly LoaiDanToc Ngai = new(42, "Ngái");

    /// <summary>
    /// Chứt (Mã: 43)
    /// </summary>
    public static readonly LoaiDanToc Chut = new(43, "Chứt");

    /// <summary>
    /// Lô Lô (Mã: 44)
    /// </summary>
    public static readonly LoaiDanToc LoLo = new(44, "Lô Lô");

    /// <summary>
    /// Mảng (Mã: 45)
    /// </summary>
    public static readonly LoaiDanToc Mang = new(45, "Mảng");

    /// <summary>
    /// Pà Thẻn (Mã: 46)
    /// </summary>
    public static readonly LoaiDanToc PaThen = new(46, "Pà Thẻn");

    /// <summary>
    /// Cơ Lao (Mã: 47)
    /// </summary>
    public static readonly LoaiDanToc CoLao = new(47, "Cơ Lao");

    /// <summary>
    /// Cống (Mã: 48)
    /// </summary>
    public static readonly LoaiDanToc Cong = new(48, "Cống");

    /// <summary>
    /// Bố Y (Mã: 49)
    /// </summary>
    public static readonly LoaiDanToc BoY = new(49, "Bố Y");

    /// <summary>
    /// Si La (Mã: 50)
    /// </summary>
    public static readonly LoaiDanToc SiLa = new(50, "Si La");

    /// <summary>
    /// Pu Péo (Mã: 51)
    /// </summary>
    public static readonly LoaiDanToc PuPeo = new(51, "Pu Péo");

    /// <summary>
    /// Brâu (Mã: 52)
    /// </summary>
    public static readonly LoaiDanToc Brau = new(52, "Brâu");

    /// <summary>
    /// Ơ Đu (Mã: 53)
    /// </summary>
    public static readonly LoaiDanToc ODu = new(53, "Ơ Đu");

    /// <summary>
    /// Rơ măm (Mã: 54)
    /// </summary>
    public static readonly LoaiDanToc RoMam = new(54, "Rơ măm");

    /// <summary>
    /// Danh sách tất cả các dân tộc.
    /// <para>
    /// Danh sách này chứa tất cả các dân tộc được định nghĩa trong hệ thống theo thứ tự mã số.
    /// </para>
    /// </summary>
    public static readonly IReadOnlyList<LoaiDanToc> TatCa =
    [
        Kinh, Tay, Thai, Hoa, Khmer, Muong, Nung, HMong, Dao, GiaRai,
        EDe, BaNa, XoDang, SanChay, CoHo, Cham, SanDiu, Hre, Mnong, RaGlai,
        Xtieng, BruVanKieu, Tho, Giay, CoTu, GieTrieng, Ma, KhoMu, Co, TaOi,
        ChoRo, Khang, XinhMun, HaNhi, ChuRu, Lao, LaChi, LaHa, PhuLa, LaHu,
        Lu, Ngai, Chut, LoLo, Mang, PaThen, CoLao, Cong, BoY, SiLa,
        PuPeo, Brau, ODu, RoMam
    ];
    
    /// <summary>
    /// Lấy mã dân tộc theo tên.
    /// </summary>
    /// <para>
    /// Trả về mã tương ứng với tên dân tộc. Nếu không tìm thấy sẽ trả về null.
    /// </para>
    /// <para>
    /// Phương thức này không phân biệt chữ hoa/thường, bỏ qua dấu cách và ký tự đặc biệt,
    /// đồng thời chuẩn hóa các ký tự tiếng Việt (ví dụ: â, ă = a).
    /// </para>
    public static int? GetMaByTen(string ten)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return null;

        var normalizedInput = ten.NormalizeVietnameseName();
        return TatCa.FirstOrDefault(x => x.Ten.NormalizeVietnameseName() == normalizedInput)?.Ma;
    }

    /// <summary>
    /// Lấy tên dân tộc theo mã.
    /// <para>
    /// Trả về tên tương ứng với mã dân tộc. Nếu không tìm thấy sẽ trả về null.
    /// </para>
    /// </summary>
    /// <param name="ma">Mã dân tộc</param>
    /// <returns>Tên dân tộc tương ứng hoặc null nếu không tìm thấy</returns>
    public static string? GetTenByMa(int ma)
    {
        return TatCa.FirstOrDefault(x => x.Ma == ma)?.Ten;
    }
}