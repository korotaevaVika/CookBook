USE dbCookBook

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/********************************************************************************

  ���     : 
  sp_SelectProducts
  
  ����������:
		ProductKey				���� ��������
		ProductName				�������� ��������
		Energy					�������������� ��������
		Protein					�����
		Fat						����
		Carbohydrates			��������

  ������� ���������: 
             24.10.2017, VKo, ��������

********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'sp_SelectProducts') 
		and objectproperty(id, N'IsProcedure') = 1)
  execute (N'create procedure sp_SelectProducts as begin return end')
go

ALTER PROCEDURE sp_SelectProducts 
	-- Add the parameters for the stored procedure here
	@pGroupKey int = 0
AS
BEGIN
	SELECT 
	  nKey as ProductKey
	, szMaterialName as ProductName
	, rEnergy as Energy
	, rProtein as Protein
	, rFat as Fat
	, rCarbohydrate as Carbohydrates FROM  tbl_Product 
	WHERE nMaterialGroup_nKey = @pGroupKey
		
END
GO

