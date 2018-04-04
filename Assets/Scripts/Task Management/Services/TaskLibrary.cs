// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

using System.Collections.Generic;

using UnityEngine.Assertions;

namespace game.taskmanagement.services
{
    using framework.id;
    using game.taskmanagement.data;

    public interface ITaskLibrary
    {
        void AddTaskDefinition(TaskDefinition taskDefinition);
        void RemoveTaskDefinition(Id taskId);

        void UpdateTaskDefinition(Id taskId, TaskDefinition taskDefinition);
        void UpdateOrAddTaskDefinition(Id taskId, TaskDefinition taskDefinition);

        TaskDefinition GetTaskDefinition(Id taskId);
    }

    public class TaskLibrary : ITaskLibrary
    {
        IIdService idService;

        Dictionary<Id, TaskDefinition> taskDefinitions = new Dictionary<Id, TaskDefinition> ();

        public TaskLibrary()
        {
            this.idService = new SimpleIdService ();

            // TODO: Load data from task library storage
        }

        public void AddTaskDefinition(TaskDefinition taskDefinition)
        {
            Assert.IsFalse (taskDefinition.id.IsValid (), "Task definition already has a valid Id, attempting to add duplicates!");

            Id newTaskId = this.idService.GenerateNewTaskId ();
            taskDefinition.id = newTaskId;

            this.taskDefinitions.Add (newTaskId, taskDefinition);
        }

        public void RemoveTaskDefinition(Id taskId)
        {
            Assert.IsTrue (taskId.IsValid (), "Task Id is invalid!");
            Assert.IsTrue (this.taskDefinitions.ContainsKey (taskId), "Task Id not found!");

            TaskDefinition definition = this.taskDefinitions[taskId];
            Assert.IsNotNull (definition, "Task definition is null!");

            definition.id = Id.INVALID;
            this.taskDefinitions.Remove (taskId);
        }

        public void UpdateTaskDefinition(Id taskId, TaskDefinition taskDefinition)
        {
            Assert.IsTrue (taskId.IsValid (), "Task Id is invalid!");
            Assert.IsTrue (this.taskDefinitions.ContainsKey (taskId), "Task Id not found!");

            this.taskDefinitions[taskId] = taskDefinition;
        }

        public void UpdateOrAddTaskDefinition(Id taskId, TaskDefinition taskDefinition)
        {
            Assert.IsTrue (taskId.IsValid (), "Task Id is invalid!");

            if (this.taskDefinitions.ContainsKey(taskId))
            {
                this.taskDefinitions[taskId] = taskDefinition;
            }
            else
            {
                this.taskDefinitions.Add (taskId, taskDefinition);
            }
        }

        public TaskDefinition GetTaskDefinition(Id taskId)
        {
            Assert.IsTrue (taskId.IsValid (), "Task Id is invalid!");

            if (this.taskDefinitions.ContainsKey (taskId))
            {
                return this.taskDefinitions[taskId];
            }

            return null;
        }
    }
}