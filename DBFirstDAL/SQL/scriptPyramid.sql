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


create table Seo(
	Id int  not null primary key identity,

	[MetaDescription] [nvarchar](1000) NULL,
	[MetaTitle] [nvarchar](1000) NULL,
	[MetaKeywords] [nvarchar](1000) NULL,
	[Alias] [nvarchar](1000) NULL,
	--CategoryId int  NULL  foreign key references [Categories](Id),
)

GO

CREATE TABLE Recommendations(
Id int primary key identity,

Title  nvarchar(100) not null,

Content nvarchar(max), 
 ShortContent nvarchar(500)
)
GO

GO
CREATE TABLE [Categories](
	Id int not null identity primary key,
	[Title] [nvarchar](100) NOT NULL,
	SeoId int  NULL  foreign key references Seo(Id),
	Content nvarchar(max),
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
	[Title] [nvarchar](300) NULL,
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
	
	IsPriority bit not null default 0,
	IsFilled bit not null default 0,
	Content nvarchar(max),
	--ThumbnailId int NULL foreign key references [Images](Id),

	TypeStatusProduct int not null default 0

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
	TypeValue int not null default 0,
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

CREATE TABLE [Reviews](
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
   CREATE TABLE [RecommendationCategories](
	[RecommendationId] int not NULL ,
	[CategoryId] int not NULL  ,
	
	CONSTRAINT pkRecommendationCategories PRIMARY KEY ([RecommendationId], [CategoryId]),

	CONSTRAINT FK_RecommendationCategories_ForMTM FOREIGN KEY ([RecommendationId])     
    REFERENCES  [Recommendations](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_CategoriesRecommendation_ForMTM FOREIGN KEY ([CategoryId])     
    REFERENCES  [Categories](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE
 )
 go
  Create table HomeEntity(
 Id int primary key identity,
 Title nvarchar(500) not null,
 Content nvarchar(max),
 TitleVideoGuide nvarchar(100),
   LinkYouTobe nvarchar(100),
   ThumbnailId int foreign key references Images(Id) on delete cascade,
   CallToAction nvarchar(100)
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
	
	CONSTRAINT pkHomeEntityCategories PRIMARY KEY ([HomeEntityId],[CategoryId]),

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
  --Create table VideoGuide(
  -- HomeEntityId  int primary key foreign key references HomeEntity(Id) ,
  -- SrcVideoThumbnail nvarchar(100),
  -- LinkYouTobe nvarchar(100),
  -- ThumbnailId int foreign key references Images(Id)
  --)
  go


 CREATE TABLE [BannerWithPoints](
	BannerId int not null  primary key,
	[ImageId] int  foreign key references [Images](Id),
	constraint FK_Banner foreign key (BannerId) references HomeEntity(Id) on delete cascade,

 )
 GO
 /****** Object:  Table [dbo].[PointOnImgs]    Script Date: 10.05.2017 11:59:16 ******/

CREATE TABLE [PointOnImgs](
	Id int not null identity primary key,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	BannerId int not null foreign key references BannerWithPoints(BannerId) on delete cascade,
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
	 ProductId int not null foreign key references [Products](Id) on delete cascade,
	 OrderId int not null foreign key references [Orders](Id) on delete cascade,
	 Quantity int not null,
	 CONSTRAINT pkProductOrders PRIMARY KEY (ProductId, OrderId),
)

go

create table HomeEntityWithProducts(
	 ProductId int not null foreign key references [Products](Id) on delete cascade,
	HomeEntityId int not null foreign key references [HomeEntity](Id) on delete cascade,
	 CONSTRAINT pkHomeEntityWithProducts PRIMARY KEY (ProductId, HomeEntityId),
)

go
create table Events(
Id int not null identity primary key,
Title nvarchar(200) not null,
Content nvarchar(max),
ShortContent nvarchar(max),
DateEventStart datetime not null,
DateEventEnd datetime not null,
isActive bit not null,
)

go
create table EventProducts(
	EventId int foreign key references Events(Id) on delete cascade,
	ProductId int foreign key references Products(Id) on delete cascade,
	constraint pkEventProducts primary key (EventId,ProductId)
)
go
create table EventImages(
	EventId int foreign key references Events(Id),
	ImageId int foreign key references Images(Id),
	constraint pkEventImages primary key (EventId),

	CONSTRAINT FK_EventImages_ FOREIGN KEY (EventId)     
    REFERENCES  Events(Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImagesEvent_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE

)

go
alter table Products
add PopularCount int

go

create table FeedBackEmails(
Id int  not null primary key identity,
Email nvarchar(200) not null,
)

go

create table EventBanners(
Id int  not null primary key identity,
ImageId int ,
Title nvarchar(200) not null,

CONSTRAINT FK_EventBannersImage_OtM FOREIGN KEY (ImageId)     
    REFERENCES  Images(Id)   
    ON DELETE CASCADE    
    ON UPDATE CASCADE , 
)



go

create table EventImages(
	EventId int ,
	ImageId int ,
	constraint pkEventImages primary key (EventId),

	CONSTRAINT FK_EventImages_ FOREIGN KEY (EventId)     
    REFERENCES  Events(Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,   
	CONSTRAINT FK_ImagesEvent_ForMTM FOREIGN KEY ([ImageId])     
    REFERENCES  [Images](Id)    
    ON DELETE CASCADE    
    ON UPDATE CASCADE

)

Go



create table GlobalOption
(
Id int  not null primary key identity,
StringKey nvarchar(100) not null unique,
DescriptionKey nvarchar(100) not null ,
IsHtml bit not null,
OptionContent nvarchar(max) 
)	


--create table AllowableEnumValue
--(
--Id int  not null primary key identity,
--ProductId int not null constraint FK_AllowEnumVal_Product foreign key references Products(Id),
--EnumValueId int not null constraint FK_AllowEnumVal_EnumValue foreign key references [EnumValues](Id),
--FilterId int not null constraint FK_AllowEnumVal_Filter foreign key references Filters(Id),

--)
go
create table BannersOnHomePage(
Id int  not null primary key identity,
ImageId int,
Link nvarchar(max),
Title nvarchar(200) not null,
Content nvarchar(max),
CONSTRAINT FK_BannersOnHomePageImage_OtM FOREIGN KEY (ImageId)     
    REFERENCES  Images(Id)   
    ON DELETE CASCADE    
    ON UPDATE CASCADE , 
)

use Pyramid
go
alter table Categories
add ShowCategoryOnSite bit not null default 1
go
alter table Products
alter column Title nvarchar(300)
go
alter table EventBanners
add Link nvarchar(max)

go
alter table Products
add IsNotUnloading1C bit  default 0 not null

go
create table RouteItems(
Id int  not null primary key identity,
FriendlyUrl nvarchar(250) not null,
ControllerName nvarchar(50) not null,
TypeEntity int not null,
ActionName nvarchar(50),
ContentId int
)

ALTER TABLE Products
DROP COLUMN Alias, MetaKeywords,MetaTitle,MetaDescription
go
ALTER TABLE Products
add SeoId int  NULL  foreign key references Seo(Id) on delete set null
go
ALTER TABLE Pages
add SeoId int  NULL  foreign key references Seo(Id) on delete set null

go
--CREATE UNIQUE  NONCLUSTERED INDEX index_for_FriendlyUrl
--    ON RouteItems (FriendlyUrl )
--        INCLUDE ( ControllerName, ActionName,ContentId) 

go
CREATE UNIQUE  NONCLUSTERED INDEX index_for_ContentId
    ON RouteItems (TypeEntity ,ContentId)
        INCLUDE ( FriendlyUrl ) 
go

CREATE UNIQUE  NONCLUSTERED INDEX index_for_FriendlyUrl
    ON RouteItems (FriendlyUrl )
      
go
ALTER TABLE [Events]
add SeoId int  NULL  foreign key references Seo(Id) on delete set null
go
ALTER TABLE Faq
add SeoId int  NULL  foreign key references Seo(Id) on delete set null
go
alter table Orders
add DateTimeOrder datetime2 not null default GetDate()

go
ALTER TABLE Recommendations
add SeoId int  NULL  foreign key references Seo(Id) on delete set null
