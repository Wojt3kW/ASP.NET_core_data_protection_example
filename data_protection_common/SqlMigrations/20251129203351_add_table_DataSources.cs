using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data_protection_common.SqlMigrations
{
    /// <inheritdoc />
    public partial class add_table_DataSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    ContainerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BlobName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    BlobPrefix = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    DatabaseDataSource_ConnectionString = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    DatabaseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Schema = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Query = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CommandTimeoutSeconds = table.Column<int>(type: "int", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Encoding = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Delimiter = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HasHeader = table.Column<bool>(type: "bit", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Port = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    RemotePath = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UseSftp = table.Column<bool>(type: "bit", nullable: true),
                    UsePassiveMode = table.Column<bool>(type: "bit", nullable: true),
                    PrivateKeyPath = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    BucketName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccessKey = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ObjectKey = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UrlDataSource_Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UrlDataSource_Password = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    BearerToken = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    TimeoutSeconds = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSources");
        }
    }
}
