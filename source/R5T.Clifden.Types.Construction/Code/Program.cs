using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;
using R5T.Norwich;
using R5T.Norwich.Standard;
using R5T.Thessaloniki;
using R5T.Thessaloniki.Default;
using R5T.York;
using R5T.York.Standard;


namespace R5T.Clifden.Types.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.GetPosts();
        }

        private static void GetPosts()
        {
            var serviceProvider = Program.GetServiceProvider();

            var httpJsonSource = serviceProvider.GetRequiredService<IHttpJsonSource>();
            var jsonFileSerializer = serviceProvider.GetRequiredService<IJsonFileSerializer>();
            var temporaryDirectoryPathProvider = serviceProvider.GetRequiredService<ITemporaryDirectoryPathProvider>();
            var stringlyTypedPathOperator = serviceProvider.GetRequiredService<IStringlyTypedPathOperator>();

            var postsJsonUrlEndpoint = "https://jsonplaceholder.typicode.com/posts";

            var posts = httpJsonSource.GetAsync<IEnumerable<Post>>(postsJsonUrlEndpoint).Result;

            var tempDirectoryPath = temporaryDirectoryPathProvider.GetTemporaryDirectoryPath();

            var postsFileName = @"JSONPlaceholder.Posts.json";
            var postsFilePath = stringlyTypedPathOperator.GetFilePath(tempDirectoryPath, postsFileName);

            jsonFileSerializer.SerializeAsync(postsFilePath, posts).Wait();
        }

        private static IServiceProvider GetServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddHttpJsonSource()
                .AddJsonFileSerializer()
                .AddSingleton<ITemporaryDirectoryPathProvider, CTempTemporaryDirectoryPathProvider>()
                .AddSingleton<IStringlyTypedPathOperator, StringlyTypedPathOperator>()
                
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}