// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

using NUnit.Framework;

using framework.id;

namespace testing.framework.id
{
    public class IdServiceTests : BaseTest
    {
        [Test]
        public void GenerateNewId()
        {
            int startValue = 0;

            IIdService taskIdService = new SimpleIdService (startValue);
            Id taskId = taskIdService.GenerateNewTaskId ();

            Assert.AreEqual (new Id(startValue), taskId);
        }

        [Test]
        public void GenerateNewIdWithOffset()
        {
            int startValue = 10;

            IIdService taskIdService = new SimpleIdService (startValue);
            Id taskId = taskIdService.GenerateNewTaskId ();

            Assert.AreEqual (new Id (startValue), taskId);
        }

        [Test]
        public void GenerateMultipleNewIds()
        {
            int startValue = 0;

            IIdService taskIdService = new SimpleIdService (startValue);
            taskIdService.GenerateNewTaskId ();
            taskIdService.GenerateNewTaskId ();
            Id taskId3 = taskIdService.GenerateNewTaskId ();

            Assert.AreEqual (new Id (startValue + 2), taskId3);
        }
    }
}