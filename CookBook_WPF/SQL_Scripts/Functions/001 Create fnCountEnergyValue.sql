USE dbCookBook

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/********************************************************************************

  Имя     : 
  fnCountEnergyValue
  
  Возвращает:
		Energy					Энергетическая ценность продукта
		
  История изменений: 
             26.10.2017, VKo, Создание

********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'fnCountEnergyValue') 
		and objectproperty(id, N'IsScalarFunction') = 1)
  execute (N'create function fnCountEnergyValue (@p1 int) returns int as begin return 1 end')
go


ALTER FUNCTION fnCountEnergyValue 
	(@pProtein FLOAT = 0
	,@pFat FLOAT = 0
	,@pCarbo FLOAT = 0
	)
RETURNS FLOAT
AS
BEGIN
	DECLARE @res FLOAT
	SELECT @res = @pProtein * 4 + @pFat * 9 + @pCarbo * 4; 
	RETURN @res
END
GO

