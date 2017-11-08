USE dbCookBook

DELETE FROM tbl_Product
DELETE FROM tbl_MaterialGroup
DELETE FROM tbl_MeasureValue

INSERT INTO tbl_Measure VALUES(N'кг', 1)
INSERT INTO tbl_Measure VALUES(N'г', 1)
INSERT INTO tbl_Measure VALUES(N'шт', 0)
INSERT INTO tbl_Measure VALUES(N'ч.л.', 0)
INSERT INTO tbl_Measure VALUES(N'ст.л.', 0)

INSERT INTO tbl_MaterialGroup
VALUES (N'ћ€со', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'–ыба', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'ќвощи', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N'—алаты', 1 )
INSERT INTO tbl_MaterialGroup
VALUES (N'ƒесерты', 1 )
INSERT INTO tbl_MaterialGroup
VALUES (N'‘рукты', 0 )
INSERT INTO tbl_MaterialGroup
VALUES (N' рупы', 0 )
GO

INSERT INTO tbl_Product 
VALUES 
(
	N'ћорковь',
	1, 1, 1, 
	 1, 
	(SELECT TOP (1) nKey FROM tbl_MaterialGroup WHERE szGroupName = N'ќвощи')
)
GO

INSERT INTO tbl_Product 
VALUES 
(
	N'ќгурец',
	2, 2, 2, 
	 2, 
	(SELECT TOP (1) nKey FROM tbl_MaterialGroup WHERE szGroupName = N'ќвощи')
)
GO