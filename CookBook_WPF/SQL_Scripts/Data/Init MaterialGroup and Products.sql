USE dbCookBook

DELETE FROM tbl_Product
DELETE FROM tbl_MaterialGroup
DELETE FROM tbl_MeasureValue

INSERT INTO tbl_Measure VALUES(N'��', 1)
INSERT INTO tbl_Measure VALUES(N'�', 1)
INSERT INTO tbl_Measure VALUES(N'��', 0)
INSERT INTO tbl_Measure VALUES(N'�.�.', 0)
INSERT INTO tbl_Measure VALUES(N'��.�.', 0)

INSERT INTO tbl_MaterialGroup
VALUES (N'����', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'����', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'�����', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'������', 1 )
INSERT INTO tbl_MaterialGroup
VALUES (N'�������', 1 )
INSERT INTO tbl_MaterialGroup
VALUES (N'������', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'�����', 0 )
GO

INSERT INTO tbl_Product 
VALUES 
(
	N'�������',
	1, 1, 1, 
	 1, 
	(SELECT TOP (1) nKey FROM tbl_MaterialGroup WHERE szGroupName = N'�����')
)
GO

INSERT INTO tbl_Product 
VALUES 
(
	N'������',
	2, 2, 2, 
	 2, 
	(SELECT TOP (1) nKey FROM tbl_MaterialGroup WHERE szGroupName = N'�����')
)
GO