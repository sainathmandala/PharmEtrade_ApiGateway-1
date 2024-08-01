using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

public class S3Helper
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Helper(IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");

        _s3Client = new AmazonS3Client(
            awsOptions["AccessKey"],
            awsOptions["SecretKey"],
            RegionEndpoint.GetBySystemName(awsOptions["Region"])
        );

        _bucketName = awsOptions["BucketName"];
    }

    public async Task<string> UploadFileAsync(Stream inputStream, string folderName, string fileName)
    {
        // Ensure the bucket exists or create it
        await EnsureBucketExistsAsync(_bucketName);

        var key = $"{folderName}/{fileName}";

        var transferUtility = new TransferUtility(_s3Client);
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = inputStream,
            Key = key,
            BucketName = _bucketName
            //CannedACL = S3CannedACL.PublicRead
        };

        await transferUtility.UploadAsync(uploadRequest);

        return $"https://{_bucketName}.s3.{RegionEndpoint.GetBySystemName(_s3Client.Config.RegionEndpoint.SystemName).SystemName}.amazonaws.com/{key}";
    }

    private async Task EnsureBucketExistsAsync(string bucketName)
    {
        try
        {
            var response = await _s3Client.ListBucketsAsync();
            if (!response.Buckets.Exists(b => b.BucketName == bucketName))
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                });
            }
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                throw;
            }
            // If the bucket already exists, we can ignore the Conflict exception
        }
    }
}
