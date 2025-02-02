﻿namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public record GiayChungNhan(
    long MaGcn,
    long MaDangKy,
    int MaHinhThucSh,
    double DienTichRieng,
    double DienTichChung,
    string Serial,
    DateTime NgayKy,
    string NguoiKy,
    string SoVaoSo
);