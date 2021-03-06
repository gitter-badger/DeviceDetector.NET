﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using DeviceDetector.NET.Parser.Client;
using DeviceDetector.NET.Tests.Class.Client;
using DeviceDetector.NET.Yaml;

namespace DeviceDetector.NET.Tests.Parser.Client
{
    [Trait("Category", "Pim")]
    public class PimTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public PimTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\pim.yml"}";

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
        public void MobileAppTestParse()
        {
            var pimParser = new PimParser();
            foreach (var fixture in _fixtureData)
            {
                pimParser.SetUserAgent(fixture.user_agent);
                var result = pimParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.ShouldBeEquivalentTo(fixture.client.name, "Names should be equal");
                result.Match.Type.ShouldBeEquivalentTo(fixture.client.type, "Types should be equal");
                result.Match.Version.ShouldBeEquivalentTo(fixture.client.version, "Versions should be equal");
            }

        }
    }
}
