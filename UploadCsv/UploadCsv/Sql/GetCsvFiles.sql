USE [CsvDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[GetCsvFiles];


GO
CREATE PROCEDURE [dbo].[GetCsvFiles]
AS
	SELECT Id, FileName, ContentType, LastModified 
	FROM dbo.CsvFile
RETURN 0
