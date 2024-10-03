CREATE OR ALTER PROCEDURE GetBooks 
@Offset int = 0, 
@Take int = 10, 
@SearchText NVARCHAR(100) = '', 
@SortColumn NVARCHAR(128) = '', 
@SortDirection NVARCHAR(4) = 'ASC' 
AS

DECLARE @Query AS NVARCHAR(MAX) = 'SELECT * 
FROM dbo.Books ';

IF(@SearchText <> '')
	SET @Query += '
WHERE
PublicationYear LIKE ''%' + @SearchText + '%''
OR Quantity LIKE ''%' + @SearchText + '%''
OR Title LIKE ''%' + @SearchText + '%''	
OR Author LIKE ''%' + @SearchText + '%''	
OR ISBN LIKE ''%' + @SearchText + '%''	
OR CategoryId LIKE ''%' + @SearchText + '%'' 
';

IF(@SortColumn <> '')
	SET @Query += 'ORDER BY ' + @SortColumn + ' ' + @SortDirection + ' ';
ELSE
	SET @Query += 'ORDER BY Id ';

SET @Query += '
OFFSET ' + CAST(@Offset AS NVARCHAR(20)) + ' ROWS '
SET @Query += '
FETCH NEXT ' + CAST(@Take AS NVARCHAR(20)) + ' ROWS ONLY';

PRINT(@Query);
EXEC(@Query);