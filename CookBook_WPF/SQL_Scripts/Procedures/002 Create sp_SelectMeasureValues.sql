USE dbCookBook

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/********************************************************************************

  Имя     : 
  sp_SelectMeasureValues
  
  Параметры:
		

  Возвращает:
		NeasureKey				Ключ ЕИ
		NeasureName				Название ЕИ
		IsDefault				Является ли основной
		

  История изменений: 
             30.10.2017, VKo, Создание

********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'sp_SelectMeasureValues') 
		and objectproperty(id, N'IsProcedure') = 1)
  execute (N'create procedure sp_SelectMeasureValues as begin return end')
go

ALTER PROCEDURE sp_SelectMeasureValues 
	-- Add the parameters for the stored procedure here
	 @pProductKey int = 0
	,@pFindDefault bit = 0
AS
BEGIN
	IF (@pProductKey <> 0)
	BEGIN
		IF (@pFindDefault = 0)
		BEGIN
			SELECT 
				msr.nKey as MeasureKey
				, rel.nKey as MeasureRelationKey
				, msr.szMeasureName as MeasureName
				, rel.bIsDefault as IsDefault 
				, rel.rQuantity as Quantity
				FROM  tbl_Measure msr
				INNER JOIN tbl_MeasureProductRelation rel ON msr.nKey = rel.nMeasure_nKey 
				WHERE rel.nProduct_nKey = @pProductKey
				ORDER BY rel.bIsDefault DESC
		END
		ELSE 
			BEGIN
				SELECT TOP (1)
				msr.nKey as MeasureKey
				, msr.szMeasureName as MeasureName
				FROM  tbl_Measure msr
				INNER JOIN tbl_MeasureProductRelation rel ON msr.nKey = rel.nMeasure_nKey 
				WHERE rel.nProduct_nKey = @pProductKey AND rel.bIsDefault = 1
			END
	END
	ELSE
		BEGIN
			SELECT 
			  nKey as MeasureKey
			, szMeasureName as MeasureName
			, bIsDefault as IsDefault FROM  tbl_Measure 
			ORDER BY bIsDefault DESC
		END
END
GO

