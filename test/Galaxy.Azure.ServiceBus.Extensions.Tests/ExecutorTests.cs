using FluentAssertions;
using Galaxy.Azure.ServiceBus.Extensions.Retry;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Galaxy.Azure.ServiceBus.Extensions.Tests
{
    public class ExecutorTests
    {

        [Fact]
        public async Task Should_catch_the_ex_when_handled()
        {
            var result = false;

            await Executor.Execute<Exception>(async () => throw new Exception(), async _ => result = true);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_not_catch_the_ex_when_handled()
        {
            Func<Task> act = async () => await Executor.Execute<ArgumentOutOfRangeException>(async () => throw new ArgumentNullException(), async _ => { });

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_catch_the_ex_when_execution_func_is_null()
        {
            var result = false;

            await Executor.Execute<Exception>(null, async _ => result = true);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_catch_the_ex_when_predicate_true()
        {
            var result = false;

            await Executor.Execute<Exception>(async () => throw new Exception()
            , async _ => result = true,
            ex => ex is Exception);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_throw_the_ex_when_predicate_false()
        {
            Func<Task> act = async () => await Executor.Execute<Exception>(async () => throw new ArgumentNullException(),
            async _ => { }, ex => ex is ArgumentOutOfRangeException);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
