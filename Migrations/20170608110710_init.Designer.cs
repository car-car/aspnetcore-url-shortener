using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UrlShortener.Data;

namespace UrlShortener.Migrations
{
    [DbContext(typeof(UrlShortenerContext))]
    [Migration("20170608110710_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("UrlShortener.Models.ShortUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OriginalUrl");
                    b.Property<string>("MetaTitle");
                    b.Property<string>("MetaDescription");
                    b.Property<string>("PreviewImageUrl");
                    b.Property<string>("Provider");
                    b.Property<string>("Memo");
                    b.Property<string>("Path");
                    b.Property<bool>("IsPrivate");
                    b.Property<DateTime>("CreateDate");

                    b.HasKey("Id");

                    b.ToTable("ShortUrls");
                });
        }
    }
}
