CREATE DATABASE SLIDING_PUZZLE_DB
USE SLIDING_PUZZLE_DB

/* Phan biệt ký tự hoa thường */
ALTER DATABASE SLIDING_PUZZLE_DB COLLATE Latin1_General_CS_AS

/* Tạo TABLEs */
CREATE TABLE PICTURE(
	PICNAME VARCHAR(40) NOT NULL,
	PICPATH VARCHAR(MAX) NOT NULL,
	PLAYERID VARCHAR(6) NOT NULL,
	isDEFAULT BIT
)

CREATE TABLE PLAYER(
	PLAYERID VARCHAR(6) NOT NULL,
	PLAYERNAME VARCHAR(40) NOT NULL,
	PLAYERPASSWORD VARCHAR(MAX) NOT NULL
)

CREATE TABLE GAMEROUND(
	GAMEID VARCHAR(6) NOT NULL,
	PLAYERNAME VARCHAR(40) NOT NULL,
	PLAYERID VARCHAR(6) NOT NULL,
	PIECES VARCHAR(5) NOT NULL,
	PLAYTIME VARCHAR(8) NOT NULL,
	PLAYDATE SMALLDATETIME NOT NULL
)

/* Xóa TABLEs nếu tạo cấu trúc sai */
DROP TABLE PICTURE
DROP TABLE PLAYER
DROP TABLE GAMEROUND

/* Thêm khóa chính */
ALTER TABLE PICTURE ADD CONSTRAINT PK_PICTURE PRIMARY KEY (PICNAME, PLAYERID)
ALTER TABLE PLAYER ADD CONSTRAINT PK_PLAYER PRIMARY KEY (PLAYERID)
ALTER TABLE GAMEROUND ADD CONSTRAINT PK_GAMEROUND PRIMARY KEY (GAMEID)

/* thêm ràng buộc duy nhât */
ALTER TABLE PLAYER ADD CONSTRAINT UQ_PLAYER_PLAYERNAME UNIQUE (PLAYERNAME);

/* Thêm khóa ngoại */
ALTER TABLE PICTURE ADD CONSTRAINT FK_PICTURE_PLAYER FOREIGN KEY (PLAYERID) REFERENCES PLAYER(PLAYERID)
ALTER TABLE GAMEROUND ADD CONSTRAINT FK_PLAYERNAME_GAMEROUND_PLAYER FOREIGN KEY (PLAYERNAME) REFERENCES PLAYER(PLAYERNAME)
ALTER TABLE GAMEROUND ADD CONSTRAINT FK_PLAYERID_GAMEROUND_PLAYER FOREIGN KEY (PLAYERID) REFERENCES PLAYER(PLAYERID)



/* Nhập thông tin bảng PICTURE */
INSERT INTO PICTURE VALUES
('babystar', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000001babystar.png', '000001', 0),
('cutegirl', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000003cutegirl.png', '000003', 0),
('gloss', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000004gloss.png', '000004', 0),
('InTheCloud', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000001InTheCloud.png', '000001', 0),
('moon', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000003moon.png', '000003', 0),
('musicGhost', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000003musicGhost.png', '000003', 0),
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
('picC', 'pack://application:,,,/Assets/picture/picC.png', '000000', 1),
('puzzle', 'D:\\Intuitive_programming_Final_Project\\src\\Final_Project\\PuzzleGame\\Assets\\picture\\000004puzzle.png', '000004', 0);



/* Nhập thông tin bảng PLAYER */
 INSERT INTO PLAYER VALUES
	('000000','SlideFun','sildefun'),
	('000001','BAO','bao123@qer'),
	('000002','HOAI','hoai123'),
	('000003','Kass','kasinmidair1207'),
	('000004','Namnguyen9743','NguyenDinhHoaiNam@137')


/* Nhập thông tin bảng GAMEROUND */
INSERT INTO GAMEROUND  VALUES
('000001', 'SlideFun', '000000', '3 x 3', '00:03:45', '2024-12-25 14:30:00'),
('000002', 'BAO', '000001', '4 x 4', '00:05:30', '2024-12-26 09:15:00'),
('000003', 'HOAI', '000002', '3 x 3', '00:07:50', '2024-12-15 16:45:00'),
('000004', 'SlideFun', '000000', '5 x 5', '00:30:15', '2024-12-13 11:00:00'),
('000005', 'BAO', '000001', '6 x 6', '01:03:12', '2024-12-21 19:30:00'),
('000006', 'BAO', '000001', '6 x6', '01:05:03', '2024-12-13 19:30:00'),
('000007', 'BAO', '000001', '6 x 6', '01:30:01', '2024-12-05 13:20:11'),
('000008', 'BAO', '000001', '7 x 7', '02:00:00', '2024-12-29 19:30:00'),
('000009', 'BAO', '000001', '9 x 9', '02:50:05', '2024-12-16 22:24:02'),
('000010', 'Namnguyen9743', '000004', '3 x 3', '00:02:35', '2024-12-25 14:30:00'),
('000011', 'Namnguyen9743', '000004', '3 x 3', '00:01:49', '2024-12-25 14:55:00'),
('000012', 'Namnguyen9743', '000004', '3 x 3', '00:00:45', '2024-12-25 14:30:50')


/* Xoa toàn bộ các dòng nếu lỡ nhập sai */
DELETE FROM PLAYER;
DELETE FROM PICTURE;
delete from gameround

/*in ra bang*/
SELECT * FROM PLAYER
SELECT * FROM PICTURE
SELECT * FROM  GAMEROUND


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