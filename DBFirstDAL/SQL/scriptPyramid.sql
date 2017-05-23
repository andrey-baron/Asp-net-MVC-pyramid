USE [master]
GO
/****** Object:  Database [Pyramid]    Script Date: 10.05.2017 11:59:16 ******/
CREATE DATABASE [PyramidFinal]
 
 
USE [PyramidFinal]
GO

CREATE TABLE [BannerWithPoints](
	Id int not null identity primary key,
	[PathToImg] [nvarchar](1000) NULL,
 )
 GO
 /****** Object:  Table [dbo].[PointOnImgs]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [PointOnImgs](
	Id int not null identity primary key,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	BannerId int not null foreign key references BannerWithPoints(Id)
 )


GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [Images](
	Id int not null identity primary key,
	[PathInFileSystem] [nvarchar](max) NULL,
	[ImgAlt] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
)
GO
CREATE TABLE [Categories](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NOT NULL,
	--ThumbnailId int NULL foreign key references [Images](Id),
)

GO
/****** Object:  Table [dbo].[Filters]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [Filters](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NULL,
)
GO
/****** Object:  Table [dbo].[FilterValues]    Script Date: 10.05.2017 11:59:16 ******/
/*
CREATE TABLE [FilterValues](
	Id int not null identity primary key,
	[Value] [nvarchar](max) NULL,
	[Product_Id] [int] NULL,
	[FilterId] int not NULL foreign key references [Filters](Id),
)
*/
go

CREATE TABLE [dbo].[Pages](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[ImageId] [int] NULL,
)
--CREATE TABLE [dbo].[ImagesToProducts](
--	Id int not null identity primary key,
--	[ProductId] int not NULL foreign key references [Products](Id),
--	[ImageId] int not NULL foreign key references [Images](Id),
--)
GO
CREATE TABLE [dbo].[Products](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NULL,
	[Price] [float] NOT NULL,
	[TypePrice] [int] NOT NULL,
	[MetaDescription] [nvarchar](1000) NULL,
	[MetaTitle] [nvarchar](1000) NULL,
	[MetaKeywords] [nvarchar](1000) NULL,
	[IsSEOReady] [bit] NOT NULL,
	[Alias] [nvarchar](1000) NULL,
	[DateCreation] [datetime] NOT NULL,
	[DateChange] [datetime] NOT NULL,
	[PointOnImg_Id] [int] NULL,
	--ThumbnailId int NULL foreign key references [Images](Id),
)
go
/*
CREATE TABLE [ProductCategories](
	Id int not null identity primary key,
	[ProductId] int not NULL foreign key references [Products](Id),
	[CategoryId] int not NULL foreign key references [Categories](Id),
 )
 */
CREATE TABLE [ProductCategories](
	/*Id int not null identity primary key,*/
	[ProductId] int not NULL ,
	[CategoryId] int not NULL  ,
	CONSTRAINT pkProductCategories PRIMARY KEY ([ProductId], [CategoryId]),

	CONSTRAINT FK_Products FOREIGN KEY ([ProductId])     
    REFERENCES  [Products](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_Categories FOREIGN KEY ([CategoryId])     
    REFERENCES  Categories(Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
go

CREATE TABLE [dbo].[ProductValues](
	Id int not null identity primary key,
	[ProductId] int not NULL foreign key references [Products](Id),
	[Key] [nvarchar](100) NULL,
	[Value] [nvarchar](max) NULL,
 )

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.05.2017 11:59:16 ******/
CREATE TABLE [dbo].[Users](
	Id int not null identity primary key,
	[Login] [nvarchar](100) NULL,
	[Email] [nvarchar](max) NULL,
	[UserRole] [int] NOT NULL,
	[Password] [nvarchar](max) NULL,
)

GO
/*
alter table [Filters]
alter column   CategoryId int  NULL */

alter table [Products]
add InStock bit null


alter table [Images]
add ServerPathImg [nvarchar](max) NULL

alter table Categories
add ParentId int null 

alter table Categories
add FlagRoot bit not null


alter table Categories
add CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentId)     
    REFERENCES  Categories(Id)    
/* add 20.05.17*/

CREATE TABLE [EnumValues](
	Id int not null identity primary key,
	[Key] [nvarchar](100) NULL,
 )

 CREATE TABLE [ProductEnumValues](
	[ProductId] int not NULL ,
	[EnumValueId] int not NULL  ,
	CONSTRAINT pkProductEnumValues PRIMARY KEY ([ProductId], [EnumValueId]),

	CONSTRAINT FK_Products_ForEnumValue FOREIGN KEY ([ProductId])     
    REFERENCES  [Products](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_EnumValues FOREIGN KEY ([EnumValueId])     
    REFERENCES  [EnumValues](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
go


 CREATE TABLE [FilterCategories](
	[FilterId] int not NULL ,
	[CategoryId] int not NULL  ,
	CONSTRAINT pkFilterCategories PRIMARY KEY ([FilterId], [CategoryId]),

	CONSTRAINT FK_Filter_ForManyToMany FOREIGN KEY ([FilterId])     
    REFERENCES  [Filters](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_Category_ForManyToMany FOREIGN KEY ([CategoryId])     
    REFERENCES  Categories(Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
go


go

 CREATE TABLE [FilterEnumValues](
	[FilterId] int not NULL ,
	[EnumValueId] int not NULL  ,
	CONSTRAINT pkFilterEnumValues PRIMARY KEY ([FilterId], [EnumValueId]),

	CONSTRAINT FK_Filter_ForMTM FOREIGN KEY ([FilterId])     
    REFERENCES  [Filters](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_EnumValue_ForMTM FOREIGN KEY ([EnumValueId])     
    REFERENCES  EnumValues(Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
go


 CREATE TABLE [ProductImages](
	[ProductId] int not NULL ,
	[ImageId] int not NULL  ,
	[TypeImage]int not null,
	CONSTRAINT pkProductImages PRIMARY KEY ([ProductId], [ImageId]),

	CONSTRAINT FK_ProductImages_ForMTM FOREIGN KEY ([ProductId])     
    REFERENCES  [Products](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImageProducts_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )

  CREATE TABLE [CategoryImages](
	[CategoryId] int not NULL ,
	[ImageId] int not NULL  ,
	[TypeImage]int not null,
	CONSTRAINT pkCategoryImages PRIMARY KEY ([CategoryId], [ImageId]),

	CONSTRAINT FK_CategoryImages_ForMTM FOREIGN KEY ([CategoryId])     
    REFERENCES  [Categories](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImageCategories_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
go
