USE dbCookBook;

/********************************************************************************

  Имя     : 
  vwMaterialGroupList
  
  Возвращает:
		GroupKey	Ключ группы продуктов
		GroupName	Имя группы продуктов
		NumberProductsInGroup	Количество продуктов в группе
		ContainsFinishedProduct	Содержит ли готовые блюда

  История изменений: 
             24.10.2017, VKo, Создание

********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'vwMaterialGroupList') 
		and objectproperty(id, N'IsView') = 1)
  execute (N'create view vwMaterialGroupList as select 1 as clm')
go

if not exists (select * from dbo.sysobjects where id = object_id(N'fnNumberProductsInGroup') 
		and objectproperty(id, N'IsScalarFunction') = 1)
  execute (N'create function fnNumberProductsInGroup (@p1 int) returns int as begin return 1 end')
go


ALTER FUNCTION fnNumberProductsInGroup 
	(@group_nKey INT)
RETURNS INT
AS
BEGIN
	DECLARE @res INT
	SELECT @res = COUNT(*) FROM tbl_Product WHERE nMaterialGroup_nKey = @group_nKey
	RETURN @res
END
go

ALTER VIEW vwMaterialGroupList
AS
SELECT 
	  nKey as GroupKey
	, szGroupName as GroupName
	, dbo.fnNumberProductsInGroup(grp.nKey) as NumberProductsInGroup
	, bContainsFinishedProduct as ContainsFinishedProduct
FROM tbl_MaterialGroup grp
go
