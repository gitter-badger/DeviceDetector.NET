﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using DeviceDetector.NET.Parser.Client;
using DeviceDetector.NET.Tests.Class.Client;
using DeviceDetector.NET.Yaml;

namespace DeviceDetector.NET.Tests.Parser.Client
{
    [Trait("Category", "FeedReader")]
    public class FeedReaderTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public FeedReaderTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\feed_reader.yml"}";

            var parser = new YamlParser<List<ClientFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.client.version = f.client.version ?? "";
                return f;
            }).ToList();
        }

        [Fact]
        public void FeedReaderTestParse()
        {
            var feedReaderParser = new FeedReaderParser();
            foreach (var fixture in _fixtureData)
            {
                feedReaderParser.SetUserAgent(fixture.user_agent);
                var result = feedReaderParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.ShouldBeEquivalentTo(fixture.client.name,"Names should be equal");
                result.Match.Type.ShouldBeEquivalentTo(fixture.client.type, "Types should be equal");
                result.Match.Version.ShouldBeEquivalentTo(fixture.client.version, "Versions should be equal");
            }
        }
    }
}
