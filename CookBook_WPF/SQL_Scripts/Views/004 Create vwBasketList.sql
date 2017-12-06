USE dbCookBook;

/********************************************************************************

  ���     : 
  vwBasketList
  
  ����������:
		BusketKey	
		Date		
		Description	
		
  ������� ���������: 
             04.12.2017, VKo, ��������
********************************************************************************/
if not exists (select * from dbo.sysobjects where id = object_id(N'vwBasketList') 
		and objectproperty(id, N'IsView') = 1)
  execute (N'create view vwBasketList as select 1 as clm')
go


ALTER VIEW vwBasketList
AS
SELECT 
	  bsk.nKey as BasketKey
	, bsk.tDate as Date
	, bsk.szDescription as Description
	, CONCAT(bsk.tDate, N' ', bsk.szDescription ) as szDisplayText
FROM tbl_Basket bsk
go

