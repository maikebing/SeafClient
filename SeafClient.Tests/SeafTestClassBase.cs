﻿using System;
using System.Threading.Tasks;

namespace SeafileClient.Tests
{
    /// <summary>
    ///     Base class for TestClasses which test the SeafClient library
    ///     which defines some useful test properties and functions
    /// </summary>
    public class SeafTestClassBase
    {
        public SeafTestClassBase()
        {
            DummyServerUri = new Uri("https://www.test.test:4444/", UriKind.Absolute);
            FakeToken = "24fd3c026886e3121b2ca630805ed425c272cb96";
            FakeRepoId = "632ab8a8-ecf9-4435-93bf-f495d5bfe975";
            TestConnection = new SeafHttpConnection();
            FakeServerVersion = new Version(6, 0, 0);
        }

        public Uri DummyServerUri { get; }

        public string FakeToken { get; }

        public string FakeRepoId { get; }

        public Version FakeServerVersion { get; }

        public SeafHttpConnection TestConnection { get; }

        /// <summary>
        ///     Execute the given async function synchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T ExecuteSync<T>(Func<Task<T>> func)
        {
            var task = func();
            task.Wait();

            return task.Result;
        }
    }
}