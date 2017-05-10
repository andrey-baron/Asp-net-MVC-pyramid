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
	ThumbnailId int NULL foreign key references [Images](Id),
)

GO
/****** Object:  Table [dbo].[Filters]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [Filters](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NULL,
	CategoryId int not NULL foreign key references [Categories](Id),
)
GO
/****** Object:  Table [dbo].[FilterValues]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [FilterValues](
	Id int not null identity primary key,
	[Value] [nvarchar](max) NULL,
	[Product_Id] [int] NULL,
	[FilterId] int not NULL foreign key references [Filters](Id),
)

go

CREATE TABLE [dbo].[Pages](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[ImageId] [int] NULL,
)

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
	[ImagesId] [int] NULL,
	[ThumbnailImgId] [int] NULL,
	[PointOnImg_Id] [int] NULL,
)
go
CREATE TABLE [ProductCategories](
	Id int not null identity primary key,
	[ProductId] int not NULL foreign key references [Products](Id),
	[CategoryId] int not NULL foreign key references [Categories](Id),
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
