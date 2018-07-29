using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UrlShortener.Data;

namespace UrlShortener.Migrations
{
    [DbContext(typeof(UrlShortenerContext))]
    partial class UrlShortenerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("UrlShortener.Models.ShortUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();
                    //原始網址
                    b.Property<string>("OriginalUrl");
                    //網址內容的Title
                    b.Property<string>("MetaTitle");
                    //網址內容的Description
                    b.Property<string>("MetaDescription");
                    //預覽圖片
                    b.Property<string>("PreviewImageUrl");
                    //提供者
                    b.Property<string>("Provider");
                    //說明
                    b.Property<string>("Memo");
                    //雜湊
                    b.Property<string>("Path");
                    //是否隱私
                    b.Property<bool>("IsPrivate");
                    //產生日期
                    b.Property<DateTime>("CreateDate");
                    //Key
                    b.HasKey("Id");
                    //Table
                    b.ToTable("ShortUrls");
                });
        }
    }
}
