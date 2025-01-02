CREATE DATABASE SLIDING_PUZZLE_DB
GO
USE SLIDING_PUZZLE_DB
GO

/* Phan biệt ký tự hoa thường */
ALTER DATABASE SLIDING_PUZZLE_DB COLLATE Latin1_General_CS_AS

/* Tạo TABLEs */
CREATE TABLE PICTURE(
	PICNAME VARCHAR(40) NOT NULL,
	PICPATH VARCHAR(MAX) NOT NULL,
	PLAYERID VARCHAR(6) NOT NULL,
	isDEFAULT BIT
)
GO

CREATE TABLE PLAYER(
	PLAYERID VARCHAR(6) NOT NULL,
	PLAYERNAME VARCHAR(40) NOT NULL,
	PLAYERPASSWORD VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE GAMEROUND(
	GAMEID VARCHAR(6) NOT NULL,
	PLAYERNAME VARCHAR(40) NOT NULL,
	PLAYERID VARCHAR(6) NOT NULL,
	PIECES VARCHAR(5) NOT NULL,
	PLAYTIME VARCHAR(8) NOT NULL,
	PLAYDATE SMALLDATETIME NOT NULL
)
GO
/* Xóa TABLEs nếu tạo cấu trúc sai */
--DROP TABLE PICTURE
--DROP TABLE PLAYER
--DROP TABLE GAMEROUND

/* Thêm khóa chính */
ALTER TABLE PICTURE ADD CONSTRAINT PK_PICTURE PRIMARY KEY (PICNAME, PLAYERID)
ALTER TABLE PLAYER ADD CONSTRAINT PK_PLAYER PRIMARY KEY (PLAYERID)
ALTER TABLE GAMEROUND ADD CONSTRAINT PK_GAMEROUND PRIMARY KEY (GAMEID)
GO
/* thêm ràng buộc duy nhât */
ALTER TABLE PLAYER ADD CONSTRAINT UQ_PLAYER_PLAYERNAME UNIQUE (PLAYERNAME);
GO
/* Thêm khóa ngoại */
ALTER TABLE PICTURE ADD CONSTRAINT FK_PICTURE_PLAYER FOREIGN KEY (PLAYERID) REFERENCES PLAYER(PLAYERID)
ALTER TABLE GAMEROUND ADD CONSTRAINT FK_PLAYERNAME_GAMEROUND_PLAYER FOREIGN KEY (PLAYERNAME) REFERENCES PLAYER(PLAYERNAME)
ALTER TABLE GAMEROUND ADD CONSTRAINT FK_PLAYERID_GAMEROUND_PLAYER FOREIGN KEY (PLAYERID) REFERENCES PLAYER(PLAYERID)
GO

Create trigger CHECK_ID
on GAMEROUND
for insert
as 
begin
	if exists(
	select * from inserted i,GAMEROUND G
	where G.PLAYERID=i.PLAYERID and G.PLAYERNAME=i.PLAYERNAME and G.PIECES=i.PIECES and  G.PLAYTIME <i.PLAYTIME 
	)
	begin
		rollback tran
	end
end
GO

/* Nhập thông tin bảng PLAYER */
 INSERT INTO PLAYER VALUES
	('000000','SlideFun','sildefun'),
	('000001','BAO','bao123@qer'),
	('000002','HOAI','hoai123'),
	('000003','Kass','kasinmidair1207'),
	('000004','Namnguyen9743','NguyenDinhHoaiNam@137')
GO

/* Nhập thông tin bảng PICTURE */
INSERT INTO PICTURE VALUES
('pic1', 'pack://application:,,,/Assets/picture/pic1.png', '000000', 1),
('pic2', 'pack://application:,,,/Assets/picture/pic2.png', '000000', 1),
('pic3', 'pack://application:,,,/Assets/picture/pic3.png', '000000', 1),
('pic4', 'pack://application:,,,/Assets/picture/pic4.png', '000000', 1),
('pic5', 'pack://application:,,,/Assets/picture/pic5.png', '000000', 1),
('pic6', 'pack://application:,,,/Assets/picture/pic6.png', '000000', 1),
('pic7', 'pack://application:,,,/Assets/picture/pic7.png', '000000', 1),
('pic8', 'pack://application:,,,/Assets/picture/pic8.png', '000000', 1),
('pic9', 'pack://application:,,,/Assets/picture/pic9.png', '000000', 1),
('picA', 'pack://application:,,,/Assets/picture/picA.png', '000000', 1),
('picB', 'pack://application:,,,/Assets/picture/picB.png', '000000', 1),
('picC', 'pack://application:,,,/Assets/picture/picC.png', '000000', 1);
GO

/* Nhập thông tin bảng GAMEROUND */
INSERT INTO GAMEROUND  VALUES('000001', 'Kass', '000003', '3 x 3', '00:03:45', '2024-12-25 14:30:00')
INSERT INTO GAMEROUND  VALUES('000002', 'BAO', '000001', '4 x 4', '00:05:30', '2024-12-26 09:15:00')
INSERT INTO GAMEROUND  VALUES('000003', 'HOAI', '000002', '3 x 3', '00:07:50', '2024-12-15 16:45:00')
INSERT INTO GAMEROUND  VALUES('000004', 'Kass', '000003', '5 x 5', '00:30:15', '2024-12-13 11:00:00')
INSERT INTO GAMEROUND  VALUES('000005', 'BAO', '000001', '6 x 6', '01:03:12', '2024-12-21 19:30:00')
INSERT INTO GAMEROUND  VALUES('000006', 'BAO', '000001', '5 x5', '00:35:03', '2024-12-13 19:30:00')
INSERT INTO GAMEROUND  VALUES('000007', 'Kass', '000003', '4 x 4', '00:03:20', '2024-12-05 13:20:11')
INSERT INTO GAMEROUND  VALUES('000008', 'BAO', '000001', '7 x 7', '02:00:00', '2024-12-29 19:30:00')
INSERT INTO GAMEROUND  VALUES('000009', 'BAO', '000001', '9 x 9', '02:50:05', '2024-12-16 22:24:02')
INSERT INTO GAMEROUND  VALUES('000010', 'Namnguyen9743', '000004', '5 x 5', '00:27:35', '2024-12-25 14:30:00')
INSERT INTO GAMEROUND  VALUES('000011', 'Namnguyen9743', '000004', '9 x 9', '01:01:49', '2024-12-25 14:55:00')
INSERT INTO GAMEROUND  VALUES('000012', 'Namnguyen9743', '000004', '3 x 3', '00:00:45', '2024-12-25 14:30:50')
INSERT INTO GAMEROUND  VALUES('000013', 'HOAI', '000002', '8 x 8', '00:57:50', '2024-12-13 22:35:00')
INSERT INTO GAMEROUND  VALUES('000014', 'HOAI', '000002', '5 x 5', '00:04:50', '2024-12-11 16:55:40')
INSERT INTO GAMEROUND  VALUES('000015', 'Kass', '000003', '7 x 7', '00:17:50', '2024-12-22 23:17:03')
INSERT INTO GAMEROUND  VALUES('000016', 'HOAI', '000002', '6 x 6', '00:03:50', '2024-12-29 01:25:01')
GO

/* Xoa toàn bộ các dòng nếu lỡ nhập sai */
--DELETE FROM PLAYER;
--DELETE FROM PICTURE;
--delete from gameround
go
/*in ra bang*/
SELECT * FROM PLAYER
SELECT * FROM PICTURE
SELECT * FROM  GAMEROUND
GO

/*in ra connection string (kiểu địa chỉ để game biết tìm database ở đâu)
copy connection string xong past vào chỗ connStr trong model Connection.cs
thay thể chỗ password bằng mật khẩu của login
*/
select
    'data source=' + @@servername +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=' + suser_name() + ';password=<<YourPassword>>'
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()
GO