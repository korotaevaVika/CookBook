USE dbCookBook;

/********************************************************************************

  ���     : 
  vwRecipeList
  
  ����������:
		RecipeKey	���� �������
		RecipeName	������������ �������
		ProductName	������������ �������� �����
		ProductKey	���� �������� ����� � tbl_Product
		Portion		���������� ������ �������� �����
		Quantity	���������� �������� ����� � �������� �������� �����
		Measure		�� �������� ����� (�� ����������� ����������)
		NumberIngredientsInRecipe	���������� ����������� � �������
		Description	�������� �������

  ������� ���������: 
             29.10.2017, VKo, ��������
			 12.11.2017, VKo, �������� ����� ProductKey, Description
********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'vwRecipeList') 
		and objectproperty(id, N'IsView') = 1)
  execute (N'create view vwRecipeList as select 1 as clm')
go

if not exists (select * from dbo.sysobjects where id = object_id(N'fnNumberIngredientsInRecipe') 
		and objectproperty(id, N'IsScalarFunction') = 1)
  execute (N'create function fnNumberIngredientsInRecipe (@p1 int) returns int as begin return 1 end')
go

if not exists (select * from dbo.sysobjects where id = object_id(N'fnDefaultMeasureForProduct') 
		and objectproperty(id, N'IsScalarFunction') = 1)
  execute (N'create function fnDefaultMeasureForProduct (@p1 int) returns int as begin return 1 end')
go


ALTER FUNCTION fnNumberIngredientsInRecipe 
	(@rcp_nKey INT)
RETURNS INT
AS
BEGIN
	DECLARE @res INT
	SELECT @res = COUNT(*) FROM tbl_Ingredient WHERE nRecipe_nKey = @rcp_nKey
	RETURN @res
END
go

ALTER FUNCTION fnDefaultMeasureForProduct 
	(@prd_nKey INT)
RETURNS NVARCHAR(255)
AS
BEGIN
	DECLARE @res NVARCHAR(255)
	SELECT TOP 1 @res = msr.szMeasureName FROM tbl_Measure msr
	INNER JOIN tbl_MeasureProductRelation rel ON msr.nKey = rel.nMeasure_nKey
	WHERE rel.nProduct_nKey = @prd_nKey AND rel.bIsDefault = 1
	RETURN @res
END
go

ALTER VIEW vwRecipeList
AS
SELECT 
	  rcp.nKey as RecipeKey
	, rcp.szRecipeName as RecipeName
	, prd.szMaterialName as ProductName
	, prd.nKey as ProductKey
	, rcp.rPortion as Portion
	, rcp.rQuantity as Quantity
	, dbo.fnDefaultMeasureForProduct(prd.nKey) as Measure
	, dbo.fnNumberIngredientsInRecipe(rcp.nKey) as NumberIngredientsInRecipe
	, COALESCE(rcp.szDescription, N'') as [Description]
FROM tbl_Recipe rcp
INNER JOIN tbl_Product prd ON prd.nKey = rcp.nProduct_nKey
go
