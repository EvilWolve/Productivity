// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

using game.taskmanagement.services;
using game.taskmanagement.data;
using framework.id;

namespace testing.game.taskmanagement
{
    public class TaskLibraryTests : BaseTest
    {
        ITaskLibrary taskLibrary;

        [SetUp]
        public new void Setup()
        {
            this.taskLibrary = new TaskLibrary ();
        }

        [Test]
        public void AddAndGetTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test"
            };

            this.taskLibrary.AddTaskDefinition (definition);

            TaskDefinition fetchedTaskDefinition = this.taskLibrary.GetTaskDefinition (definition.id);

            Assert.IsNotNull (fetchedTaskDefinition);
            Assert.AreEqual (definition.name, fetchedTaskDefinition.name);
        }

        [Test]
        public void AddMultipleAndGetTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test1"
            };
            this.taskLibrary.AddTaskDefinition (definition);

            TaskDefinition definition2 = new TaskDefinition ()
            {
                name = "Test2"
            };
            this.taskLibrary.AddTaskDefinition (definition2);

            TaskDefinition fetchedTaskDefinition = this.taskLibrary.GetTaskDefinition (definition.id);

            Assert.IsNotNull (fetchedTaskDefinition);
            Assert.AreEqual (definition.name, fetchedTaskDefinition.name);
        }

        [Test]
        public void TryAddDuplicateTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test"
            };

            this.taskLibrary.AddTaskDefinition (definition);
            this.taskLibrary.AddTaskDefinition (definition);

            LogAssert.Expect (this.logType, "Task definition already has a valid Id, attempting to add duplicates!");
        }

        [Test]
        public void UpdateTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test"
            };

            this.taskLibrary.AddTaskDefinition (definition);

            TaskDefinition newDefinition = new TaskDefinition ()
            {
                name = "New",
                id = definition.id
            };

            this.taskLibrary.UpdateTaskDefinition (newDefinition.id, newDefinition);

            TaskDefinition fetchedTaskDefinition = this.taskLibrary.GetTaskDefinition (definition.id);

            Assert.IsNotNull (fetchedTaskDefinition);
            Assert.AreEqual (newDefinition.name, fetchedTaskDefinition.name);
        }

        [Test]
        public void TryUpdateTaskDefinitionWithNonExistentId()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "New",
                id = new Id(1)
            };

            this.taskLibrary.UpdateTaskDefinition (definition.id, definition);

            LogAssert.Expect (this.logType, "Task Id not found!");
        }

        [Test]
        public void TryUpdateTaskDefinitionWithInvalidId()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "New"
            };

            this.taskLibrary.UpdateTaskDefinition (definition.id, definition);

            LogAssert.Expect (this.logType, "Id is invalid!");
        }

        [Test]
        public void UpdateOrAddExistingTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test"
            };

            this.taskLibrary.AddTaskDefinition (definition);

            TaskDefinition newDefinition = new TaskDefinition ()
            {
                name = "New",
                id = definition.id
            };

            this.taskLibrary.UpdateOrAddTaskDefinition (newDefinition.id, newDefinition);

            TaskDefinition fetchedTaskDefinition = this.taskLibrary.GetTaskDefinition (definition.id);

            Assert.IsNotNull (fetchedTaskDefinition);
            Assert.AreEqual (newDefinition.name, fetchedTaskDefinition.name);
        }

        [Test]
        public void UpdateOrAddNewTaskDefinition()
        {
            Id newId = new Id (0);

            TaskDefinition newDefinition = new TaskDefinition ()
            {
                name = "New",
                id = newId
            };

            this.taskLibrary.UpdateOrAddTaskDefinition (newDefinition.id, newDefinition);

            TaskDefinition fetchedTaskDefinition = this.taskLibrary.GetTaskDefinition (newId);

            Assert.IsNotNull (fetchedTaskDefinition);
            Assert.AreEqual (newDefinition.name, fetchedTaskDefinition.name);
        }

        [Test]
        public void TryUpdateOrAddTaskDefinitionWithInvalidId()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "New"
            };

            this.taskLibrary.UpdateOrAddTaskDefinition (definition.id, definition);

            LogAssert.Expect (this.logType, "Id is invalid!");
        }

        [Test]
        public void RemoveTaskDefinition()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "Test"
            };

            this.taskLibrary.AddTaskDefinition (definition);
            this.taskLibrary.RemoveTaskDefinition (definition.id);
            this.taskLibrary.GetTaskDefinition (definition.id);

            LogAssert.Expect (this.logType, "Id is invalid!");
        }

        [Test]
        public void TryRemoveTaskDefinitionWithNonExistentId()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "New",
                id = new Id (1)
            };

            this.taskLibrary.RemoveTaskDefinition (definition.id);

            LogAssert.Expect (this.logType, "Task Id not found!");
        }

        [Test]
        public void TryRemoveTaskDefinitionWithInvalidId()
        {
            TaskDefinition definition = new TaskDefinition ()
            {
                name = "New"
            };

            this.taskLibrary.RemoveTaskDefinition (definition.id);

            LogAssert.Expect (this.logType, "Id is invalid!");
        }

        [Test]
        public void TryGetTaskDefinitionWithNonExistentId()
        {
            TaskDefinition missingTask = this.taskLibrary.GetTaskDefinition (new Id(1));

            Assert.IsNull (missingTask, "Found task even though we shouldn't have!");
        }

        [Test]
        public void TryGetTaskDefinitionWithInvalidId()
        {
            this.taskLibrary.GetTaskDefinition (Id.INVALID);

            LogAssert.Expect (this.logType, "Id is invalid!");
        }
    }
}