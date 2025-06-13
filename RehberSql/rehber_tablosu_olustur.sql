CREATE TABLE [dbo].[rehber] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Adsoyad]  NVARCHAR (25)  NULL,
    [Eposta]   NVARCHAR (50)  NULL,
    [Sehir]    NVARCHAR (30)  NULL,
    [Adres]    NVARCHAR (50)  NULL,
    [Aciklama] NVARCHAR (400) NULL,
    [Ktarihi]  DATETIME       CONSTRAINT [DF_rehber_KTARIHI] DEFAULT (getdate()) NULL,
    [Telno]    NVARCHAR (15)  NULL,
    CONSTRAINT [PK_rehber1] PRIMARY KEY CLUSTERED ([Id] ASC))