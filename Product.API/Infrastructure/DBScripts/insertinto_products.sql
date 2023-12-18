USE [identity]
GO

INSERT INTO [dbo].[Products]([Name],[Price],[CreatedDate],[UpdatedDate]) VALUES ('product1',3,getdate(),getdate())
INSERT INTO [dbo].[Products]([Name],[Price],[CreatedDate],[UpdatedDate]) VALUES ('product2',5,getdate(),getdate())
GO


