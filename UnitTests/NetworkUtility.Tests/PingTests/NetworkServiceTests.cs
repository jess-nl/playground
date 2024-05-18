using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
using NetworkUtility.Ping;
using System.Net.NetworkInformation;

namespace NetworkUtility.Tests.PingTests
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _pingService;
        private readonly IDNS _dNS;

        public NetworkServiceTests()
        {
            _dNS = A.Fake<IDNS>();
            _pingService = new NetworkService(_dNS);
        }

        [Test]
        public void NetworkService_SendPing_ReturnString()
        {
            A.CallTo(() => _dNS.SendDNS()).Returns(true);

            var result = _pingService.SendPing();

            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [TestCase(1, 1, 2)]
        [TestCase(2, 2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        {
            var result = _pingService.PingTimeout(a, b);

            result.Should().Be(expected);
            result.Should().BeGreaterThanOrEqualTo(2);
            result.Should().NotBeInRange(-1000, 0);
        }

        [Test]
        public void NetworkService_LastPingDate_ReturnDate()
        {
            var result = _pingService.LastPingDate();

            result.Should().BeAfter(1.January(2010));
            result.Should().BeBefore(1.January(2030));
        }

        [Test]
        public void NetworkService_GetPingOptions_ReturnsObject()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            var result = _pingService.GetPingOptions();

            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Test]
        public void NetworkService_GetPingOptions_MostRecentPings()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            var result = _pingService.MostRecentPings();

            result.Should().BeOfType<PingOptions[]>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x => x.DontFragment == true);
            result.Should().Contain(x => x.Ttl == 1);
        }
    }
}
