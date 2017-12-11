/*    ==Параметры сценариев==

    Версия исходного сервера : SQL Server 2016 (13.0.4206)
    Выпуск исходного ядра СУБД : Выпуск Microsoft SQL Server Express Edition
    Тип исходного ядра СУБД : Изолированный SQL Server

    Версия целевого сервера : SQL Server 2017
    Выпуск целевого ядра СУБД : Выпуск Microsoft SQL Server Standard Edition
    Тип целевого ядра СУБД : Изолированный SQL Server
*/

USE [dbCookBook]
GO
/****** Object:  StoredProcedure [dbo].[sp_SelectIngredients]    Script Date: 25.11.2017 22:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'sp_SelectIngredients') 
		and objectproperty(id, N'IsProcedure') = 1)
  execute (N'create procedure sp_SelectIngredients as begin select 0 end')
go


ALTER PROCEDURE [dbo].[sp_SelectIngredients] 
	-- Add the parameters for the stored procedure here
	@pRecipeKey int = 0
AS
BEGIN
	SELECT 
	  ing.nKey as IngredientKey
	, ing.nMeasureProduct_nKey as MeasureProdRelationKey
	, pr.nKey as ProductKey
	, pr.szMaterialName as ProductName
	, msr.nKey as MeasureKey
	, msr.szMeasureName as MeasureName
	, ing.rQuantity as Quantity
	, ROUND(CAST(pr.rEnergy*ing.rQuantity/(100*mpr.rQuantity) as float), 1)  as Energy
	, ROUND(CAST(pr.rProtein*ing.rQuantity/(100*mpr.rQuantity) as float), 1)  as Protein
	, ROUND(CAST(pr.rFat*ing.rQuantity/(100*mpr.rQuantity) as float), 1)  as Fat
	, ROUND(CAST(pr.rCarbohydrate*ing.rQuantity/(100*mpr.rQuantity) as float), 1)  as Carbohydrates

	 FROM  tbl_Ingredient ing
	 INNER JOIN tbl_MeasureProductRelation mpr on mpr.nKey  =  ing.nMeasureProduct_nKey
	 INNER JOIN tbl_Product pr on pr.nKey  = mpr.nProduct_nKey
	 INNER JOIN tbl_Measure msr on msr.nKey  = mpr.nMeasure_nKey

	 WHERE nRecipe_nKey = @pRecipeKey

		
END
