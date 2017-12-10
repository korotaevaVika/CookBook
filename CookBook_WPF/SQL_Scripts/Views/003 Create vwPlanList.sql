USE dbCookBook;

/********************************************************************************

  ���     : 
  vwPlanList
  
  ����������:
		PlanKey		���� �����
		rQuantity	���������� �������� ����� � �������� �������� ����� � ����� 
		Description	�������� �������� ����� 
		
		RecipeKey	���� �������
		RecipeName	������������ �������

		ProductKey	���� ����� � tbl_Product
		ProductName	������������ �����

		Portion		���������� ������ �������� �����
		RecipeQuantity	���������� �������� ����� � �������� �������� ����� � �������
		HasBasket	True, ���� ���� ����� � �������

		Measure		�� �������� �����

  ������� ���������: 
             02.12.2017, VKo, ��������
********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'vwPlanList') 
		and objectproperty(id, N'IsView') = 1)
  execute (N'create view vwPlanList as select 1 as clm')
go


ALTER VIEW vwPlanList
AS
SELECT 
	  pln.nKey as PlanKey
	, CONVERT(date, pln.tDate) as tDate
	, pln.rQuantity as rQuantity

	, rcp.nKey as RecipeKey
	, rcp.szRecipeName as RecipeName

	, prd.nKey as ProductKey
	, dbo.fnDefaultMeasureForProduct(prd.nKey) as Measure
	, prd.szMaterialName as ProductName

	, (pln.rQuantity/rcp.rQuantity) as Portion
	, rcp.rQuantity as RecipeQuantity
	, 1 as IsSelected
	, CONCAT(
	prd.szMaterialName, 
	N' �� ������� "' , rcp.szRecipeName , N'", ', pln.rQuantity, 
	 dbo.fnDefaultMeasureForProduct(prd.nKey),
	  N'/', CONVERT(int,  (pln.rQuantity/rcp.rQuantity)) , N'������')
		 as [Description]
	, (CASE
		WHEN pln.Basket_nKey IS NULL
			THEN 0
			ELSE 1
		END) as HasBasket
FROM tbl_Plan pln
INNER JOIN tbl_Recipe rcp ON rcp.nKey = pln.nRecipe_nKey
INNER JOIN tbl_Product prd ON prd.nKey = pln.nProduct_nKey

go

