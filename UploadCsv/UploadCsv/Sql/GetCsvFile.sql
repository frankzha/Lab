USE [CsvDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[GetCsvFile];


GO
CREATE PROCEDURE [dbo].[GetCsvFile]
	@id INT
AS
	SELECT FileName, ContentType, FileContent 
	FROM dbo.CsvFile 
	WHERE Id=@id
RETURN 0
