CREATE DATABASE QuanLyQuanCaFeHoang
GO

USE QuanLyQuanCaFeHoang
GO
-- Các bảng cần tạo
CREATE TABLE TableFood
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Status NVARCHAR(100) -- TRỐNG || CÓ NGƯỜI
)
GO
CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY NOT NULL,
	DisplayName NVARCHAR(100) NOT NULL,
	PassWord NVARCHAR(1000) NOT NULL,
	Type INT NOT NULL
)
GO

CREATE TABLE FoodCategory
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO
CREATE TABLE Food
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	IdFoodCategory INT NOT NULL,
	Price FLOAT NOT NULL DEFAULT 0
	FOREIGN KEY (IdFoodCategory) REFERENCES FoodCategory(Id)
)
GO
CREATE TABLE Bill
(
	Id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	IdTable INT NOT NULL,
	Status INT NOT NULL DEFAULT 0
	FOREIGN KEY (IdTable) REFERENCES TableFood(Id)
)
GO
ALTER TABLE dbo.Bill
ADD discount INT

ALTER TABLE dbo.Bill
ADD TotalPrice FLOAT


CREATE TABLE BillInfo
(
	Id INT IDENTITY PRIMARY KEY,
	IdBill INT NOT NULL,
	IdFood INT NOT NULL,
	Count INT NOT NULL DEFAULT 0
	FOREIGN KEY (IdBill) REFERENCES Bill(Id),
	FOREIGN KEY (IdFood) REFERENCES Food(Id)
)
GO

-- Tạo PROC cho Login
CREATE PROC USP_Login
@UserName NVARCHAR(100), @PassWord NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Account WHERE UserName = @UserName COLLATE SQL_Latin1_General_CP1_CS_AS 
	AND @PassWord = @PassWord COLLATE SQL_Latin1_General_CP1_CS_AS -- phân biệt chữ hoa chữ thường
END
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
Go


ALTER PROC USP_InsertBill
@idTable INT
AS 
BEGIN 
	INSERT INTO dbo.Bill
	(
	    DateCheckIn,
	    DateCheckOut,
	    IdTable,
	    Status,
		discount,
		TotalPrice
	)
	VALUES
	(   GETDATE(), -- DateCheckIn - date
	    NULL, -- DateCheckOut - date
	    @idTable,         -- IdTable - int
	    0,        -- Status - int
		0,
		0
	    )
END

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS 
BEGIN 
	INSERT INTO dbo.BillInfo
	(
	    IdBill,
	    IdFood,
	    Count
	)
	VALUES
	(   @idBill, -- IdBill - int
	    @idFood, -- IdFood - int
	    @count  -- Count - int
	    )
END





ALTER PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS 
BEGIN 
	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT =1

	SELECT @isExitsBillInfo=id, @foodCount = b.count FROM dbo.BillInfo AS b
	WHERE b.IdBill=@idBill AND b.IdFood=@idFood
	IF (@isExitsBillInfo > 1)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count 
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET Count =  @foodCount + @count WHERE IdFood=@idFood
		ELSE
			DELETE dbo.BillInfo WHERE IdBill=@idBill AND IdFood=@idFood
	END
	ELSE
	BEGIN
		INSERT INTO dbo.BillInfo
		(
			IdBill,
			IdFood,
			Count
		)
		VALUES
		(   @idBill, -- IdBill - int
			@idFood, -- IdFood - int
			@count  -- Count - int
			)
	END
END

ALTER TRIGGER UTG_UpdateBillInfo
ON BillInfo FOR INSERT,UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM Inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id =@idBill AND Status=0

	DECLARE @count INT
	SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE idBill=@idBill
	IF(@count>0)
		UPDATE dbo.TableFood SET Status = N'Có' WHERE id = @idTable
	ELSE
        UPDATE dbo.TableFood SET Status = N'Trống' WHERE id = @idTable
END


ALTER TRIGGER UTG_UpdateBill
ON Bill FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = id FROM Inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id =@idBill 
	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable= @idTable AND Status = 0
	IF(@count =0)
		UPDATE dbo.TableFood SET Status = N'Trống' WHERE id =@idTable
	IF(@count =0)
		UPDATE dbo.Bill SET DateCheckOut = GETDATE() WHERE Id =@idBill
END


