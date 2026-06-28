using System.Linq;
using NUnit.Framework;
using Shooter.Loading;

namespace Shooter.Tests.Loading
{
    public sealed class DemoLoadingServiceTests
    {
        [Test]
        public void RunToCompletion_EverySecondAttemptFailsAndRetryCanRecover()
        {
            var service = new DemoLoadingService();

            LoadingSnapshot firstAttempt = service.RunToCompletion().Last();
            LoadingSnapshot secondAttempt = service.RetryToCompletion().Last();
            LoadingSnapshot thirdAttempt = service.RunToCompletion().Last();
            LoadingSnapshot fourthAttempt = service.RetryToCompletion().Last();

            Assert.That(firstAttempt.Status, Is.EqualTo(LoadingStatus.Failed));
            Assert.That(firstAttempt.Progress, Is.EqualTo(1f));
            Assert.That(firstAttempt.CanRetry, Is.True);
            Assert.That(secondAttempt.Status, Is.EqualTo(LoadingStatus.Succeeded));
            Assert.That(secondAttempt.Progress, Is.EqualTo(1f));
            Assert.That(secondAttempt.CanRetry, Is.False);
            Assert.That(thirdAttempt.Status, Is.EqualTo(LoadingStatus.Failed));
            Assert.That(thirdAttempt.CanRetry, Is.True);
            Assert.That(fourthAttempt.Status, Is.EqualTo(LoadingStatus.Succeeded));
            Assert.That(fourthAttempt.CanRetry, Is.False);
        }
    }
}
