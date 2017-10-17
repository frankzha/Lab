USE [CsvDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[AddCsvFile];


GO
CREATE PROCEDURE [dbo].[AddCsvFile]
	@fileName nvarchar(128),
	@contentType nvarchar(128),
	@fileContent varbinary(MAX)
AS
	INSERT INTO dbo.CsvFile
	([FileName], [ContentType], [FileContent])
	VALUES
	(@fileName, @contentType, @fileContent)
RETURN 0
