CREATE DATABASE QuanlySV
USE QuanlySV
GO

CREATE TABLE Lop (
    MaLop CHAR(3) PRIMARY KEY,
    TenLop NVARCHAR(30) NOT NULL
);

CREATE TABLE Sinhvien (
    MaSV CHAR(6) PRIMARY KEY,
    HoTenSV NVARCHAR(40),
    MaLop CHAR(3),
    NgaySinh DATETIME,
    CONSTRAINT FK_Sinhvien_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

INSERT INTO Lop (MaLop, TenLop)
VALUES ('C01', N'Công Nghệ Thông Tin'),
       ('C02', N'Kinh Tế Học');

INSERT INTO Sinhvien (MaSV, HoTenSV, MaLop, NgaySinh)
VALUES ('SV0001', N'Nguyễn Văn A', 'C01', '2002-01-15'),
       ('SV0002', N'Lê Thị B', 'C01', '2003-03-22'),
       ('SV0003', N'Trần Văn C', 'C02', '2004-07-10'),
       ('SV0004', N'Phạm Thị D', 'C02', '2002-11-30');