ALTER PROC USP_SwitchTable
@idTable1 INT , @idTable2 INT
AS
BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT
	DECLARE @isFirstTableEmty INT =1
	DECLARE @isSecondTableEmty INT =1

	SELECT @idFirstBill = id FROM dbo.Bill WHERE IdTable = @idTable1 AND Status = 0
	SELECT @idSecondBill = id FROM dbo.Bill WHERE IdTable = @idTable2 AND Status = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		INSERT INTO dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    IdTable,
		    Status
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    NULL, -- DateCheckOut - date
		    @IdTable1,         -- IdTable - int
		    0          -- Status - int
		    )
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE IdTable = @idTable1 AND Status = 0
	END

	SELECT @isFirstTableEmty = COUNT(*)  FROM dbo.BillInfo WHERE idBill = @idFirstBill

	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT INTO dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    IdTable,
		    Status
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    NULL, -- DateCheckOut - date
		    @IdTable2,         -- IdTable - int
		    0          -- Status - int
		    )
		SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE IdTable = @idTable2 AND Status = 0
	END

	SELECT @isSecondTableEmty = COUNT(*)  FROM dbo.BillInfo WHERE idBill = @idSecondBill

	SELECT id INTO IdBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill

	UPDATE dbo.BillInfo SET IdBill = @idSecondBill WHERE IdBill = @idFirstBill
	UPDATE dbo.BillInfo SET IdBill =@idFirstBill WHERE id IN (SELECT * FROM dbo.IdBillInfoTable)

	DROP TABLE dbo.IdBillInfoTable
	IF(@isFirstTableEmty =0)
		UPDATE dbo.TableFood SET Status=N'Trống' WHERE id = @idTable2
	IF(@isSecondTableEmty =0)
		UPDATE dbo.TableFood SET Status=N'Trống' WHERE id = @idTable1
END

ALTER PROC USP_GetListBillByDate
@dateCheckIn DATE , @dateCheckOut DATE
AS 
BEGIN 
	SELECT t.Name AS [Tên bàn],b.TotalPrice AS [Tổng tiền],b.discount AS [Giảm giá],b.DateCheckIn AS [Ngày vào],
	b.DateCheckOut AS [Ngày ra] FROM dbo.Bill AS b , 
	dbo.TableFood AS t
	WHERE b.IdTable=t.Id
	AND b.DateCheckIn >= @dateCheckIn AND b.DateCheckOut <= @dateCheckOut AND b.Status=1
END 

SELECT t.Name AS [Tên bàn],b.TotalPrice AS [Tổng tiền],b.discount AS [Giảm giá],b.DateCheckIn AS [Ngày vào],
	b.DateCheckOut AS [Ngày ra] FROM dbo.Bill AS b , 
	dbo.TableFood AS t
	WHERE b.IdTable=t.Id 
	AND b.DateCheckIn >= '20200323' AND b.DateCheckOut <= '20200323'  AND b.Status=1

ALTER PROC USP_UpdateAccount
@username nvarchar(100), @displayname nvarchar(100), @password nvarchar(100) , @newpassword nvarchar(100)
AS
BEGIN
	DECLARE @isRightPassword INT =0
	SELECT @isRightPassword=COUNT(*) FROM dbo.Account WHERE UserName = @username AND PassWord = @password

	IF(@isRightPassword = 1)
	BEGIN
		IF(@newpassword = NULL OR @newpassword ='')
		BEGIN	
			UPDATE dbo.Account SET DisplayName=@displayname WHERE UserName=@username AND PassWord=@password 
		END
		ELSE 
		BEGIN
			UPDATE dbo.Account SET DisplayName=@displayname,PassWord = @newpassword  WHERE UserName=@username 
			AND PassWord=@password 
		END
	END
END




CREATE TRIGGER USP_DeleteBillInfo
ON BillInfo FOR DELETE 
AS 
BEGIN 
	DECLARE @idBillInfo INT 
	DECLARE @idBill INT
	SELECT @idBillInfo =id ,@idBill=Deleted.idBill  FROM Deleted

	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id=@idBill

	DECLARE @count INT =0
	SELECT @count=COUNT(*) FROM dbo.BillInfo AS bi ,dbo.Bill AS b WHERE b.Id = bi.IdBill AND b.Id = @idBill AND b.Status=0

	IF(@count = 0)
		UPDATE dbo.TableFood SET Status= N'Trống 'WHERE Id =@idTable
END
-- hàm tìm gần đúng bỏ dấu
CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) 
RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput 
IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) 
DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) 
SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý 
ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) 
SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy 
AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int 
DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 
WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = 
UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + 
SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +
SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET 
@COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput 
END


ALTER TRIGGER USP_DeleteBillInfo
ON BillInfo FOR DELETE 
AS 
BEGIN 
	DECLARE @idBillInfo INT 
	DECLARE @idBill INT
	SELECT @idBillInfo =id ,@idBill=Deleted.idBill  FROM Deleted

	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id=@idBill

	DECLARE @count INT =0
	SELECT @count=COUNT(*) FROM dbo.BillInfo AS bi ,dbo.Bill AS b WHERE b.Id = bi.IdBill AND b.Id = @idBill AND b.Status=0

	IF(@count = 0)
		UPDATE dbo.TableFood SET Status= N'Trống' WHERE Id =@idTable
END

SELECT * FROM dbo.Account

INSERT INTO dbo.Account
(
    UserName,
    DisplayName,
    PassWord,
    Type
)
VALUES
(   N'', -- UserName - nvarchar(100)
    N'', -- DisplayName - nvarchar(100)
    N'', -- PassWord - nvarchar(1000)
    0    -- Type - int
    )