using Microsoft.Extensions.Configuration;
using Moq;

namespace Last.Simple.Tests.Utils
{
    internal static class ConfigurationMockBuilder
    {
        public static IConfiguration Build(string configurationSectionName, Dictionary<string, string> values)
        {
            var configurationMock = new Mock<IConfiguration>();
            var mockConfSection = new Mock<IConfigurationSection>();

            foreach (var item in values)
            {
                mockConfSection.SetupGet(m => m[It.Is<string>(s => s == item.Key)]).Returns(item.Value);
            }
            
            configurationMock.Setup(a => a.GetSection(It.Is<string>(s => s == configurationSectionName))).Returns(mockConfSection.Object);

            return configurationMock.Object;
        }
    }
}
