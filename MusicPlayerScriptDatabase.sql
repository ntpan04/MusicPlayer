
USE AirConditionerShop2024DB;
GO

DROP DATABASE IF EXISTS MusicPlayerManagement;
GO

CREATE DATABASE MusicPlayerManagement;
GO

USE MusicPlayerManagement;
GO

CREATE TABLE Account (
  AccountID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
  UserName VARCHAR(40),
  Password VARCHAR(40)
);
GO

CREATE TABLE PlayList (
  PlayListID INT IDENTITY(1,1) PRIMARY KEY,
  PlayListName VARCHAR(40),
  AccountID INT FOREIGN KEY REFERENCES Account(AccountID)
);
GO

CREATE TABLE Song (
  SongID INT IDENTITY(1,1) PRIMARY KEY,
  SongName NVARCHAR(100),
  Artist NVARCHAR(60),
  FilePath TEXT
);
GO

CREATE TABLE Include (
  SongID INT,
  PlayListID INT,
  FOREIGN KEY (SongID) REFERENCES Song(SongID),
  FOREIGN KEY (PlayListID) REFERENCES PlayList(PlayListID)
);
GO

INSERT INTO Account (UserName, Password)
VALUES 
('John Doe', 'password123'),
('Jane Smith', 'abc123'),
('Alice Johnson', 'alicepass'),
('Bob Brown', 'bobpass');

--INSERT INTO PlayList (PlayListName, AccountID)
--VALUES 
--('Favorite Songs', 1),
--('Favorite Songs', 2),
--('Favorite Songs', 3);

---ADD NEW SONG RESOURCE IN YOUR DEVICE
--INSERT INTO Song (SongName, Artist, FilePath)
--VALUES 
--(N'Đi giữa trời rực rỡ', N'Ngô Lan Hương', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\\DiGiuaTroiRucRoOst.mp3'),
--(N'MỘNG YU', N'AMEE, MCK', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\MONGYU.mp3'),
--(N'Như ngày hôm qua', N'Sơn Tùng MTP', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\NhuNgayHomQua.mp3'),
--(N'Vừa hận vừa yêu', N'Trung Tự', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\VuaHanVuaYeu.mp3'),
--(N'Từng là', N'Vũ Cát Tường', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\TungLa-VuCatTuong-13962415.mp3'),
--(N'Thiên lý ơi', N'J97', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\ThienLyOi-JackJ97-13829746.mp3'),
--(N'Ánh sao và bầu trời', N'T.R.I', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\AnhSaoVaBauTroi-TRI-7085073.mp3'),
--(N'Tình cờ yêu em', N'Kuun Đức Nam', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\TinhCoYeuEm-KuunDucNam-9634825.mp3'),
--(N'Waiting for you', N'MONO', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\WaitingForYou-MONOOnionn-7733882.mp3'),
--(N'Tháng tư là lời nói dối của em', N'Hà Anh Tuấn', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\ThangTuLaLoiNoiDoiCuaEm-HaAnhTuan-4609544.mp3'),
--(N'Nắng có mang em về', N'V.A', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\NangCoMangEmVe-VA-14302569.mp3'),
--(N'Nhắn nhủ', N'Ronboogz', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\NhanNhu-Ronboogz-12896660.mp3'),
--(N'ĐỢI', N'52Hz, Rio', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\DOIProdRIO-52HzRio-12890598.mp3'),
--(N'Miên man', N'MinhHuy', N'D:\FINAL\MusicPlayer\MusicPlayer\Music\\MienMan-MinhHuy-7561811.mp3');

--INSERT INTO Include (PlayListID, SongID)
--VALUES 
--(1, 1),
--(1, 2),
--(2, 3),
--(2, 4);
--GO


select * from Account
select * from Song
select * from PlayList
select * from Include