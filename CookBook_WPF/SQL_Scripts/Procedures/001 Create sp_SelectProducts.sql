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
/****** Object:  StoredProcedure [dbo].[sp_SelectProducts]    Script Date: 25.11.2017 22:30:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'sp_SelectProducts') 
		and objectproperty(id, N'IsProcedure') = 1)
  execute (N'create procedure sp_SelectProducts as begin select 0 end')
go


ALTER PROCEDURE [dbo].[sp_SelectProducts] 
	-- Add the parameters for the stored procedure here
	@pGroupKey int = 0, 
	@pRecipeKey int = 0, 
	@pIngredientKey int = 0,
	@pExceptOutputProducts int = 0
AS
BEGIN
	IF (@pGroupKey <> 0)
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
	ELSE
		IF (@pRecipeKey <> 0)
			BEGIN
			SELECT 
		  pr.nKey as ProductKey
		, szMaterialName as ProductName
		, rEnergy as Energy
		, rProtein as Protein
		, rFat as Fat
		, rCarbohydrate as Carbohydrates FROM  tbl_Product pr
		INNER JOIN tbl_MaterialGroup mg ON mg.nKey = pr.nMaterialGroup_nKey
		WHERE 
		mg.bContainsFinishedProduct = 0 AND
		NOT pr.nKey IN 
			(SELECT mpr.nProduct_nKey FROM tbl_Ingredient ing
				INNER JOIN tbl_MeasureProductRelation mpr 
					ON ing.nMeasureProduct_nKey = mpr.nKey
				 WHERE ing.nRecipe_nKey =  @pRecipeKey) 
				OR 
			pr.nKey = (SELECT TOP 1 mpr.nProduct_nKey FROM tbl_Ingredient ing
				INNER JOIN tbl_MeasureProductRelation mpr 
					ON ing.nMeasureProduct_nKey = mpr.nKey
				 WHERE ing.nKey =  @pIngredientKey) 

		END
		ELSE
			BEGIN
				IF (@pExceptOutputProducts <> 0)
			BEGIN
				SELECT 
				  pr.nKey as ProductKey
				, szMaterialName as ProductName
				, rEnergy as Energy
				, rProtein as Protein
				, rFat as Fat
				, rCarbohydrate as Carbohydrates FROM  tbl_Product pr
				INNER JOIN tbl_MaterialGroup mg ON mg.nKey = pr.nMaterialGroup_nKey 
				WHERE mg.bContainsFinishedProduct = 0
			END
			ELSE 
			BEGIN
				SELECT 
				  pr.nKey as ProductKey
				, szMaterialName as ProductName
				, rEnergy as Energy
				, rProtein as Protein
				, rFat as Fat
				, rCarbohydrate as Carbohydrates FROM  tbl_Product pr
			END
			END
		
END
