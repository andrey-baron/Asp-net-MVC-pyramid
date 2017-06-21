USE [master]
GO
/****** Object:  Database [Pyramid]    Script Date: 10.05.2017 11:59:16 ******/
CREATE DATABASE [Pyramid]
 
 GO
USE [Pyramid]
GO

CREATE TABLE [Images](
	Id int not null identity primary key,
	[PathInFileSystem] [nvarchar](max) NULL,
	[ImgAlt] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
)




GO



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
	[ProductId] int not NULL foreign key references [Products](Id) on delete cascade,
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
GO

alter table [Images]
add ServerPathImg [nvarchar](max) NULL
GO
alter table Categories
add ParentId int null 
GO
alter table Categories
add FlagRoot bit not null
GO

alter table Categories
add CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentId)     
    REFERENCES  Categories(Id)    
/* add 20.05.17*/
GO
CREATE TABLE [EnumValues](
	Id int not null identity primary key,
	[Key] [nvarchar](100) NULL,
 )
 GO
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
	Id int primary key identity,
	[ProductId] int not NULL ,
	[ImageId] int not NULL  ,
	[TypeImage]int not null,
	

	CONSTRAINT FK_ProductImages_ForMTM FOREIGN KEY ([ProductId])     
    REFERENCES  [Products](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImageProducts_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
 GO
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

CREATE TABLE [Review](
Id int primary key identity,
Name nvarchar(100) not null,

Content nvarchar(max)  not null,
ProductId int not null foreign key references Products(Id) on delete cascade,
Rating int not null,
[DateCreation] [datetime] NOT NULL,
IsRead bit not null,
IsApproved bit not null
)
GO
CREATE TABLE Recommendations(
Id int primary key identity,
CategoryId int foreign key references Categories(Id),

Title  nvarchar(100) not null,

Content nvarchar(max) 

)
GO
CREATE TABLE Faq(
Id int primary key identity,
Title  nvarchar(100) not null,
)
GO
CREATE TABLE QuestionAnswer(
Id int primary key identity,
Question nvarchar(500) not null,
Answer nvarchar(max) ,
FaqId int foreign key references Faq(id)	
)
GO
  CREATE TABLE [RecommendationImages](
	[RecommendationId] int not NULL ,
	[ImageId] int not NULL  ,
	
	CONSTRAINT pkRecommendationImages PRIMARY KEY ([RecommendationId], [ImageId]),

	CONSTRAINT FK_RecommendationImages_ForMTM FOREIGN KEY ([RecommendationId])     
    REFERENCES  [Recommendations](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImageRecommendation_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
 go
  Create table HomeEntity(
 Id int primary key identity,
 Title nvarchar(500) not null,
 Content nvarchar
 )

 GO
 CREATE TABLE [HomeEntityFaq](
	[HomeEntityId] int not NULL ,
	[FaqId] int not NULL  ,
	
	CONSTRAINT pkHomeEntityFaq PRIMARY KEY ([HomeEntityId]),

	CONSTRAINT FK_HomeEntity_ForMTM FOREIGN KEY ([HomeEntityId])     
    REFERENCES  [HomeEntity](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_Faq_ForMTM FOREIGN KEY ([FaqId])     
    REFERENCES  [Faq](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
 GO
 CREATE TABLE [HomeEntityCategories](
	[HomeEntityId] int not NULL ,
	[CategoryId] int not NULL  ,
	
	CONSTRAINT pkHomeEntityCategories PRIMARY KEY ([HomeEntityId]),

	CONSTRAINT FK_HomeEntityCategories_ FOREIGN KEY ([HomeEntityId])     
    REFERENCES  [HomeEntity](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_CategoryHomeEntity_ForMTM FOREIGN KEY ([CategoryId])     
    REFERENCES  [Categories](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
 GO
  Create table VideoGuide(
   Id int primary key identity,
   SrcVideoThumbnail nvarchar(100),
   LinkYouTobe nvarchar(100),
   HomeEntityId int foreign key references HomeEntity(Id)
  )
  go


 CREATE TABLE [BannerWithPoints](
	BannerId int not null  primary key,
	[ImageId] int not null foreign key references [Images](Id),
	constraint FK_Banner foreign key (BannerId) references HomeEntity(Id),

 )
 GO
 /****** Object:  Table [dbo].[PointOnImgs]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [PointOnImgs](
	Id int not null identity primary key,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	BannerId int not null foreign key references BannerWithPoints(BannerId),
	ReferenceProductId int null foreign key references Products(Id) on delete cascade
 )

go

alter table [Products]
add SeasonOffer bit null
GO

alter table [Products]
add OneCId nvarchar(100) null
GO

alter table [Categories]
add OneCId nvarchar(100) null
GO

create table Orders(
	Id int not null identity primary key,
	UserName nvarchar(50) not null,
	Phone nvarchar(20) not null,
	Email nvarchar(100) not null,
	Adress nvarchar(1000),
	TypeProgressOrder int not null,
)
GO

create table ProductOrders(
	 ProductId int not null foreign key references [Products](Id),
	 OrderId int not null foreign key references [Orders](Id),
	 Quantity int not null,
	 CONSTRAINT pkProductOrders PRIMARY KEY (ProductId, OrderId),
)

go

create table HomeEntityWithProducts(
	 ProductId int not null foreign key references [Products](Id) on delete cascade,
	HomeEntityId int not null foreign key references [HomeEntity](Id) on delete cascade,
	 CONSTRAINT pkHomeEntityWithProducts PRIMARY KEY (ProductId, HomeEntityId),
)

--create table Menu(
	
--)