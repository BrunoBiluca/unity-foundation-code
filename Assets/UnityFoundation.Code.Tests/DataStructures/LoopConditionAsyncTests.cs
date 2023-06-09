using Moq;
using NUnit.Framework;
using UnityFoundation.TestUtility;

namespace UnityFoundation.Code.Tests
{
    public class LoopConditionAsyncTests
    {
        [Test]
        [Timeout(1000)]
        public async void Should_not_execute_when_condition_is_initially_not_true()
        {
            var action = new ActionTestHelper();

            var alwaysFalse = ConditionEvaluation.Create(() => false);
            await AsyncCondition.While(alwaysFalse).Loop(action.Action);

            Assert.That(action.WasExecuted, Is.False);
        }

        [Test]
        [Timeout(1000)]
        public async void Should_execute_when_condition_is_initially_true()
        {
            var action = new ActionTestHelper();

            var condition = new Mock<ICondition>();
            condition.SetupSequence(condition => condition.Resolve())
                .Returns(true)
                .Returns(false);

            await AsyncCondition.While(condition.Object).Loop(action.Action);

            Assert.That(action.WasExecuted, Is.True);
            Assert.That(action.TimesExecuted, Is.EqualTo(1));
        }

        [Test]
        [Timeout(1000)]
        public async void Should_execute_while_condition_is_true()
        {
            var action = new ActionTestHelper();

            var condition = new Mock<ICondition>();
            condition.SetupSequence(condition => condition.Resolve())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);

            await AsyncCondition.While(condition.Object).Loop(action.Action);

            Assert.That(action.WasExecuted, Is.True);
            Assert.That(action.TimesExecuted, Is.EqualTo(4));
        }
    }
}
